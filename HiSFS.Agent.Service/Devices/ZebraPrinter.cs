using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;
using Zebra.Sdk.Printer.Discovery;

namespace HiSFS.Agent.Service.Devices
{
    public class ZebraPrinter : IDevice
    {
        public bool IsConnected { get; set; }

        public string DeviceName => "Zebra Printer";

        public string ConnectionString { get; set; }

        public String gPrintIp = "172.0.56.100";

        public ZebraPrintLanguage Language
        {
            get
            {
                if (printerLanguage == null)
                    return ZebraPrintLanguage.None;

                if (printerLanguage == PrinterLanguage.ZPL)
                    return ZebraPrintLanguage.ZPL;
                else if (printerLanguage == PrinterLanguage.CPCL)
                    return ZebraPrintLanguage.CPCL;
                else if (printerLanguage == PrinterLanguage.LINE_PRINT)
                    return ZebraPrintLanguage.LINE_PRINT;

                return ZebraPrintLanguage.None;
            }
        }

        private Connection conn;
        private PrinterLanguage printerLanguage;


        public void Close()
        {
            Dispose();
        }

        public void Open()
        {
            DiscoveredUsbPrinter selectedUsbPrinter = null;
            foreach (DiscoveredUsbPrinter usbPrinter in UsbDiscoverer.GetZebraUsbPrinters(new ZebraPrinterFilter()))
            {
                selectedUsbPrinter = usbPrinter;
            }

            if (selectedUsbPrinter != null)
            {
                try
                {
                    conn = selectedUsbPrinter.GetConnection();
                    conn.Open();
                }
                catch (Exception ex)
                {
                    IsConnected = false;
                    Console.WriteLine("UsbPrint Connection Error : " + ex.Message);
                    return;
                }
                printerLanguage = ZebraPrinterFactory.GetInstance(conn).PrinterControlLanguage;

                IsConnected = true;
            }
            else
            {
                IsConnected = false;
            }
        }

        public async void LANOpen(string _uri)
        {
            //var agentSettings = await AgentSettings.LoadAsync(Settings.AgentSettingsPath);
            //var zebraUri = agentSettings.ZebraUri;
            try
            {
                conn = new TcpConnection(_uri, TcpConnection.DEFAULT_ZPL_TCP_PORT);
                //conn = new TcpConnection(gPrintIp, TcpConnection.DEFAULT_ZPL_TCP_PORT);

                // Open the connection - physical connection is established here.
                conn.Open();
            }
            catch (Exception ex)
            {
                IsConnected = false;
                Console.WriteLine("LAN Print Connection Error : " + ex.Message);
                return;
            }
            printerLanguage = ZebraPrinterFactory.GetInstance(conn).PrinterControlLanguage;

            IsConnected = true;
        }

        public void Dispose()
        {
            if (IsConnected == false)
                return;

            conn.Close();

            IsConnected = false;
        }

        /*
		 * Returns the command for a test label depending on the printer control language
		 * The test label is a box with the word "TEST" inside of it
		 * 
		 * _________________________
		 * |                       |
		 * |                       |
		 * |        TEST           |
		 * |                       |
		 * |                       |
		 * |_______________________|
		 * 
		 */
        public byte[] GetConfigLabel()
        {
            byte[] configLabel = null;
            if (printerLanguage == PrinterLanguage.ZPL)
            {
                configLabel = Encoding.UTF8.GetBytes(@"
^XA
^FO17,16
^GB379,371,8
^FS
^FT65,255
^A0N,135,134
^FDTEST
^FS
^XZ");
            }
            else if (printerLanguage == PrinterLanguage.CPCL)
            {
                string cpclConfigLabel = "! 0 200 200 406 1\r\n" + "ON-FEED IGNORE\r\n" + "BOX 20 20 380 380 8\r\n" + "T 0 6 137 177 TEST\r\n" + "PRINT\r\n";
                configLabel = Encoding.UTF8.GetBytes(cpclConfigLabel);
            }
            return configLabel;
        }

        public byte[] GetQRCodeLabel(string barcodeText, string text, int size)
        {
            if (Language != ZebraPrintLanguage.ZPL)
                return null;

            /*
^FO17,16
^GB379,371,3
^FS
^FT65,255

            */

            //var maxTextLength = 12;
            //var textLength = Encoding.GetEncoding(51949).GetBytes(text).Length;
            //var textLength = text.Length;
            //var leftPadding = new string(' ', (maxTextLength - textLength) / 2);
            var leftPadding = "";
            var textsize = 20;
            var text_len = text.Length;
            var tot_len = (text_len / 2) * textsize;
            var text_center = 250 - tot_len;

            var left_size = 0;
            var up_size = 0;
            var down_size = 0;
            var dpi_size = 0;

            //2021.03.05 추가
            if (size == 1)
            {
                left_size = 170;
                up_size = 050;
                dpi_size = 10;
                down_size = 300;
            }
            else if (size == 2)
            {
                left_size = 035;
                up_size = 035;
                dpi_size = 9;
                text_center = 010;
                down_size = 280;
            }
            else
            {
                left_size = 045;
                up_size = 015;
                dpi_size = 6;
                textsize = 10;
                text_center = 020;
                down_size = 160;
                text = "";
            }

            // ^CW1,E: KFONT3.FNT ^ CI26 ^ FS
            //^FO170,050,0 ^ BQN,2,10 ^ FDMM,A{ barcodeText}^FS
            //^FO085,300,0^A1N,45,45^FD{ leftPadding} { text}^FS

            // 작은 용지 - 공백이 있으면 않됨
            //왼쪽여백,상단, , ,DPI(사이즈조정됨:예 10)
            //^FO40,050,0^BQN,2,10^FDMM,A{barcodeText}^FS
            //^FO050,290,0^A1N,20,20^FD{leftPadding}{text}^FS
            var data = Encoding.GetEncoding(51949).GetBytes(
@$"
^XA
^BY2,2.0^FS
^SEE:UHANGUL.DAT^FS
^CW1,E:KFONT3.FNT^CI26^FS

^FO{left_size},{up_size},0^BQN,2,{dpi_size}^FDMM,A{barcodeText}^FS
^FO{text_center},{down_size},0^A1N,{textsize},{textsize}^FD{leftPadding}{text}^FS
^PQ1,1,1,Y^FS
^XZ
"
);
            return data;
        }

        public void Write(byte[] data)
        {
            conn.Write(data);
        }


        /*public void LanWrite(byte[] data)
        {
            conn.Write(data);
        }*/

        public Task WriteAsync(byte[] data)
        {
            return Task.Run(() =>
            {
                Write(data);
            });
        }
    }

    public enum ZebraPrintLanguage
    {
        None,
        ZPL,
        CPCL,
        LINE_PRINT
    }
}
