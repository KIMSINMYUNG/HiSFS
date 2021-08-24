using HiSFS.Api.Shared.Models;
using HiSFS.WebApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.ProtectedBrowserStorage;
using Syncfusion.Blazor.FileManager;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.TreeGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using TreeGridEditMode = Syncfusion.Blazor.TreeGrid.EditMode;
using GridEditMode = Syncfusion.Blazor.Grids.EditMode;
using System.Linq;
using System.Reactive.Subjects;
using HiSFS.Agent.Service.Devices;
using HiSFS.Api.Host.Services;

namespace HiSFS.WebApp.Component
{
    public abstract partial class CustomComponent : ComponentBase, IDisposable
    {
        [Inject]
        private MessageService Message { get; set; }

        [Inject]
        private GlobalMessageService GlobalMessage { get; set; }

        [Inject]
        protected RemoteService Remote { get; set; }

        [Inject]
        protected CacheService Cache { get; set; }

        [Inject]
        protected Context Context { get; set; }

        [Inject]
        protected ProtectedSessionStorage SessionStorage { get; set; }

        [Inject]
        protected ProtectedLocalStorage LocalStorage { get; set; }

        protected CommonCodeDictionary<공통코드> 코드 => Cache.코드;
        protected CommonCodeDictionary<IList<공통코드>> 코드목록 => Cache.코드목록;
        protected CommonCodeDictionary<IList<공통코드>> 코드형제목록 => Cache.코드형제목록;

        [Parameter]
        public object Params { get; set; }

        [Parameter]
        public decimal Params2 { get; set; }

        [Parameter]
        public string MenuId { get; set; }


        private 메뉴직원권한정보 메뉴권한정보;

        public String connectCtype = "C";
        public String connectLtype = "L";

        public void Dispose()
        {
            Message.MessageEvent -= MessageService_MessageEvent;
            GlobalMessage.MessageEvent -= GlobalMessageService_MessageEvent;
            //Remote.ReadyEvent -= RemoteService_ReadyEvent;

            OnDispose();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Message.MessageEvent += MessageService_MessageEvent;
            GlobalMessage.MessageEvent += GlobalMessageService_MessageEvent;

            //Remote.ReadyEvent += RemoteService_ReadyEvent;

            // 이미 준비가 된 상태에는 원격 API 준비됨 이벤트 강제 발생
            //if (Remote.IsReady == true)
            //    RemoteService_ReadyEvent(this, RemoteServiceReadyEventArgs.ReadyEventArgs);

            // 컴포넌트 생성 시 해당 권한이 있는지를 확인한다.

            var 권한목록 = Context.직원?.메뉴직원권한목록;
            if (권한목록 != null)
                메뉴권한정보 = 권한목록.FirstOrDefault(x => x.메뉴순번.ToString() == MenuId);
        }

        //private async void RemoteService_ReadyEvent(object sender, RemoteServiceReadyEventArgs e)
        //{
        //    await InvokeAsync(() => OnReadyRemoteServiceAsync(e.IsReady));
        //}

        //protected virtual Task OnReadyRemoteServiceAsync(bool isReady)
        //{
        //    return Task.CompletedTask;
        //}

        private async void MessageService_MessageEvent(object sender, MessageEventArgs e)
        {
            await InvokeAsync(() =>
            {
                if (e.Name is Message m)
                    OnReceivedMessage(m, false, e.Args);
                else
                    OnReceivedMessage(e.Name.ToString(), false, e.Args);
            });
        }

        private async void GlobalMessageService_MessageEvent(object sender, MessageEventArgs e)
        {
            await InvokeAsync(() =>
            {
                if (e.Name is Message m)
                    OnReceivedMessage(m, true, e.Args);
                else
                    OnReceivedMessage(e.Name.ToString(), true, e.Args);
            });
        }

        public void NotifyMessage(string name, params dynamic[] args)
        {
            Message.Notify(name, args);
        }

        public void NotifyMessage(Message message, params dynamic[] args)
        {
            Message.Notify(message, args);
        }


        public void NotifyGlobalMessage(string name, params dynamic[] args)
        {
            GlobalMessage.Notify(name, args);
        }

        public void NotifyGlobalMessage(Message message, params dynamic[] args)
        {
            GlobalMessage.Notify(message, args);
        }

        protected abstract void OnDispose();

        protected virtual void OnReceivedMessage(string name, bool isGlobal, dynamic[] args)
        {
        }

        protected virtual void OnReceivedMessage(Message message, bool isGlobal, dynamic[] args)
        {
        }

        public async Task<bool> ShowMessageBox(string title, string contents, MessageBoxResultType resultType)
        {
            var messageBoxInfo = (Context as IMessageBoxContainer).MessageBox;

            messageBoxInfo.Result = false;
            messageBoxInfo.Title = title;
            messageBoxInfo.Contents = contents;
            messageBoxInfo.IsVisible = true;
            messageBoxInfo.ResultType = resultType;

            NotifyMessage(Services.Message.ShowMessage);

            await Task.Run(() => messageBoxInfo.VisibleManualResetEvent.WaitOne());

            return messageBoxInfo.Result;
        }

        public async Task<bool> ShowPromptBox(string title, string contents, string count, MessageBoxResultType resultType)
        {
            var promptBoxInfo = (Context as IPromptBoxContainer).PromptBox;

            promptBoxInfo.Result = false;
            promptBoxInfo.Title = title;
            promptBoxInfo.Contents = contents;
            promptBoxInfo.Count = count;
            promptBoxInfo.IsVisible = true;
            promptBoxInfo.ResultType = resultType;

            NotifyMessage(Services.Message.ShowMessage);

            await Task.Run(() => promptBoxInfo.VisibleManualResetEvent.WaitOne());

            return promptBoxInfo.Result;
        }

        public class BindableProperty<T> : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private object instance;
            private PropertyInfo property;
            private T v;

            public BindableProperty(object instance, PropertyInfo property)
            {
                this.instance = instance;
                this.property = property;
            }

            public BindableProperty(object instance, string propertyName)
            {
                this.instance = instance;
                this.property = instance.GetType().GetProperty(propertyName);
            }

            public BindableProperty(T value)
            {
                this.v = value;
            }

            public T Value
            {
                get => instance == null ? v : (T)property.GetValue(instance);
                set
                {
                    if (instance == null)
                    {
                        v = value;
                    }
                    else
                    {
                        var oldValue = Value;
                        if (oldValue?.Equals(value) == true)
                            return;

                        property.SetValue(instance, value);
                    }

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
        }

        public void ModifyList<T>(IEnumerable<T> list, Action<T, IReadOnlyDictionary<string, 공통코드>> func, bool isInversNo = true)
            where T : 공통정보
        {
            if (list == null)
                return;

            var num = isInversNo == true ? list.Count() : 1;
            foreach (var row in list)
            {
                func(row, 코드);
                
                row.No = num;

                if (isInversNo == true)
                    num--;
                else
                    num++;
            }
        }

        protected bool IsAuthRegist => !(메뉴권한정보 == null || 메뉴권한정보.등록권한 != true);
        protected bool IsAuthModify => !(메뉴권한정보 == null || 메뉴권한정보.변경권한 != true);
        protected bool IsAuthDelete => !(메뉴권한정보 == null || 메뉴권한정보.삭제권한 != true);

        /// <summary>
        /// 권한 체크 후 등록
        /// </summary>
        /// <param name="action"></param>
        protected async Task<bool> CheckAndRegist(Func<Task> action)
        {
            if (IsAuthRegist == false)
            {
                await ShowMessageBox("권한없음", "등록 권한이 없습니다.", MessageBoxResultType.Okay);
                return false;
            }

            if (action != null)
                await action();
            return true;
        }

        /// <summary>
        /// 권한 체크 후 변경
        /// </summary>
        /// <param name="action"></param>
        protected async Task<bool> CheckAndModify(Func<Task> action)
        {
            if (IsAuthModify == false)
            {
                await ShowMessageBox("권한없음", "변경 권한이 없습니다.", MessageBoxResultType.Okay);
                return false;
            }

            if (action != null)
                await action();
            return true;
        }

        /// <summary>
        /// 권한 체크 후 삭제
        /// </summary>
        /// <param name="action"></param>
        protected async Task<bool> CheckAndDelete(Func<Task> action)
        {
            if (IsAuthDelete == false)
            {
                await ShowMessageBox("권한없음", "삭제 권한이 없습니다.", MessageBoxResultType.Okay);
                return false;
            }

            if (action != null)
                await action();
            return true;
        }

        protected async Task CheckAuth(CheckAuthEventArgs e)
        {
            var action = e.Action;

            var bResult = true;
            if (action == "Add")
                bResult = await CheckAndRegist(null);
            else if (action == "BeginEdit" || action == "Save")
                bResult = await CheckAndModify(null);
            else if (action == "Delete")
                bResult = await CheckAndDelete(null);

            e.Cancel = bResult == false;
        }

        // 2021.01.23 추가
        protected async Task QRcodePrinte(string _uri, string _type, string num, string barcodeValue, string barcodeText, int size)
        {
             var zebraPrinter = new ZebraPrinter();
            if (_type.Equals("C")) { 
               zebraPrinter.Open();
            } else
            {
                zebraPrinter.LANOpen(_uri);
            }
            if (zebraPrinter.IsConnected)
            {
                //zebraPrinter.Write(zebraPrinter.GetQRCodeLabel(item.보유품목코드, item.보유명));
                //zebraPrinter.Write(zebraPrinter.GetQRCodeLabel(item.보유품목코드, "CNC"));
                var prn_num = Convert.ToInt32(num);
                for (int lcv = 0; lcv < prn_num; lcv++)
                {
                    zebraPrinter.Write(zebraPrinter.GetQRCodeLabel(barcodeValue, barcodeText, size));
                }
                
                //break;
            }
            else
            {
                NotifyMessage(Services.Message.NotConnected);
                await ShowMessageBox("연결안됨", "연결이 안되었습니다.", MessageBoxResultType.Okay);
            }


            zebraPrinter.Close(); 
        }

        public async Task QRPrinte_Act(string barcodeValue, int size)
        {
            var zebraUri = await Remote.Command.공통.PrtUri();

            //await ShowMessageBox("수량", "숫자를 입력 하세요", MessageBoxResultType.Okay);
            var name = await ShowPromptBox("수량", "숫자를 입력 하세요", "1", MessageBoxResultType.YesOrNo);
            /*var name = await JS.InvokeAsync<string>(
            "exampleJsFunctions.showPrompt",
            "수량을 입력하세요"); */

         

            if (name)
            {
                var qrCount = this.Context.promptBoxInfo.Count;
                try
                {
                    var prn_su = Convert.ToInt32(qrCount);
                }
                catch (Exception ex)
                {
                    await ShowMessageBox(qrCount, "숫자를 입력 하세요", MessageBoxResultType.Okay);
                    Console.WriteLine("텍스트 문자 입력 : " + ex.Message);
                    return;
                }
                //CustomComponent
                await QRcodePrinte(zebraUri, connectLtype, qrCount, barcodeValue, barcodeValue, size); // connectCtype, connectLtype
                NotifyMessage(Services.Message.PrintOutOk);
            }
        }



        public async Task QRPrinteSerial_Act(string barcodeValue, int size , string cnt)
        {
            var zebraUri = await Remote.Command.공통.PrtUri();

            await QRcodePrinte(zebraUri, connectLtype, cnt, barcodeValue, barcodeValue, size); // connectCtype, connectLtype
            NotifyMessage(Services.Message.PrintOutOk);
        }

        //2021.05.10
        public async Task QRPrinte_바코드Act(string barcodeValue, string 수량, int size)
        {
            var zebraUri = await Remote.Command.공통.PrtUri();

            //CustomComponent
            await QRcodePrinte(zebraUri, connectLtype, 수량, barcodeValue, barcodeValue, size); // connectCtype, connectLtype
            NotifyMessage(Services.Message.PrintOutOk);
            
        }


    }

    //public static class SfExtension
    //{
    //    private static readonly List<object> editModeToolbars = new List<object>
    //    {
    //        "Add", "Edit", "Delete", "Cancel", "Update", "Search", "ExcelExport",

    //        new Syncfusion.Blazor.Navigations.ItemModel { Text="새로고침", TooltipText = "새로고침", PrefixIcon = "e-reload", Align = ItemAlign.Left, Id = "Reload" }
    //    };

    //    private static readonly List<object> readModeToolbars = new List<object>
    //    {
    //        "Search", "ExcelExport",

    //        new Syncfusion.Blazor.Navigations.ItemModel { Text="새로고침", TooltipText = "새로고침", PrefixIcon = "e-reload", Align = ItemAlign.Left, Id = "Reload" }
    //    };

    //    private static readonly List<object> popupModeToolbars = new List<object>
    //    {
    //        "Search"
    //    };

    //    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "BL0005:Component parameter should not be set outside of its component.", Justification = "<보류 중>")]
    //    public static void SetEditMode<T>(this SfGrid<T> @this, bool allowEditing = true)
    //    {
    //        //<GridEditSettings AllowAdding="true" AllowDeleting="true" AllowEditing="true" Mode="EditMode.Normal" NewRowPosition="NewRowPosition.Bottom" ShowDeleteConfirmDialog="true"/>
    //        @this.EditSettings.AllowAdding = true;
    //        @this.EditSettings.AllowDeleting = true;
    //        @this.EditSettings.AllowEditing = allowEditing;
    //        //@this.EditSettings.Mode = GridEditMode.Normal;
    //        @this.EditSettings.Mode = GridEditMode.Dialog;
    //        @this.EditSettings.NewRowPosition = NewRowPosition.Bottom;
    //        @this.EditSettings.ShowDeleteConfirmDialog = true;
            
    //        @this.Toolbar = editModeToolbars;
    //        //ContextMenuItems="@(new List<object> { "Delete" })"
    //        @this.ContextMenuItems = new List<object> { "Delete" };
    //    }

    //    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "BL0005:Component parameter should not be set outside of its component.", Justification = "<보류 중>")]
    //    public static void SetDefault<T>(this SfGrid<T> @this, PageMode pageMode, params string[] searchFields)
    //    {
    //        //AllowPaging="true"
    //        @this.AllowPaging = true;
    //        //<GridPageSettings PageCount="10" PageSize="19" />
    //        @this.PageSettings.PageCount = 10;
    //        if (pageMode == PageMode.Default)
    //        {
    //            @this.PageSettings.PageSize = 19;

    //            @this.AllowExcelExport = true;

    //            @this.Toolbar = readModeToolbars;
    //        }
    //        else
    //        {
    //            @this.PageSettings.PageSize = 10;

    //            @this.Toolbar = popupModeToolbars;
    //        }
    //        //<GridSelectionSettings Type="SelectionType.Single" EnableToggle="false" />
    //        @this.SelectionSettings.Type = SelectionType.Single;
    //        @this.SelectionSettings.EnableToggle = false;

    //        if (searchFields != null && searchFields.Length > 0)
    //        {
    //            @this.SearchSettings.Fields = searchFields;
    //            @this.SearchSettings.Operator = Syncfusion.Blazor.Operator.Contains;
    //            @this.SearchSettings.IgnoreCase = true;
    //        }
    //    }

    //    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "BL0005:Component parameter should not be set outside of its component.", Justification = "<보류 중>")]
    //    public static void SetEditMode<T>(this SfTreeGrid<T> @this)
    //    {
    //        //<GridEditSettings AllowAdding="true" AllowDeleting="true" AllowEditing="true" Mode="EditMode.Normal" NewRowPosition="NewRowPosition.Bottom" ShowDeleteConfirmDialog="true"/>
    //        @this.EditSettings.AllowAdding = true;
    //        @this.EditSettings.AllowDeleting = true;
    //        @this.EditSettings.AllowEditing = true;
    //        @this.EditSettings.Mode = TreeGridEditMode.Row;
    //        @this.EditSettings.NewRowPosition = RowPosition.Below;
    //        @this.EditSettings.ShowDeleteConfirmDialog = true;

    //        @this.Toolbar = editModeToolbars;
    //        //ContextMenuItems="@(new List<object> { "Delete" })"
    //        @this.ContextMenuItems = new List<object> { "Delete" };
    //    }

    //    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "BL0005:Component parameter should not be set outside of its component.", Justification = "<보류 중>")]
    //    public static void SetDefault<T>(this SfTreeGrid<T> @this, params string[] searchFields)
    //    {
    //        //AllowPaging="true"
    //        @this.AllowPaging = true;
    //        //<GridPageSettings PageCount="10" PageSize="19" />
    //        @this.PageSettings.PageCount = 10;
    //        @this.PageSettings.PageSize = 19;
    //        //<GridSelectionSettings Type="SelectionType.Single" EnableToggle="false" />
    //        @this.SelectionSettings.Type = SelectionType.Single;
    //        @this.SelectionSettings.EnableToggle = false;

    //        @this.AllowExcelExport = true;

    //        @this.Toolbar = readModeToolbars;

    //        if (searchFields != null && searchFields.Length > 0)
    //        {
    //            @this.SearchSettings.Fields = searchFields;
    //            @this.SearchSettings.Operator = Syncfusion.Blazor.Operator.Contains;
    //            @this.SearchSettings.IgnoreCase = true;
    //        }
    //    }
    //}

    public enum PageMode
    {
        Default,
        Popup,
        Inline,
        Small
    }

    public class SelectedEventArgs<TValue> : System.EventArgs
    {
        public TValue Selected { get; }
        public bool IsDoubleClick { get; }

        public SelectedEventArgs(TValue selected, bool isDoubleClick = false)
        {
            this.Selected = selected;
            this.IsDoubleClick = IsDoubleClick;
        }
    }

    public class CheckAuthEventArgs : System.EventArgs
    {
        public string Action { get; private set; }
        public bool Cancel { get; set; }

        public CheckAuthEventArgs(string action)
        {
            this.Action = action;
        }
    }

}