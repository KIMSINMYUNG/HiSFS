using HiSFS.Api.Shared.Models;
using HiSFS.WebApp.Component.Common;
using HiSFS.WebApp.Services;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using WampSharp.V2;


namespace HiSFS.WebApp.Pages.QV.IV
{
    public partial class 발주서별수입검사상세Page
    {
        private FGrid<발주서별품질검사장비> grid5;
        private ObservableCollection<발주서별품질검사장비> list5 = new ObservableCollection<발주서별품질검사장비>();

        private HGrid<발주서별품질검사정보> grid4;
        private ObservableCollection<발주서별품질검사정보> list4 = new ObservableCollection<발주서별품질검사정보>();
        private 발주서별품질검사정보 selected발주서별품질검사정보;


        private Dictionary<int?, string> RowButton = new Dictionary<int?, string>();
        Dictionary<int, decimal> myValue = new Dictionary<int, decimal>();
        private int checkCnt = 0;
        public int Sid = 0;
        private bool 검사장비연결 = false;

        // 2021.06.26
        private bool 품검종료 = false;
        private int 현재연결장비번호 = 0;
        private IDisposable pum_subscription = null;

        #region  품질검사
        //품질검사 시작
        private async Task onClickStart()
        {

            var 검사항목수 = await grid4.Grid.GetCurrentViewRecords();
            if (btnStartFlag == false)
            {
                btnStartFlag = true;
                IsBtnStartEnable = true;
                IsBtnEndEnable = false;
                var userId = await SessionStorage.GetAsync<string>("userId");
                //await Remote.Command.공통.포인트_저장(userId, 생산지시.생산계획.생산품공정코드, Convert.ToInt32(생산지시.생산수량), 검사항목수.Count);
                await Remote.Command.공통.수입검사포인트_저장2(userId, 발주서수입검사.품번, Convert.ToInt32(발주서수입검사.발주수량), 검사항목수.Count);

                //await Remote.Command.품질관리.품질검사시작_보유품목코드_저장(생산품코드, 생산지시.생산계획.생산품.품목구분코드, Convert.ToInt32(생산지시.생산수량), 회사코드);

                /* 보유품목 다시 정의  20210513
                await Remote.Command.품질관리.보유품목일지_저장(생산지시.생산계획.생산품코드, Convert.ToInt32(생산지시.생산수량), 생산지시.생산지시코드);
                await Remote.Command.품질관리.품질검사측정_보유품목일련번호생성_저장(생산지시.생산지시코드, 생산지시.생산계획.생산품코드, Convert.ToInt32(생산지시.생산수량));
                */

                NotifyMessage(Message.CheckStart);
            }

            // 2021.06.26
            품검종료 = false;

        }
        //품질검사 종료
        private async Task onClickEnd()
        {
            if (btnStartFlag == true)
            {
                btnStartFlag = false;

                IsBtnStartEnable = false;
                IsBtnEndEnable = true;

                var userId = await SessionStorage.GetAsync<string>("userId");
                bool nowPoint = await Remote.Command.공통.외주포인트_삭제(userId, 발주서수입검사.품번);

                NotifyMessage(Message.CheckEnd);
            }

            // 2021.06.26
            품검종료 = true;
            RowButton[현재연결장비번호] = "연결";
            Sid = 0;
            //isConn = false;
            isConns = false;
            Console.WriteLine("Closing ...");
            if (pum_subscription != null)
            {
                pum_subscription.Dispose();
                pum_subscription = null;
            }
                
        }
        #endregion


        public static bool isConns = false;
        private async Task OnConnection(발주서별품질검사장비 info, bool isConn, int BtnSid)
        {
            if (BtnSid == 0 || RowButton[info.검사장비식별번호] == "연결")
            {
                await grid5.Grid.SelectRow(info.No, true);
                RowButton[info.검사장비식별번호] = "해제";
                Sid = 1;
                isConn = true;
                isConns = true;
                현재연결장비번호 = Convert.ToInt32(info.검사장비식별번호);
            }
            else if (BtnSid == 1 || RowButton[info.검사장비식별번호] == "해제")
            {
                await grid5.Grid.ClearCellSelection();
                RowButton[info.검사장비식별번호] = "연결";
                Sid = 0;
                isConn = false;
                isConns = false;
                return;
            }

            //btnSave.Disabled = true;
            검사장비연결 = isConns;
            NotifyMessage(isConns == true ? Message.ConnectionAlert : Message.DisconnectAlert);
            //var bResult = await CheckAndModify(() => Remote.Command.생산관리.공정단위검사장비_저장(info, false, isConn));
            //if (bResult == false)
            //    return;

            if (!isConns)
            {
                return;
            }
            var server_Uri = await Remote.Command.공통.Server_Uri();

            DefaultWampChannelFactory factory =
                new DefaultWampChannelFactory();
            //const string serverAddress = "ws://127.0.0.1:31200/ws";

            IWampChannel channel =
                    factory.CreateJsonChannel(server_Uri, "HiSFS.Api"); //"HongAppsRealm");
            //await channel.Close(info.검사장비.식별코드, null);
            await channel.Open().ConfigureAwait(false);

            ISubject<(string, decimal)> MitutoyoSubject = channel.RealmProxy.Services.GetSubject(info.검사장비.식별코드, new MyMitutoyoTupleEventConverter());

            Notifier notifier = new Notifier();
            notifier.SomethingHappened += MyHandler;  // 발생시킬 이벤트를 등록한다.

            // 2021.06.21
            //IDisposable pum_subscription = null;

            pum_subscription = MitutoyoSubject.Subscribe(value =>
            {
                var 검사함항목체크 = grid4.Grid.GetSelectedRecords();

                var 검사함항목 = grid4.Grid.SelectedRowIndexes;
                Console.WriteLine(검사함항목.Count);
                int i = 0;
                if (검사함항목체크.Result.Count == 0 || 검사함항목체크.Result.Count == checkCnt)
                {
                    if (i == 0)
                        NotifyMessage(Message.SelectAlert);
                    i++;
                    return;
                }
                if (!btnStartFlag)
                {
                    NotifyMessage(Message.품질검사시작해주세요);
                    return;
                }
                if (!isConns)
                {
                    NotifyMessage(Message.장비를연결해주세요);
                    return;
                }
                //if (검사함항목.Count == 0 || 검사함항목.Count == checkCnt)
                //            {
                //                NotifyMessage(Message.SelectAlert);
                //                return;
                //            }

                (string id, decimal message_value) = value;
                Console.WriteLine($">com.myapp.topicqr : Got event : id : {id}, b : {message_value}");

                myValue.Add(checkCnt, message_value);
                notifier.DoSomething(myValue);
                checkCnt++;

                // 2021.06.26
                //if (품검종료)
                //{
                //    Console.WriteLine("Closing ...");
                //    pum_subscription.Dispose();
                //}
            });

            async void MyHandler(Dictionary<int, decimal> dic)
            {
                var GetSelectedRowIndexes = await grid4.Grid.GetSelectedRecords();

                if (GetSelectedRowIndexes.Count >= dic.Count)
                {
                    foreach (KeyValuePair<int, decimal> pair in dic)
                    {
                        var found4 = list4.FirstOrDefault(x => x.No == GetSelectedRowIndexes[Convert.ToInt32(pair.Key)].No);
                        found4.검사측정값 = Convert.ToDecimal(pair.Value);
                        found4.합격여부 = ConfirmsData(found4);

                        //list4[Convert.ToInt32(pair.Key)].검사측정값 = Convert.ToDecimal(pair.Value);
                        //list4[Convert.ToInt32(pair.Key)].합격여부 = ConfirmsData(list4[Convert.ToInt32(pair.Key)]);
                    }

                }
                //if (GetSelectedRowIndexes.Result.Count <= dic.Count)
                //    btnSave.Disabled = false;

                await InvokeAsync(() =>
                {
                    StateHasChanged();
                });
            }
        }

        public async Task ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            Console.WriteLine(검사수량입력);
            if (args.Item.Text == "검사수량설정")
            {
                if (Convert.ToInt32(검사수량입력) > 발주서수입검사.발주수량)
                    return;

                검사수량입력_숫자 = Convert.ToInt32(검사수량입력);

                var cancle = await ShowMessageBox("검사수량", "검사수량을 설정하시겠습니까?", MessageBoxResultType.YesOrNo);
                if (!cancle)
                    return;

                IsEnable = false;
                IsBtnEnable = true;

                Console.WriteLine(args.Item.Text);
                list3 = Enumerable.Range(1, Convert.ToInt32(검사수량입력_숫자)).Select(cnt => new 품질검사생산품()
                {
                    Seq = cnt,
                    생산지시코드 = 발주서수입검사.발주번호,
                    발주순번 = 발주서수입검사.발주순번,
                    생산품공정코드 = 발주서수입검사.품번,
                    생산품공정명 = null,
                    공정단위코드 = null,
                    Result = "",
                    보유품목코드 = 발주서수입검사.품번,
                    CheckDate = DateTime.Now.AddDays(-cnt),
                }).ToList();
                품질검사생산품목록 = new ObservableCollection<품질검사생산품>(list3);

            }
        }


        public static int nowItem { get; set; }
        delegate void EventHandler(Dictionary<int, decimal> dic);
        private async Task OnCheckSave()
        {
            var 검사체크항목 = await grid4.Grid.GetSelectedRecords();
            var 검사항목전체 = await grid4.Grid.GetCurrentViewRecords();
            //if (검사체크항목.Count == 0)
            //{
            //    NotifyMessage(Message.SelectAlert);
            //    return;
            //}
            var userId = await SessionStorage.GetAsync<string>("userId");

            if (!btnStartFlag)
            {
                ShowMessageBox(userId, "품질검사를 시작해주세요.", MessageBoxResultType.Okay);
                return;
            }
            Console.WriteLine(btnStartFlag);

            var save = await ShowMessageBox("측정저장", "측정을 저장하시겠습니까?", MessageBoxResultType.YesOrNo);
            if (!save)
                return;
            if (save == true && 검사체크항목.Count == 0)
            {
                NotifyMessage(Message.SelectAlert);
                return;
            }

            List<품질검사생산품> 품목 = await grid3.GetCurrentViewRecords();
            foreach (var item in 검사체크항목)
            {
                if (item.검사측정값 == null)
                {
                    ShowMessageBox(userId, "측정데이터가  업습니다.", MessageBoxResultType.Okay);
                    return;
                }
            }
            try
            {
                //await Remote.Command.공통.포인트_저장("20062901", "KMFA-518986:1:1", 생산지시.생산지시코드
                int nowPoint = await Remote.Command.공통.수입검사포인트_조회(userId, 발주서수입검사.품번, 발주서수입검사.발주번호, 발주서수입검사.발주순번, 발주서수입검사.회사코드);

                Console.WriteLine("saveStart_nowPoint --> " + nowPoint);
                Console.WriteLine("saveStart_nowItem --> " + nowItem);
                nowItem = nowPoint;
                if (nowItem < 품목.Count() && nowItem > -1)
                {
                    string[] testResultArayy = new string[검사체크항목.Count];
                    string[] testResultArayy2 = new string[검사체크항목.Count];
                    int i = 0;
                    foreach (var 측정 in 검사체크항목)
                    {
                        발주서별품질검사측정정보 측정정보 = new 발주서별품질검사측정정보();
                        측정정보.시리얼넘버 = 품목[nowItem].Seq;
                        측정정보.발주번호 = 발주서수입검사.발주번호;
                        측정정보.발주순번 = 발주서수입검사.발주순번;
                        측정정보.공정단위코드 = null;
                        측정정보.품질검사코드 = 측정.품질검사코드;                   //"Q000001";
                        측정정보.검사단위코드 = 측정.검사단위코드;                   //"B2401";
                        측정정보.생산품공정코드 = 발주서수입검사.품번;         //"KMFA-518986:1";
                        측정정보.생산품공정명 = 품목[nowItem].생산품공정명;     // "테스트 생산품공정"
                        측정정보.검사기준값 = 측정.검사기준값;
                        측정정보.오차범위 = 측정.오차범위;
                        측정정보.검사측정값 = 측정.검사측정값;
                        측정정보.합격여부 = 측정.합격여부;
                        측정정보.보유품목코드 = 발주서수입검사.품번;
                        측정정보.회사코드 = 발주서수입검사.회사코드;

                        var result = await Remote.Command.품질관리.수입검사품질검사측정정보유무_조회(품목[nowItem].Seq, 측정정보);

                        if (result != null)
                            측정정보.RowVersion = result.RowVersion;


                        if (측정.검사측정값 != null)
                        {
                            bool multiChk = await Remote.Command.품질관리.수입검사품질검사측정정보_저장(측정정보, result == null ? true : false);
                            if (!multiChk)
                                throw new Exception("이미처리되었습니다.");
                            //일련번호생성
                            //await Remote.Command.품질관리.품질검사측정_보유품목일련정보_저장(생산품코드, 품목[nowItem].Seq);

                            //var 검사카운트 = 품질검사목록.GroupBy(x => x.시리얼넘버 == 품목[nowItem].Seq).FirstOrDefault().Count();
                            //if (품질검사목록.Count() == (검사항목전체.Count() * Convert.ToInt32(생산지시.생산수량)))

                            var 품질검사목록 = await Remote.Command.품질관리.발주서별품질검사측정완료유무_조회(발주서수입검사.발주번호, 발주서수입검사.발주순번, 회사코드);
                            var 검사카운트 = 품질검사목록.Where(x => x.시리얼넘버 == 품목[nowItem].Seq).GroupBy(x => x.시리얼넘버).FirstOrDefault().Count();
                            //.Select(g => new { Name = g.Key, Speed = g.Min(l => l.Speed) });

                            if (검사카운트 == 검사항목전체.Count())
                                testResultArayy[i] = "완료";
                            else
                                testResultArayy[i] = "진행중";
                        }
                        else
                        {
                            testResultArayy[i] = "";
                        }

                        testResultArayy2[i] = 측정.합격여부;

                        i++;
                    }
                    string strResult;
                    string strResult2 = string.Empty;
                    if (testResultArayy.Any(c => c.Contains("진행중")) || testResultArayy.Any(c => c.Contains("완료")))
                    {

                        strResult = testResultArayy.Any(c => c.Contains("완료")) ? "완료" : "진행중";

                        if (testResultArayy2.Any(c => c.Contains("불합격")))
                            strResult2 = "불합격";
                        else
                            strResult2 = "합격";
                    }

                    else
                        strResult = "패스";

                    var foundItem = 품질검사생산품목록.FirstOrDefault(c => c.Seq == 품목[nowItem].Seq);
                    foundItem.Result = strResult;

                    //캐시 리플레쉬
                    //list_Cache.Add(new 품질검사측정정보 { 시리얼넘버 = 품목[nowItem].Seq, 합격여부 = strResult });
                    //Cache.ModifyCheckItem(list_Cache);


                    //생산지시정보 검사수량,합격수량,불량수량 업데이트


                    if (strResult.Equals("완료"))
                    {
                        await Remote.Command.품질관리.수입검사품질검사측정_측정수량_저장(품목[nowItem].생산지시코드, 품목[nowItem].발주순번, strResult2, 발주서수입검사.회사코드);
                        //await Remote.Command.품질관리.품질검사측정_보유품목코드_저장(생산품코드, 생산지시.생산계획.생산품.품목구분코드, 품목[nowItem].Seq);
                    }

                    nowItem++;

                    //btnSave.Disabled = true;
                    var SelectedRecords = await grid4.Grid.GetSelectedRecords();

                    foreach (var item in SelectedRecords)
                    {
                        var found = list4.FirstOrDefault(c => c.품질검사코드 == item.품질검사코드);
                        found.검사측정값 = null;
                        found.합격여부 = null;
                    }

                    checkCnt = 0;
                    myValue.Clear();

                    NotifyMessage(Message.CheckDataSave);
                    NotifyGlobalMessage(Services.Message.ChangeCheckState);


                    Console.WriteLine("saveEND_nowPoint --> " + nowPoint);
                    Console.WriteLine("saveEND_nowItem --> " + nowItem);
                    if (품목.Count() == nowItem)
                    {
                        //IsEnable = false;
                        //보유품목코드 저장

                        await Remote.Command.공통.수입검사포인트_설정(userId, 발주서수입검사.품번, -1, 0);
                        //ShowMessageBox(userId, "측정이 끝났습니다.다음측정을 선택하세요.", MessageBoxResultType.Okay);
                    }
                    else
                    {
                        //await grid3.SelectRow(nowItem, true);
                    }

                }
            }
            catch (Exception ex)
            {
                NotifyMessage(Message.CheckDataSaveErorr);
                //await JSRuntime.InvokeAsync<bool>("alert", "오류입니다.");
            }

        }

        private async Task OnCheckReset()
        {

            var cancle = await ShowMessageBox("측정저장", "측정을 리셋하시겠습니까?", MessageBoxResultType.YesOrNo);
            if (!cancle)
                return;

            var SelectedRecords = await grid4.Grid.GetSelectedRecords();

            foreach (var item in SelectedRecords)
            {
                var found = list4.FirstOrDefault(c => c.품질검사코드 == item.품질검사코드);
                found.검사측정값 = null;
                found.합격여부 = null;
            }
            checkCnt = 0;
            myValue.Clear();

            NotifyMessage(Message.CheckDataReset);
        }


        public async Task RowSelectHandler(RowSelectEventArgs<품질검사생산품> args)
        {

            selected품질검사생산품 = args.Data;
            //var found = list4.FirstOrDefault(x => x.공정단위코드 == selected품질검사생산품.공정단위코드 );
            var result = await Remote.Command.품질관리.수입검사품질검사측정정보_조회(발주서수입검사.발주번호,발주서수입검사.발주순번, selected품질검사생산품.Seq);
            int i = 0;

            foreach (var item in list4)
            {
                var found = result.FirstOrDefault(x => x.품질검사코드 == list4[i].품질검사코드);

                if (found != null)
                {
                    list4[i].검사측정값 = found.검사측정값; ///result.FirstOrDefault(x => x.품질검사코드 == list4[i].품질검사코드).검사측정값;
					list4[i].합격여부 = found.합격여부;//result.FirstOrDefault(x => x.품질검사코드 == list4[i].품질검사코드).합격여부;
                }
                else
                {
                    list4[i].검사측정값 = null;
                    list4[i].합격여부 = null;
                }
                i++;
            }

        }

        public void QueryCellInfoHandler(QueryCellInfoEventArgs<발주서별품질검사정보> Args)
        {
            if (Args.Column.Type == ColumnType.CheckBox && (Args.Data.합격여부 == "합격" || Args.Data.합격여부 == "불합격"))
            {
                Args.Cell.AddClass(new string[] { "e-checkbox-disabled" });
            }

        }


        class Notifier
        {
            public event EventHandler SomethingHappened;
            public void DoSomething(Dictionary<int, decimal> dic)
            {
                SomethingHappened(dic);
            }
        }
        private string ConfirmsData(발주서별품질검사정보 found)
        {
            if ((found.검사기준값 + found.오차범위상한) < found.검사측정값 || (found.검사기준값 + found.오차범위하한) > found.검사측정값)
            {
                found.합격여부 = "불합격";
            }
            else if (found.검사측정값 == null)
            {
            }
            else
            {
                found.합격여부 = "합격";
            }
            return found.합격여부;
        }

        //2021.02.01 캘리퍼스를 위한 Event
        public class MyMitutoyoTupleEventConverter : WampEventValueTupleConverter<(string, decimal)>
        { }

        public ObservableCollection<품질검사생산품> 품질검사생산품목록 { get; set; }

        public List<품질검사생산품> list3 = new List<품질검사생산품>();

        private 품질검사생산품 selected품질검사생산품;

        private SfGrid<품질검사생산품> grid3;


        public SfButton btnTest, btnCheck, btnSave;

        public class 품질검사생산품 : INotifyPropertyChanged
        {
            public int seq { get; set; }
            public string 생산지시코드 { get; set; }
            public decimal 발주순번 { get; set; }
            public string 생산품공정코드 { get; set; }
            public string 생산품공정명 { get; set; }
            public string? 공정단위코드 { get; set; }

            public string 보유품목코드 { get; set; }
            public string result { get; set; }
            [Column(TypeName = "decimal(7, 3)")]
            public decimal 검사측정값 { get; set; }
            public DateTime? CheckDate { get; set; }

            public int Seq
            {
                get { return seq; }
                set
                {
                    this.seq = value;
                    INotifyPropertyChanged("Seq");
                }
            }

            public string Result
            {
                get { return result; }
                set
                {
                    this.result = value;
                    INotifyPropertyChanged("Result");
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;

            private void INotifyPropertyChanged(string propertyName)
            {
                var handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }

        }

    }
}
