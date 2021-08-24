using HiSFS.Api.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using HiSFS.Api.Shared.Models.View;
using Syncfusion.XlsIO;
using System.Dynamic;
using Syncfusion.Drawing;
using System.Globalization;
using System.Data;
using System.ComponentModel;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Syncfusion.Blazor.Buttons;

namespace HiSFS.WebApp.Pages.QV.CV
{

    public partial class 품질검사현황Page
    {

        [Inject]
        IJSRuntime JS { get; set; }



        public List<ExpandoObject> CustomerList;

        private void OnExcel(생산지시정보 info)
        {

            //CreateExcel(JSRuntime);
            // 작업지시 상세 페이지로 이동
            //NotifyMessage(Message.SelectedMenu, null, null, "/pm/wo/작업지시상세", null, info.생산지시코드);
        }

        private async Task ExcelExport()
        {
            MemoryStream excelStream;
            string excelVersion = "XLSX";

            using (ExcelEngine excelEngine = new ExcelEngine())
            {

                //CreateService service = new CreateService();
                excelStream = CreateXlsIO(excelVersion);
                if (excelVersion == "XLSX")
                {
                    await JS.SaveAsFileAsync("Sample.xlsx", excelStream.ToArray());
                }
                else
                {
                    await JS.SaveAsFileAsync("Sample.xls", excelStream.ToArray());
                }
            }

        }


        List<생산지시정보엑셀> 생산지시정보_list = new List<생산지시정보엑셀>();
        List<작업지시공정현황엑셀> 작업지시공정현황_list = new List<작업지시공정현황엑셀>();
        List<품질검사측정정보엑셀> 품질검사측정정보현황_list = new List<품질검사측정정보엑셀>();

        private List<생산지시정보엑셀> 엑셀_생산지시정보()
        {
            생산지시정보_list = new List<생산지시정보엑셀>();
            foreach (var item in list)
            {
                생산지시정보_list.Add(new 생산지시정보엑셀()
                {
                    순번 = item.순번,
                    생산지시코드 = item.생산지시코드,
                    생산지시명 = item.생산지시명,
                    생산수량 = item.생산수량,
                    시작일 = item.시작일,
                    완료목표일 = item.완료목표일

                });
            }


            return 생산지시정보_list;
        }

        private List<작업지시공정현황엑셀> 엑셀_작업지시공정현황()
        {

            작업지시공정현황_list = new List<작업지시공정현황엑셀>();
            foreach (var item in 작업지시공정현황목록)
            {
                작업지시공정현황_list.Add(new 작업지시공정현황엑셀()
                {
                    공정차수 = item.공정차수,
                    공정단위코드 = item.공정단위코드,
                    공정명 = item.공정명,
                    공정품명 = item.공정품명,
                    검사수량 = item.검사수량,
                    합격수량 = item.합격수량,
                    불량수량 = item.불량수량,
                    불량률 = item.불량률,
                });
            }
            return 작업지시공정현황_list;
        }


        private List<품질검사측정정보엑셀> 엑셀_품질검사측정정보()
        {

            품질검사측정정보현황_list = new List<품질검사측정정보엑셀>();
            foreach (var item in 품목검사측정현황목록)
            {
                품질검사측정정보현황_list.Add(new 품질검사측정정보엑셀()
                {
                    시리얼넘버 = item.시리얼넘버,
                    생산지시코드 = item.생산지시코드,
                    품질검사코드 = item.품질검사코드,
                    생산품공정명 = item.생산품공정명,
                    생산품공정코드 = item.생산품공정코드,
                    검사기준값 = (decimal)item.검사기준값,
                    오차범위 = (decimal)item.오차범위,
                    검사측정값 = (decimal)item.검사측정값,
                    합격여부 = item.합격여부,
                });
            }
            return 품질검사측정정보현황_list;
        }
        public async Task CreateExcel()
        {
            /*
               작업지시공정현황_list = 엑셀_작업지시공정현황();

               IWorkbook workbook = new XSSFWorkbook();
               var dataFormat = workbook.CreateDataFormat();
               var dataStyle = workbook.CreateCellStyle();
               dataStyle.DataFormat = dataFormat.GetFormat("MM/dd/yyyy HH:mm:ss");


               ISheet worksheet = workbook.CreateSheet("Sheet1");
               int number = 15;
               IRow row = worksheet.CreateRow(number++);

               // 셀에 데이터 포멧 지정
               //var style = workbook.CreateCellStyle();
               //// 날짜 포멧
               //// 정렬 포멧
               //style.Alignment = HorizontalAlignment.Center;
               //style.VerticalAlignment = VerticalAlignment.Top;
               //// 셀 색지정
               //style.FillBackgroundColor = IndexedColors.Gold.Index;
               //// 폰트 설정
               //var font = workbook.CreateFont();
               //font.Color = IndexedColors.Red.Index;
               //cell.CellStyle = dataStyle;


               ICell cell = row.CreateCell(0);
               cell.SetCellValue("No");

               cell = row.CreateCell(1);
               cell.SetCellValue("공정명");

               cell = row.CreateCell(2);
               cell.SetCellValue("공정품명");

               cell = row.CreateCell(3);
               cell.SetCellValue("시작일");

               cell = row.CreateCell(4);
               cell.SetCellValue("완료목표일");

               cell = row.CreateCell(5);
               cell.SetCellValue("목표수량");

               cell = row.CreateCell(6);
               cell.SetCellValue("양산수량");

               cell = row.CreateCell(7);
               cell.SetCellValue("불량수량");

               foreach (var sts in 작업지시공정현황_list)
               {
                   row = worksheet.CreateRow(number++);
                   cell = row.CreateCell(0);
                   cell.SetCellValue(sts.No);

                   cell = row.CreateCell(1);
                   cell.SetCellValue(sts.공정명);

                   cell = row.CreateCell(2);
                   cell.SetCellValue(sts.공정품명);

                   cell = row.CreateCell(3);
                   cell.SetCellValue(sts.시작일.ToString());

                   cell = row.CreateCell(4);
                   cell.SetCellValue(sts.완료목표일.ToString());

                   cell = row.CreateCell(5);
                   cell.SetCellValue(sts.목표수량.ToString());

                   cell = row.CreateCell(6);
                   cell.SetCellValue(sts.양산수량.ToString());

                   cell = row.CreateCell(7);
                   cell.SetCellValue(sts.불량수량.ToString());
               }

               MemoryStream ms = new MemoryStream();
               workbook.Write(ms);

               byte[] bytes = ms.ToArray();
               ms.Close();

               await jSRuntime.SaveAsFileAsync("Student List", bytes, "application/vnd.ms-excel");


               /*
                   byte[] fileContents;
                   ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


                   using (var package = new ExcelPackage())
                   {
                       package.Workbook.Properties.Title = "Test Report";
                       package.Workbook.Properties.Author = "rahul2306";
                       package.Workbook.Properties.Subject = "Test Report";
                       package.Workbook.Properties.Keywords = "Testing";
                       var worksheet = package.Workbook.Worksheets.Add("Employee");
                       //First add the headers
                       worksheet.Cells[1, 1].Value = "생산지시코드";
                       worksheet.Cells[1, 2].Value = "생산지시명";
                       worksheet.Cells[1, 3].Value = "순번";
                       worksheet.Cells[1, 4].Value = "실행상태코드";
                       //Add values
                       var numberformat = "#,##0";
                       var dataCellStyleName = "TableNumber";
                       var numStyle = package.Workbook.Styles.CreateNamedStyle(dataCellStyleName);
                       numStyle.Style.Numberformat.Format = numberformat;
                       worksheet.Cells[2, 1].Value = 1;
                       worksheet.Cells[2, 2].Value = "Rahul";
                       worksheet.Cells[2, 3].Value = "M";
                       worksheet.Cells[2, 4].Value = 50000;
                       worksheet.Cells[2, 4].Style.Numberformat.Format = numberformat;
                       worksheet.Cells[3, 1].Value = 2;
                       worksheet.Cells[3, 2].Value = "Duy";
                       worksheet.Cells[3, 3].Value = "M";
                       worksheet.Cells[3, 4].Value = 50000;
                       worksheet.Cells[3, 4].Style.Numberformat.Format = numberformat;
                       worksheet.Cells[4, 1].Value = 3;
                       worksheet.Cells[4, 2].Value = "Steve";
                       worksheet.Cells[4, 3].Value = "M";
                       worksheet.Cells[4, 4].Value = 45000;
                       worksheet.Cells[4, 4].Style.Numberformat.Format = numberformat;
                       // Add to table / Add summary row
                       var tbl = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 1, fromCol: 1, toRow: 4, toColumn: 4), "Data");
                       tbl.ShowHeader = true;
                       tbl.TableStyle = TableStyles.Dark9;
                       tbl.ShowTotal = true;
                       tbl.Columns[3].DataCellStyleName = dataCellStyleName;
                       tbl.Columns[3].TotalsRowFunction = RowFunctions.Sum;
                       worksheet.Cells[5, 4].Style.Numberformat.Format = numberformat;
                       // AutoFitColumns
                        worksheet.Cells[1, 1, 4, 4].AutoFitColumns();

                       fileContents = package.GetAsByteArray();

                       JSRuntime.InvokeAsync<object>(
                           "saveAsFile",
                           "excelfile.xlsl",
                           Convert.ToBase64String(fileContents)
                       );


                   }
                    */

        }


        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];

                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public DataTable ConvertToDataTable2<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }




        public MemoryStream CreateXlsIO(string version)
        {
            //New instance of XlsIO is created.[Equivalent to launching MS Excel with no workbooks open].
            //The instantiation process consists of two steps.

            //Step 1 : Instantiate the spreadsheet creation engine
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                //Step 2 : Instantiate the excel application object
                IApplication application = excelEngine.Excel;

                //Set the default version
                if (version == "XLSX")
                    application.DefaultVersion = ExcelVersion.Excel2016;
                else
                    application.DefaultVersion = ExcelVersion.Excel97to2003;

                //Creating new workbook
                IWorkbook workbook = application.Workbooks.Create(3);
                IWorksheet sheet = workbook.Worksheets[0];

                //IStyle rowStyle = workbook.Styles.Add("RowStyle");
                //rowStyle.Color = Color.LightGreen;
                //IStyle columnStyle = workbook.Styles.Add("ColumnStyle");
                //columnStyle.Color = Color.Orange;

                //sheet.SetDefaultRowStyle(1, 2, rowStyle);
                //Set default column style for entire column
                //sheet.SetDefaultColumnStyle(1, 2, columnStyle);


                IStyle headerStyle = workbook.Styles.Add("HeaderStyle");
                headerStyle.BeginUpdate();
                headerStyle.Color = Color.FromArgb(255, 174, 33);
                headerStyle.Font.Bold = true;
                headerStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                headerStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                headerStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                headerStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                headerStyle.EndUpdate();
                sheet.Range["A3:I3"].CellStyle = headerStyle;
                sheet.Range["A6:I6"].CellStyle = headerStyle;
                sheet.Range["A10:I10"].CellStyle = headerStyle;

                #region Generate Excel
                sheet.Range["A2"].ColumnWidth = 30;
                sheet.Range["B2"].ColumnWidth = 30;
                sheet.Range["C2"].ColumnWidth = 30;
                sheet.Range["D2"].ColumnWidth = 30;

                sheet.Range["A2:I2"].Merge(true);



                //IStyle bodyStyle = workbook.Styles.Add("BodyStyle");
                //bodyStyle.BeginUpdate();
                ////bodyStyle.Color = Color.FromArgb(239, 243, 247);
                //bodyStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                //bodyStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                //bodyStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                //bodyStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                //bodyStyle.EndUpdate();

                //sheet.Range["A2:I2"].CellStyle = bodyStyle;


                //Inserting sample text into the first cell of the first sheet
                sheet.Range["A2"].Text = "작업지시서";
                sheet.Range["A2"].CellStyle.Font.FontName = "Verdana";
                sheet.Range["A2"].CellStyle.Font.Bold = true;
                sheet.Range["A2"].CellStyle.Font.Size = 28;
                sheet.Range["A2"].CellStyle.Font.RGBColor = Color.FromArgb(0, 0, 112, 192);
                sheet.Range["A2"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

                sheet.ImportDataTable(ConvertToDataTable2(엑셀_생산지시정보()), true, 3, 1, -1, -1);
                sheet.Range["A3"].CellStyle.Font.Bold = true;

                sheet.Range["A5:I5"].Merge(true);
                sheet.Range["A5"].Text = "품질검사현황";
                sheet.Range["A5"].CellStyle.Font.FontName = "Verdana";
                sheet.Range["A5"].CellStyle.Font.Bold = true;
                sheet.Range["A5"].CellStyle.Font.Size = 28;
                sheet.Range["A5"].CellStyle.Font.RGBColor = Color.FromArgb(0, 0, 112, 192);
                sheet.Range["A5"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

                sheet.ImportDataTable(ConvertToDataTable2(엑셀_작업지시공정현황()), true, 6, 1, -1, -1);
                sheet.Range["A6"].CellStyle.Font.Bold = true;


                sheet.ImportDataTable(ConvertToDataTable2(엑셀_품질검사측정정보()), true, 10, 1, -1, -1);
                sheet.Range["A10"].CellStyle.Font.Bold = true;


                /*
                sheet.Range["A4"].Text = "Employee";
                sheet.Range["B4"].Text = "Roger Federer";
                sheet.Range["A4:B7"].CellStyle.Font.FontName = "Verdana";
                sheet.Range["A4:B7"].CellStyle.Font.Bold = true;
                sheet.Range["A4:B7"].CellStyle.Font.Size = 11;
                sheet.Range["A4:A7"].CellStyle.Font.RGBColor = Color.FromArgb(0, 128, 128, 128);
                sheet.Range["A4:A7"].HorizontalAlignment = ExcelHAlign.HAlignLeft;
                sheet.Range["B4:B7"].CellStyle.Font.RGBColor = Color.FromArgb(0, 174, 170, 170);
                sheet.Range["B4:B7"].HorizontalAlignment = ExcelHAlign.HAlignRight;

                sheet.Range["A9:D20"].CellStyle.Font.FontName = "Verdana";
                sheet.Range["A9:D20"].CellStyle.Font.Size = 11;

                sheet.Range["A5"].Text = "Department";
                sheet.Range["B5"].Text = "Administration";

                sheet.Range["A6"].Text = "Week Ending";
                sheet.Range["B6"].NumberFormat = "m/d/yyyy";
                sheet.Range["B6"].DateTime = DateTime.Parse("10/20/2012", CultureInfo.InvariantCulture);

                sheet.Range["A7"].Text = "Mileage Rate";
                sheet.Range["B7"].NumberFormat = "$#,##0.00";
                sheet.Range["B7"].Number = 0.70;

                sheet.Range["A10"].Text = "Miles Driven";
                sheet.Range["A11"].Text = "Miles Reimbursement";
                sheet.Range["A12"].Text = "Parking and Tolls";
                sheet.Range["A13"].Text = "Auto Rental";
                sheet.Range["A14"].Text = "Lodging";
                sheet.Range["A15"].Text = "Breakfast";
                sheet.Range["A16"].Text = "Lunch";
                sheet.Range["A17"].Text = "Dinner";
                sheet.Range["A18"].Text = "Snacks";
                sheet.Range["A19"].Text = "Others";
                sheet.Range["A20"].Text = "Total";
                sheet.Range["A20:D20"].CellStyle.Color = Color.FromArgb(0, 0, 112, 192);
                sheet.Range["A20:D20"].CellStyle.Font.Color = ExcelKnownColors.White;
                sheet.Range["A20:D20"].CellStyle.Font.Bold = true;

                IStyle style = sheet["B9:D9"].CellStyle;
                style.VerticalAlignment = ExcelVAlign.VAlignCenter;
                style.HorizontalAlignment = ExcelHAlign.HAlignRight;
                style.Color = Color.FromArgb(0, 0, 112, 192);
                style.Font.Bold = true;
                style.Font.Color = ExcelKnownColors.White;

                sheet.Range["A9"].Text = "Expenses";
                sheet.Range["A9"].CellStyle.Color = Color.FromArgb(0, 0, 112, 192);
                sheet.Range["A9"].CellStyle.Font.Color = ExcelKnownColors.White;
                sheet.Range["A9"].CellStyle.Font.Bold = true;
                sheet.Range["B9"].Text = "Day 1";
                sheet.Range["B10"].Number = 100;
                sheet.Range["B11"].NumberFormat = "$#,##0.00";
                sheet.Range["B11"].Formula = "=(B7*B10)";
                sheet.Range["B12"].NumberFormat = "$#,##0.00";
                sheet.Range["B12"].Number = 0;
                sheet.Range["B13"].NumberFormat = "$#,##0.00";
                sheet.Range["B13"].Number = 0;
                sheet.Range["B14"].NumberFormat = "$#,##0.00";
                sheet.Range["B14"].Number = 0;
                sheet.Range["B15"].NumberFormat = "$#,##0.00";
                sheet.Range["B15"].Number = 9;
                sheet.Range["B16"].NumberFormat = "$#,##0.00";
                sheet.Range["B16"].Number = 12;
                sheet.Range["B17"].NumberFormat = "$#,##0.00";
                sheet.Range["B17"].Number = 13;
                sheet.Range["B18"].NumberFormat = "$#,##0.00";
                sheet.Range["B18"].Number = 9.5;
                sheet.Range["B19"].NumberFormat = "$#,##0.00";
                sheet.Range["B19"].Number = 0;
                sheet.Range["B20"].NumberFormat = "$#,##0.00";
                sheet.Range["B20"].Formula = "=SUM(B11:B19)";

                sheet.Range["C9"].Text = "Day 2";
                sheet.Range["C10"].Number = 145;
                sheet.Range["C11"].NumberFormat = "$#,##0.00";
                sheet.Range["C11"].Formula = "=(B7*C10)";
                sheet.Range["C12"].NumberFormat = "$#,##0.00";
                sheet.Range["C12"].Number = 15;
                sheet.Range["C13"].NumberFormat = "$#,##0.00";
                sheet.Range["C13"].Number = 0;
                sheet.Range["C14"].NumberFormat = "$#,##0.00";
                sheet.Range["C14"].Number = 45;
                sheet.Range["C15"].NumberFormat = "$#,##0.00";
                sheet.Range["C15"].Number = 9;
                sheet.Range["C16"].NumberFormat = "$#,##0.00";
                sheet.Range["C16"].Number = 12;
                sheet.Range["C17"].NumberFormat = "$#,##0.00";
                sheet.Range["C17"].Number = 15;
                sheet.Range["C18"].NumberFormat = "$#,##0.00";
                sheet.Range["C18"].Number = 7;
                sheet.Range["C19"].NumberFormat = "$#,##0.00";
                sheet.Range["C19"].Number = 0;
                sheet.Range["C20"].NumberFormat = "$#,##0.00";
                sheet.Range["C20"].Formula = "=SUM(C11:C19)";

                sheet.Range["D9"].Text = "Day 3";
                sheet.Range["D10"].Number = 113;
                sheet.Range["D11"].NumberFormat = "$#,##0.00";
                sheet.Range["D11"].Formula = "=(B7*D10)";
                sheet.Range["D12"].NumberFormat = "$#,##0.00";
                sheet.Range["D12"].Number = 17;
                sheet.Range["D13"].NumberFormat = "$#,##0.00";
                sheet.Range["D13"].Number = 8;
                sheet.Range["D14"].NumberFormat = "$#,##0.00";
                sheet.Range["D14"].Number = 45;
                sheet.Range["D15"].NumberFormat = "$#,##0.00";
                sheet.Range["D15"].Number = 7;
                sheet.Range["D16"].NumberFormat = "$#,##0.00";
                sheet.Range["D16"].Number = 11;
                sheet.Range["D17"].NumberFormat = "$#,##0.00";
                sheet.Range["D17"].Number = 16;
                sheet.Range["D18"].NumberFormat = "$#,##0.00";
                sheet.Range["D18"].Number = 7;
                sheet.Range["D19"].NumberFormat = "$#,##0.00";
                sheet.Range["D19"].Number = 5;
                sheet.Range["D20"].NumberFormat = "$#,##0.00";
                sheet.Range["D20"].Formula = "=SUM(D11:D19)";
                */
                #endregion

                //Save the document as a stream and retrun the stream
                using (MemoryStream stream = new MemoryStream())
                {
                    //Save the created Excel document to MemoryStream
                    workbook.SaveAs(stream);
                    return stream;
                }
            }
        }




    }

    public static class JSInteropExt
    {
        public static async Task SaveAsFileAsync(this IJSRuntime js, string filename, byte[] data, string type = "application/octet-stream;")
        {
            await js.InvokeAsync<object>("JSInteropExt.saveAsFile", filename, type, Convert.ToBase64String(data));
        }
    }

    public class 생산지시정보엑셀
    {

        public int 순번 { get; set; }
        public string 생산지시코드 { get; set; }
        public string 생산지시명 { get; set; }
        public decimal 생산수량 { get; set; }
        public DateTime? 시작일 { get; set; }
        public DateTime? 완료목표일 { get; set; }

    }

    public class 작업지시공정현황엑셀
    {
        public int 공정차수 { get; set; }
        public string 공정단위코드 { get; set; }
        public string 공정명 { get; set; }
        public string 공정품명 { get; set; }
        public decimal 검사수량 { get; set; }
        public decimal 합격수량 { get; set; }
        public decimal 불량수량 { get; set; }
        //public int 합격률 { get; set; }
        public int 불량률 { get; set; }


        //public DateTime? 시작일 { get; set; }
        //public DateTime? 완료목표일 { get; set; }
    }

    public class 품질검사측정정보엑셀
    {
        public int 시리얼넘버 { get; set; }

        public string 생산지시코드 { get; set; }

        public string 품질검사코드 { get; set; }

        public string 생산품공정명 { get; set; }

        public string 생산품공정코드 { get; set; }

        //public string 보유품목코드 { get; set; }
        [Column(TypeName = "decimal(7, 3)")]
        public decimal 검사기준값 { get; set; }
        [Column(TypeName = "decimal(7, 3)")]
        public decimal 오차범위 { get; set; }
        [Column(TypeName = "decimal(7, 3)")]
        public decimal? 검사측정값 { get; set; }

        public string 합격여부 { get; set; }




    }




    //public class CreateService
    //{
    //    /// <summary>
    //    /// Create a simple Excel document
    //    /// </summary>
    //    /// <returns>Return the created excel document as stream</returns>
    //    public MemoryStream CreateXlsIO(string version)
    //    {
    //        //New instance of XlsIO is created.[Equivalent to launching MS Excel with no workbooks open].
    //        //The instantiation process consists of two steps.

    //        //Step 1 : Instantiate the spreadsheet creation engine
    //        using (ExcelEngine excelEngine = new ExcelEngine())
    //        {
    //            //Step 2 : Instantiate the excel application object
    //            IApplication application = excelEngine.Excel;

    //            //Set the default version
    //            if (version == "XLSX")
    //                application.DefaultVersion = ExcelVersion.Excel2016;
    //            else
    //                application.DefaultVersion = ExcelVersion.Excel97to2003;

    //            //Creating new workbook
    //            IWorkbook workbook = application.Workbooks.Create(3);
    //            IWorksheet sheet = workbook.Worksheets[0];


    //            #region Generate Excel
    //            sheet.Range["A2"].ColumnWidth = 30;
    //            sheet.Range["B2"].ColumnWidth = 30;
    //            sheet.Range["C2"].ColumnWidth = 30;
    //            sheet.Range["D2"].ColumnWidth = 30;

    //            sheet.Range["A2:D2"].Merge(true);



    //            //Inserting sample text into the first cell of the first sheet
    //            sheet.Range["A2"].Text = "작업지시서";
    //            sheet.Range["A2"].CellStyle.Font.FontName = "Verdana";
    //            sheet.Range["A2"].CellStyle.Font.Bold = true;
    //            sheet.Range["A2"].CellStyle.Font.Size = 28;
    //            sheet.Range["A2"].CellStyle.Font.RGBColor = Color.FromArgb(0, 0, 112, 192);
    //            sheet.Range["A2"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

    //            sheet.Range["A4"].Text = "Employee";
    //            sheet.Range["B4"].Text = "Roger Federer";
    //            sheet.Range["A4:B7"].CellStyle.Font.FontName = "Verdana";
    //            sheet.Range["A4:B7"].CellStyle.Font.Bold = true;
    //            sheet.Range["A4:B7"].CellStyle.Font.Size = 11;
    //            sheet.Range["A4:A7"].CellStyle.Font.RGBColor = Color.FromArgb(0, 128, 128, 128);
    //            sheet.Range["A4:A7"].HorizontalAlignment = ExcelHAlign.HAlignLeft;
    //            sheet.Range["B4:B7"].CellStyle.Font.RGBColor = Color.FromArgb(0, 174, 170, 170);
    //            sheet.Range["B4:B7"].HorizontalAlignment = ExcelHAlign.HAlignRight;

    //            sheet.Range["A9:D20"].CellStyle.Font.FontName = "Verdana";
    //            sheet.Range["A9:D20"].CellStyle.Font.Size = 11;

    //            sheet.Range["A5"].Text = "Department";
    //            sheet.Range["B5"].Text = "Administration";

    //            sheet.Range["A6"].Text = "Week Ending";
    //            sheet.Range["B6"].NumberFormat = "m/d/yyyy";
    //            sheet.Range["B6"].DateTime = DateTime.Parse("10/20/2012", CultureInfo.InvariantCulture);

    //            sheet.Range["A7"].Text = "Mileage Rate";
    //            sheet.Range["B7"].NumberFormat = "$#,##0.00";
    //            sheet.Range["B7"].Number = 0.70;

    //            sheet.Range["A10"].Text = "Miles Driven";
    //            sheet.Range["A11"].Text = "Miles Reimbursement";
    //            sheet.Range["A12"].Text = "Parking and Tolls";
    //            sheet.Range["A13"].Text = "Auto Rental";
    //            sheet.Range["A14"].Text = "Lodging";
    //            sheet.Range["A15"].Text = "Breakfast";
    //            sheet.Range["A16"].Text = "Lunch";
    //            sheet.Range["A17"].Text = "Dinner";
    //            sheet.Range["A18"].Text = "Snacks";
    //            sheet.Range["A19"].Text = "Others";
    //            sheet.Range["A20"].Text = "Total";
    //            sheet.Range["A20:D20"].CellStyle.Color = Color.FromArgb(0, 0, 112, 192);
    //            sheet.Range["A20:D20"].CellStyle.Font.Color = ExcelKnownColors.White;
    //            sheet.Range["A20:D20"].CellStyle.Font.Bold = true;

    //            IStyle style = sheet["B9:D9"].CellStyle;
    //            style.VerticalAlignment = ExcelVAlign.VAlignCenter;
    //            style.HorizontalAlignment = ExcelHAlign.HAlignRight;
    //            style.Color = Color.FromArgb(0, 0, 112, 192);
    //            style.Font.Bold = true;
    //            style.Font.Color = ExcelKnownColors.White;

    //            sheet.Range["A9"].Text = "Expenses";
    //            sheet.Range["A9"].CellStyle.Color = Color.FromArgb(0, 0, 112, 192);
    //            sheet.Range["A9"].CellStyle.Font.Color = ExcelKnownColors.White;
    //            sheet.Range["A9"].CellStyle.Font.Bold = true;
    //            sheet.Range["B9"].Text = "Day 1";
    //            sheet.Range["B10"].Number = 100;
    //            sheet.Range["B11"].NumberFormat = "$#,##0.00";
    //            sheet.Range["B11"].Formula = "=(B7*B10)";
    //            sheet.Range["B12"].NumberFormat = "$#,##0.00";
    //            sheet.Range["B12"].Number = 0;
    //            sheet.Range["B13"].NumberFormat = "$#,##0.00";
    //            sheet.Range["B13"].Number = 0;
    //            sheet.Range["B14"].NumberFormat = "$#,##0.00";
    //            sheet.Range["B14"].Number = 0;
    //            sheet.Range["B15"].NumberFormat = "$#,##0.00";
    //            sheet.Range["B15"].Number = 9;
    //            sheet.Range["B16"].NumberFormat = "$#,##0.00";
    //            sheet.Range["B16"].Number = 12;
    //            sheet.Range["B17"].NumberFormat = "$#,##0.00";
    //            sheet.Range["B17"].Number = 13;
    //            sheet.Range["B18"].NumberFormat = "$#,##0.00";
    //            sheet.Range["B18"].Number = 9.5;
    //            sheet.Range["B19"].NumberFormat = "$#,##0.00";
    //            sheet.Range["B19"].Number = 0;
    //            sheet.Range["B20"].NumberFormat = "$#,##0.00";
    //            sheet.Range["B20"].Formula = "=SUM(B11:B19)";

    //            sheet.Range["C9"].Text = "Day 2";
    //            sheet.Range["C10"].Number = 145;
    //            sheet.Range["C11"].NumberFormat = "$#,##0.00";
    //            sheet.Range["C11"].Formula = "=(B7*C10)";
    //            sheet.Range["C12"].NumberFormat = "$#,##0.00";
    //            sheet.Range["C12"].Number = 15;
    //            sheet.Range["C13"].NumberFormat = "$#,##0.00";
    //            sheet.Range["C13"].Number = 0;
    //            sheet.Range["C14"].NumberFormat = "$#,##0.00";
    //            sheet.Range["C14"].Number = 45;
    //            sheet.Range["C15"].NumberFormat = "$#,##0.00";
    //            sheet.Range["C15"].Number = 9;
    //            sheet.Range["C16"].NumberFormat = "$#,##0.00";
    //            sheet.Range["C16"].Number = 12;
    //            sheet.Range["C17"].NumberFormat = "$#,##0.00";
    //            sheet.Range["C17"].Number = 15;
    //            sheet.Range["C18"].NumberFormat = "$#,##0.00";
    //            sheet.Range["C18"].Number = 7;
    //            sheet.Range["C19"].NumberFormat = "$#,##0.00";
    //            sheet.Range["C19"].Number = 0;
    //            sheet.Range["C20"].NumberFormat = "$#,##0.00";
    //            sheet.Range["C20"].Formula = "=SUM(C11:C19)";

    //            sheet.Range["D9"].Text = "Day 3";
    //            sheet.Range["D10"].Number = 113;
    //            sheet.Range["D11"].NumberFormat = "$#,##0.00";
    //            sheet.Range["D11"].Formula = "=(B7*D10)";
    //            sheet.Range["D12"].NumberFormat = "$#,##0.00";
    //            sheet.Range["D12"].Number = 17;
    //            sheet.Range["D13"].NumberFormat = "$#,##0.00";
    //            sheet.Range["D13"].Number = 8;
    //            sheet.Range["D14"].NumberFormat = "$#,##0.00";
    //            sheet.Range["D14"].Number = 45;
    //            sheet.Range["D15"].NumberFormat = "$#,##0.00";
    //            sheet.Range["D15"].Number = 7;
    //            sheet.Range["D16"].NumberFormat = "$#,##0.00";
    //            sheet.Range["D16"].Number = 11;
    //            sheet.Range["D17"].NumberFormat = "$#,##0.00";
    //            sheet.Range["D17"].Number = 16;
    //            sheet.Range["D18"].NumberFormat = "$#,##0.00";
    //            sheet.Range["D18"].Number = 7;
    //            sheet.Range["D19"].NumberFormat = "$#,##0.00";
    //            sheet.Range["D19"].Number = 5;
    //            sheet.Range["D20"].NumberFormat = "$#,##0.00";
    //            sheet.Range["D20"].Formula = "=SUM(D11:D19)";
    //            #endregion

    //            //Save the document as a stream and retrun the stream
    //            using (MemoryStream stream = new MemoryStream())
    //            {
    //                //Save the created Excel document to MemoryStream
    //                workbook.SaveAs(stream);
    //                return stream;
    //            }
    //        }
    //    }
    //}
}
