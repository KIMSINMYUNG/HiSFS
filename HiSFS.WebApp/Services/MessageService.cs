using System;

namespace HiSFS.WebApp.Services
{
    public class MessageService
    {
        public event EventHandler<MessageEventArgs> MessageEvent;

        public void Notify(object name, params dynamic[] args)
        {
            MessageEvent?.Invoke(this, new MessageEventArgs(name, args));
        }
    }

    public class MessageEventArgs : EventArgs
    {
        public object Name { get; }
        public dynamic[] Args { get; }

        public MessageEventArgs(object name, params dynamic[] args)
        {
            this.Name = name;
            this.Args = args;
        }
    }

    public enum Message
    {
        SelectedMenu,

        ShowLoginPage,
        Login,
        Logout,

        //////////
        ChangedCommonCode,

        //////////
        ModifiedAddData,
        ModifiedUpdateData,
        ModifiedDeleteData,

        //////////
        ReceivedActionTag,

        //////////
        ChangedMachineState,

        //////////
        SendMessage,
        ReceivedMessage,

        //////////
        ShowMessage,

        ///////////
        ReceivedTest,

        //////////
        NotConnected,
        PrintOutOk,

        //////////
        SelectAlert,
        SelectItemAlert,

        //2021.02.04
        ConnectionAlert,
        DisconnectAlert,

        CheckDataSave,
        CheckDataReset,

        //2021.02.17
        ChangeCheckState,
        CheckDataSaveErorr,

        //2021.02.18
        CheckDataAddReq,

        //2021.02.19
        CheckStart,
        CheckEnd,

        StartScan,
        EndScan,


        //추가 2021.03.17
        CheckProcStart,
        CheckProcEnd,

        StartProcScan,
        EndProcScan,

        InputErrMesage,
        DeleteErrMesage,
        FileNotFoundMesage,
        FileSaveMessage,
        ItemSelectMessage,

        PlaceSelectMessage,
        BOM품목코드선택Message,

        반영성공,
        반영실패,

        재고부족메세지,
        재고이동메세지,

        공정추가오류,

        작업지시작업자지정메세지,

        품질검사종료메세지,

        품질검사시작해주세요,

        장비를연결해주세요,

    }
}