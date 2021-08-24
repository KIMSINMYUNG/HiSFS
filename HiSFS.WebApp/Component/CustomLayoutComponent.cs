using HiSFS.Api.Shared.Models;
using HiSFS.WebApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.ProtectedBrowserStorage;

using Syncfusion.XlsIO;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HiSFS.WebApp.Component
{
    public abstract class CustomLayoutComponent : LayoutComponentBase, IDisposable
    {
        [Inject]
        private MessageService MessageService { get; set; }

        [Inject]
        private GlobalMessageService GlobalMessageService { get; set; }

        [Inject]
        protected RemoteService Remote { get; set; }

        [Inject]
        protected CacheService Cache { get; set; }

        [Inject]
        protected ProtectedSessionStorage SessionStorage { get; set; }

        [Inject]
        protected ProtectedLocalStorage LocalStorage { get; set; }

        [Inject]
        protected Context Context { get; set; }

        public CommonCodeDictionary<공통코드> 코드 => Cache.코드;
        public CommonCodeDictionary<IList<공통코드>> 코드하위목록 => Cache.코드목록;
        public CommonCodeDictionary<IList<공통코드>> 코드형제목록 => Cache.코드형제목록;


        private IDisposable globalMessageSubscribe;
        private IDisposable globalMessageSubscribe2;

        private IDisposable globalMessageSubscribe3;


        public void Dispose()
        {
            MessageService.MessageEvent -= MessageService_MessageEvent;
            GlobalMessageService.MessageEvent -= GlobalMessageService_MessageEvent;
            Remote.ReadyEvent -= RemoteService_ReadyEvent;

            globalMessageSubscribe.Dispose();
            globalMessageSubscribe2.Dispose();

            globalMessageSubscribe3.Dispose();

            OnDispose();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            MessageService.MessageEvent += MessageService_MessageEvent;
            GlobalMessageService.MessageEvent += GlobalMessageService_MessageEvent;
            Remote.ReadyEvent += RemoteService_ReadyEvent;

            // 이미 준비가 된 상태에는 원격 API 준비됨 이벤트 강제 발생
            if (Remote.IsReady == true)
                RemoteService_ReadyEvent(this, RemoteServiceReadyEventArgs.ReadyEventArgs);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await Remote.WaitForReadyRemoteService();

            globalMessageSubscribe = Remote.GetSubject<string>("global.message").Subscribe(async strMessage =>
            {
                var bResult = Enum.TryParse<Message>(strMessage, out var message);
                if (bResult == false)
                    return;

                await InvokeAsync(() =>
                {
                    NotifyMessage(message);
                });
            }, e => { });

            globalMessageSubscribe2 = Remote.GetSubject<(string, string)>("global.message.param1").Subscribe(async e =>
            {
                var bResult = Enum.TryParse<Message>(e.Item1, out var message);
                if (bResult == false)
                    return;

                await InvokeAsync(() =>
                {
                    NotifyMessage(message, e.Item2);
                });
            }, e => { });


            globalMessageSubscribe3 = Remote.GetSubject<(string, string)>("global.message.qr").Subscribe(async e =>
            {
                var bResult = Enum.TryParse<Message>(e.Item1, out var message);
                if (bResult == false)
                    return;

                await InvokeAsync(() =>
                {
                    NotifyMessage(message, e.Item2);
                });
            }, e => { });
        }

        private async void RemoteService_ReadyEvent(object sender, RemoteServiceReadyEventArgs e)
        {
            await InvokeAsync(() => OnReadyRemoteServiceAsync(e.IsReady));
        }

        protected virtual Task OnReadyRemoteServiceAsync(bool isReady)
        {
            return Task.CompletedTask;
        }

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
            MessageService.Notify(name, args);
        }

        public void NotifyMessage(Message message, params dynamic[] args)
        {
            MessageService.Notify(message, args);
        }

        public void NotifyGlobalMessage(string name, params dynamic[] args)
        {
            GlobalMessageService.Notify(name, args);
        }

        public void NotifyGlobalMessage(Message message, params dynamic[] args)
        {
            GlobalMessageService.Notify(message, args);
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

            await Task.Run(() => promptBoxInfo.VisibleManualResetEvent.WaitOne());

            return promptBoxInfo.Result;
        }
    }
}