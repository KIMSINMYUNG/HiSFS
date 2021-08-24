using HiSFS.Api.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HiSFS.WebApp.Services
{
    public class Context : IMessageBoxContainer, IPromptBoxContainer
    {
        private MessageBoxInfo messageBoxInfo = new MessageBoxInfo();
        public PromptBoxInfo promptBoxInfo = new PromptBoxInfo();

        public Type PageType { get; set; }

        // ------

        public 직원정보 직원 { get; set; }
        public bool 로그인유무 => 직원 != default;

        MessageBoxInfo IMessageBoxContainer.MessageBox => messageBoxInfo;
        PromptBoxInfo IPromptBoxContainer.PromptBox => promptBoxInfo;
    }

    public interface IMessageBoxContainer
    {
        MessageBoxInfo MessageBox { get; }
    }

    public class MessageBoxInfo
    {
        private bool isVisible;
        private ManualResetEvent mre = new ManualResetEvent(false);

        public ManualResetEvent VisibleManualResetEvent => mre;

        public bool IsVisible
        {
            get => isVisible;
            set
            {
                if (isVisible == value)
                    return;

                if (value == true)
                    mre.Reset();
                else
                    mre.Set();

                isVisible = value;
            }
        }

        public string Title { get; set; }

        public string Contents { get; set; }

        public bool Result { get; set; }

        public MessageBoxResultType ResultType { get; set; }
    }

    public interface IPromptBoxContainer
    {
        PromptBoxInfo PromptBox { get; }
    }

    public class PromptBoxInfo
    {
        private bool isVisible;
        private ManualResetEvent pmre = new ManualResetEvent(false);

        public ManualResetEvent VisibleManualResetEvent => pmre;

        public bool IsVisible
        {
            get => isVisible;
            set
            {
                if (isVisible == value)
                    return;

                if (value == true)
                    pmre.Reset();
                else
                    pmre.Set();

                isVisible = value;
            }
        }

        public string Title { get; set; }

        public string Contents { get; set; }

        public string Count { get; set; }

        public bool Result { get; set; }

        public MessageBoxResultType ResultType { get; set; }
    }

    public enum MessageBoxResultType
    {
        Okay,
        YesOrNo
    }
}
