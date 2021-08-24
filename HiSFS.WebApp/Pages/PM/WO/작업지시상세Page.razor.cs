using HiSFS.Api.Shared.Models;
using HiSFS.Api.Shared.Models.View;
using HiSFS.WebApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QRCoder;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Pdf;
using Syncfusion.XlsIO;
using Syncfusion.XlsIORenderer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using WampSharp.V2;

namespace HiSFS.WebApp.Pages.PM.WO
{
    public partial class 작업지시상세Page
    {


        [Inject]
        IJSRuntime JS { get; set; }

        private bool isChecked = true;

        private bool 검사장비연결 = false;

        public bool showModal { get; set; } = false;
        //측정카운트
        private int checkCnt = 0;

        Dictionary<int, decimal> myValue = new Dictionary<int, decimal>();

        private Dictionary<int?, string> RowButton = new Dictionary<int?, string>();

        public int Sid = 0;

        //검사장비연결
        // 캘리퍼스 Subscribe 연결
        public static bool isConns = false;

        // 2021.06.26
        private bool 품검종료 = false;
        private int 현재연결장비번호 = 0;
        private IDisposable pum_subscription = null;

        private static void OnClose(object sender, WampSharp.V2.Realm.WampSessionCloseEventArgs e)
        {
            Console.WriteLine("connection closed. reason: " + e.Reason);
        }

        private static void OnError(object sender, WampSharp.Core.Listener.WampConnectionErrorEventArgs e)
        {
            Console.WriteLine("connection error. error: " + e.Exception);
        }

        private static void OnEstablished(object sender, WampSharp.V2.Realm.WampSessionCreatedEventArgs e)
        {
            Console.WriteLine("connection established. reason: " + e.ToString());
        }


        private async Task OnConnection(공정단위검사장비 info, bool isConn, int BtnSid)
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
            검사장비연결 = isConn;
            NotifyMessage(isConn == true ? Message.ConnectionAlert : Message.DisconnectAlert);
            //var bResult = await CheckAndModify(() => Remote.Command.생산관리.공정단위검사장비_저장(info, false, isConn));
            //if (bResult == false)
            //    return;

            if (!isConn)
            {
                return;
            }
            var server_Uri = await Remote.Command.공통.Server_Uri();

            DefaultWampChannelFactory factory =
                new DefaultWampChannelFactory();
            //const string serverAddress = "ws://127.0.0.1:31200/ws";

            IWampChannel channel = factory.CreateJsonChannel(server_Uri, "HiSFS.Api"); //"HongAppsRealm");

            WampSharp.V2.Client.IWampClientConnectionMonitor monitor = channel.RealmProxy.Monitor;

            monitor.ConnectionBroken += OnClose;
            monitor.ConnectionError += OnError;
            monitor.ConnectionEstablished += OnEstablished;

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
                //    RowButton[현재연결장비번호] = "연결";
                //    Sid = 0;
                //    isConn = false;
                //    isConns = false;
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

        private string ConfirmsData(공정단위검사정보 found)
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


        /// <summary>
        /// 측정적용
        /// </summary>
        /// <returns></returns>
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


                int nowPoint = await Remote.Command.공통.포인트_조회(userId, 생산지시.생산계획.생산품공정코드, 생산지시.생산지시코드, 회사코드);

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
                        품질검사측정정보 측정정보 = new 품질검사측정정보();
                        측정정보.시리얼넘버 = 품목[nowItem].Seq;
                        측정정보.생산지시코드 = 품목[nowItem].생산지시코드;
                        측정정보.공정단위코드 = 측정.공정단위코드;
                        측정정보.품질검사코드 = 측정.품질검사코드;                   //"Q000001";
                        측정정보.검사단위코드 = 측정.검사단위코드;                   //"B2401";
                        측정정보.생산품공정코드 = 품목[nowItem].생산품공정코드;         //"KMFA-518986:1";
                        측정정보.생산품공정명 = 품목[nowItem].생산품공정명;     // "테스트 생산품공정"
                        측정정보.검사기준값 = 측정.검사기준값;
                        측정정보.오차범위 = 측정.오차범위;
                        측정정보.검사측정값 = 측정.검사측정값;
                        측정정보.합격여부 = 측정.합격여부;
                        측정정보.보유품목코드 = 생산품코드;
                        측정정보.회사코드 = 회사코드;

                        var result = await Remote.Command.품질관리.품질검사측정정보유무_조회(품목[nowItem].Seq, 측정정보);

                        if (result != null)
                            측정정보.RowVersion = result.RowVersion;


                        if (측정.검사측정값 != null)
                        {
                            bool multiChk = await Remote.Command.품질관리.품질검사측정정보_저장(측정정보, result == null ? true : false);
                            if (!multiChk)
                                throw new Exception("이미처리되었습니다.");
                            //일련번호생성
                            //await Remote.Command.품질관리.품질검사측정_보유품목일련정보_저장(생산품코드, 품목[nowItem].Seq);

                            //var 검사카운트 = 품질검사목록.GroupBy(x => x.시리얼넘버 == 품목[nowItem].Seq).FirstOrDefault().Count();
                            //if (품질검사목록.Count() == (검사항목전체.Count() * Convert.ToInt32(생산지시.생산수량)))

                            var 품질검사목록 = await Remote.Command.품질관리.품질검사측정완료유무_조회(생산지시.생산지시코드, 회사코드);
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
                        await Remote.Command.품질관리.품질검사측정_생산지시측정수량_저장(품목[nowItem].생산지시코드, strResult2, 총품질검사수량);
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

                        await Remote.Command.공통.포인트_설정(userId, 생산지시.생산계획.생산품공정코드, -1, 0);
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

        private async Task OnNextTest()
        {
            var userId = await SessionStorage.GetAsync<string>("userId");

            //int 포인트항목  =  await Remote.Command.공통.포인트항목_조회(userId);
            //포인트항목++;
            await Remote.Command.공통.포인트_설정(userId, 생산지시.생산계획.생산품공정코드, -1, 0);

            //await grid4.Grid.ClearSelection();
            //IsEnable = true;
            //await grid3.SelectRow(0, true);

        }

        delegate void EventHandler(Dictionary<int, decimal> dic);

        class Notifier
        {
            public event EventHandler SomethingHappened;
            public void DoSomething(Dictionary<int, decimal> dic)
            {
                SomethingHappened(dic);
            }
        }

        private string confirms(공정단위검사정보 found)
        {
            if ((found.검사기준값 + found.오차범위) < found.검사측정값 || (found.검사기준값 - found.오차범위) > found.검사측정값)
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
        private decimal RandomCheck()
        {
            Random r = new Random();
            r.Next();   // 랜덤값 생성
            return r.Next(1, 10);
        }

        private async Task OnCheck()
        {
            var 품질측정품목체크 = await grid4.Grid.GetSelectedRecords();
            if (품질측정품목체크.Count == 0)
            {
                NotifyMessage(Message.SelectAlert);
                return;
            }

            foreach (var item in 품질측정품목체크)
            {
                var found = await Task.FromResult(list4.FirstOrDefault(c => c.품질검사코드 == item.품질검사코드));
                found.검사측정값 = RandomCheck(); ;
                found.합격여부 = confirms(found);

            }

            //btnCheck.Disabled = true;
            NotifyMessage(Message.SendMessage);
        }


        /*private async Task OnLoopSave()
        {
            var 검사항목체크 = await grid4.Grid.GetSelectedRecords();
            var 품목체크 = await grid3.GetSelectedRecords();
            foreach (var 품목 in 품목체크)
            {
                foreach (var 측정 in 검사항목체크)
                {
                    품질검사측정정보 측정정보 = new 품질검사측정정보();
                    측정정보.시리얼넘버 = 품목.Seq;
                    측정정보.공정단위코드 = 측정.공정단위코드;
                    측정정보.품질검사코드 = 측정.품질검사코드;    //"Q000001";
                    측정정보.검사단위코드 = 측정.검사단위코드;  //"B2401";
                    측정정보.생산품코드 = 품목.생산품코드;  //"KMFA-518986:1";
                    측정정보.생산품공정명 = 품목.생산품공정명; // "테스트 생산품공정"
                    측정정보.검사기준값 = 측정.검사기준값;
                    측정정보.오차범위 = 측정.오차범위;
                    측정정보.검사측정값 = 측정.검사측정값;
                    측정정보.합격여부 = 측정.합격여부;

                    var result = await Remote.Command.품질관리.품질검사측정정보유무_조회(품목체크[nowItem].Seq, 측정.품질검사코드);

                    if (result == null)
                        await Remote.Command.품질관리.품질검사측정정보_저장(측정정보, true);
                    else
                        await Remote.Command.품질관리.품질검사측정정보_저장(측정정보, false);
                }

                btnCheck.Disabled = true;
                btnLoop.Disabled = false;
                do
                {
                    //var name = await JSRuntime.InvokeAsync<bool>("confirm", "확인하세요");

                    if (isChecked == false)
                    {
                        var found = 품질검사생산품목록.FirstOrDefault(c => c.Seq == 품목.Seq);
                        found.Result = "완료";
                        continue;
                    }
                    await Task.Delay(2000);

                } while (isChecked);


                NotifyMessage(Message.ModifiedUpdateData);
                //StateHasChanged();
            }

        }*/

        public static int nowItem { get; set; }

        public async Task RowSelectHandler(RowSelectEventArgs<품질검사생산품> args)
        {

            selected품질검사생산품 = args.Data;
            //var found = list4.FirstOrDefault(x => x.공정단위코드 == selected품질검사생산품.공정단위코드 );
            var result = await Remote.Command.품질관리.품질검사측정정보_조회(생산지시.생산지시코드, selected품질검사생산품.Seq);
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

        private void OnRowDataBound(RowDataBoundEventArgs<공정단위검사정보> args)
        {
            //await Task.Yield();
            //if (args.Data.합격여부 != null)
            //{
            //    args.Row.AddClass(new string[] { "e-checkbox-disabled" });
            //}
        }


        public void QueryCellInfoHandler(QueryCellInfoEventArgs<공정단위검사정보> Args)
        {
            if (Args.Column.Type == ColumnType.CheckBox && (Args.Data.합격여부 == "합격" || Args.Data.합격여부 == "불합격"))
            {
                Args.Cell.AddClass(new string[] { "e-checkbox-disabled" });
            }

        }



        private async Task OnSelected(Syncfusion.Blazor.Navigations.SelectEventArgs e)
        {

            await Task.Delay(200);

            var index = e.SelectedIndex;
            selected차수 = index;

            //var info = await Remote.Command.생산관리.작업지시_완제품_조회(생산지시.생산지시코드, index);
            //보유품목코드 = info?.보유품목코드 ?? "";

            공정유형코드 = 생산지시.생산지시공정차수목록[selected차수 - 1].생산품공정차수.공정단위.공정.공정유형코드;
            생산품코드 = 생산지시.생산계획.생산품코드;

            공정단위코드 = 생산지시.생산지시공정차수목록[selected차수 - 1].생산품공정차수.공정단위코드;
            var result = await Remote.Command.생산관리.공정단위검사_조회(공정단위코드);

            //Value =@($"00 S9108 {생산지시.생산지시코드} {공정차수.생산품공정차수.공정단위.공정단위코드}")
            gong_barcodeStart = $"00 S9108 {생산지시.생산지시코드} {생산지시.생산지시공정차수목록[selected차수 - 1].생산품공정차수.공정단위.공정단위코드}";
            gong_barcodeEnd = $"00 S9109 {생산지시.생산지시코드} {생산지시.생산지시공정차수목록[selected차수 - 1].생산품공정차수.공정단위.공정단위코드}";

            실적등록바코드 = $"00 S9140 {생산지시.생산지시코드} {생산지시.생산지시공정차수목록[selected차수 - 1].생산품공정차수.공정단위.공정단위코드}";

            공정실적등록바코드 = $"00 S9221 {생산지시.생산지시코드} {생산지시.생산지시공정차수목록[selected차수 - 1].생산품공정차수.공정단위.공정단위코드}";

            //if (result.Count() == 0 && 공정유형코드 == "B0205")
            //{
            //    NotifyMessage(Message.CheckDataAddReq);
            //    return;
            //}

            ModifyList(result.OrderByDescending(x => x.품질검사코드).AsEnumerable(), (x, map) =>
            {
                if (x.검사단위코드 != null)
                    x.검사단위 = map[x.검사단위코드];
            });

            // var result2 = result.OrderBy(x => x.품질검사코드).AsEnumerable();

            list4 = result.ToObservableCollection();

            //var list4_result = list4.Select(x => x.공정검사장비목록).ToList();

            //ModifyList(list4, (x, map) =>
            //{
            //    if (x.검사장비?.연동장비유형코드 != null)
            //        x.검사장비.연동장비유형 = map[x.검사장비?.연동장비유형코드];
            //});
            list5?.Clear();

            int i = 1;

            foreach (var item in list4)
            {
                if (item != null)
                {

                    foreach (var item2 in item.공정검사장비목록)
                    {
                        item2.No = i;
                        list5.Add(item2);
                        RowButton[item2.검사장비식별번호] = "연결";
                        i++;
                    }
                }
            }
            StateHasChanged();

            if (공정유형코드 == "B0205")
            {
                var 품질검사목록 = await Remote.Command.품질관리.품질검사측정완료유무_조회(생산지시.생산지시코드, 회사코드);
                var 검사항목전체 = grid4.Grid.GetCurrentViewRecords();

                Console.WriteLine("검사항목전체.Count()--> " + list4.Count);
                //품목라스트포인트 =  품질검사목록.OrderByDescending(i => i.시리얼넘버 ).First().시리얼넘버;

                list3 = Enumerable.Range(1, Convert.ToInt32(총품질검사수량 > 0 ? 총품질검사수량 : 생산지시.생산수량)).Select(cnt => new 품질검사생산품()
                {
                    Seq = cnt,
                    생산지시코드 = 생산지시.생산지시코드,
                    생산품공정코드 = 생산지시.생산계획.생산품공정.생산품공정코드,
                    생산품공정명 = 생산지시.생산계획.생산품공정.생산품공정명,
                    공정단위코드 = 생산지시.생산지시공정차수목록[selected차수 - 1].생산품공정차수.공정단위코드,
                    Result =
                품질검사목록.Where(x => x.시리얼넘버 == cnt).Count() == list4.Count ? "완료" : (품질검사목록.Where(x => x.시리얼넘버 == cnt).Count() == 0 ? "" : "진행중"),

                    //Result = (from m in 품질검사목록 where m.시리얼넘버 == cnt && m.생산지시코드 == 생산지시.생산지시코드 select m.합격여부).FirstOrDefault() != null ? "완료" : ""  ,
                    보유품목코드 = 생산지시.생산계획.생산품코드,
                    CheckDate = DateTime.Now.AddDays(-cnt),
                }).ToList();
                품질검사생산품목록 = new ObservableCollection<품질검사생산품>(list3);

                공정버튼활성여부 = true;

                if (생산지시.실행상태코드 == "B2004")
                    실적등록버튼활성유무 = true;

            }
            else
            {
                var 공정단위자재목록 = await Remote.Command.기준정보.공정단위자재현황_조회(회사코드, 공정단위코드);

                var 외주생산이동유무 = await Remote.Command.기준정보.외주생산위치품목_조회(회사코드, 생산지시.생산계획.주문번호, 공정단위자재목록[0].자재코드);

                var 생산실적등록유무 = await Remote.Command.기준정보.공정단위생산실적헤더정보_조회(회사코드, 생산지시.생산지시코드, 공정단위코드);

                공정버튼활성여부 = false;

                if (외주생산이동유무 != null)
                {
                    생산이동완료유무 = "완료";
                    공정버튼활성여부 = false;
                }
                else
                {
                    생산이동완료유무 = "미완료";
                    공정버튼활성여부 = true;
                }

                if (생산실적등록유무 != null)
                    공정버튼활성여부 = true;

                if (생산지시.실행상태코드 == "B2004")
                    공정버튼활성여부 = true;


            }

            //RefreshCheckAfterAsync();

            pum_barcodeStart = $"00 S9112 {생산지시.생산지시코드} {생산지시.생산지시공정차수목록[selected차수 - 1].생산품공정차수.공정단위.공정단위코드}";
            pum_barcodeEnd = $"00 S9113 {생산지시.생산지시코드} {생산지시.생산지시공정차수목록[selected차수 - 1].생산품공정차수.공정단위.공정단위코드}";


            StateHasChanged();

        }


        private async Task RefreshAsync()
        {

            회사코드 = await SessionStorage.GetAsync<string>("회사코드");
            생산지시 = null;
            StateHasChanged();

            if (Code == null)
                Code = Params as string;
            생산지시 = await Remote.Command.생산관리.작업지시상세_조회(Code, 회사코드);
            if (생산지시 == null)
                return;

            실행상태코드 = 생산지시.실행상태코드;

            if (실행상태코드 == "B2001")
                보기모드 = false;
            else
                보기모드 = true;

            #region 검사수량가져오기

            var result = await Remote.Command.기준정보.작업지시상세_생산실적헤더정보_조회(회사코드, 생산지시.생산지시코드);
            if (검사수량입력 == null)
            {
                var 수량 = result.GroupBy(x => new { x.생산지시코드 }).Select(g => g.FirstOrDefault()).ToList();
                Console.WriteLine("수량  --> " + 수량);
                if (수량.Count > 0)
                    검사수량입력 = Convert.ToString(Convert.ToInt32(수량[0].실적수량));
                else
                    검사수량입력 = Convert.ToString(Convert.ToInt32(생산지시.생산수량));

                총품질검사수량 = Convert.ToInt32(검사수량입력);
            }



            #endregion

            if (생산지시.실행상태코드 == "B2004")
            {
                IsBtnStartEnable = true;
                IsBtnEndEnable = true;
                IsEnable = false;
                IsBtnEnable = true;

                IsBtnProcStartEnable = true;
                IsBtnProcEndEnable = true;
                공정버튼활성여부 = true;
                실적등록버튼활성유무 = true;
            }

            StateHasChanged();
        }


        private async Task RefreshCheckAfterAsync()
        {

            if (list3.Count > 0 && 공정유형코드 == "B0205")
            {
                var 품질검사목록 = await Remote.Command.품질관리.품질검사측정완료유무_조회(생산지시.생산지시코드, 회사코드);

                var 검사항목전체 = await grid4.Grid.GetCurrentViewRecords();

                //var 검사카운트 = 품질검사목록.GroupBy(x => x.시리얼넘버).Select(g => g.First()).ToList();

                foreach (var item in 품질검사목록)
                {
                    var found = list3.FirstOrDefault(x => x.Seq == item.시리얼넘버);
                    //var 검사차수 = 품질검사목록.GroupBy(x => x.시리얼넘버 == item.시리얼넘버).FirstOrDefault().Count();
                    var 검사카운트 = 품질검사목록.Where(x => x.시리얼넘버 == item.시리얼넘버).GroupBy(x => x.시리얼넘버).FirstOrDefault().Count();
                    found.Result = 검사항목전체.Count() == 검사카운트 ? "완료" : "진행중";
                    //found.Result = (Convert.ToInt32(생산지시.생산수량) * 검사항목전체.Count()) == 품질검사목록.Count() ? "완료" : "진행중"; //item.합격여부 != null ? "완료" : "";

                }
                if (검사수량입력_숫자 > 0)
                {
                    if (nowItem == 검사수량입력_숫자)
                    {
                        var userId = await SessionStorage.GetAsync<string>("userId");
                        await Remote.Command.공통.포인트_설정(userId, 생산지시.생산계획.생산품공정코드, -1, 0);
                        await grid4.Grid.ClearRowSelection();
                        await grid3.SelectRow(0, true);
                    }
                    else
                    {
                        await grid3.SelectRow(nowItem, true);
                    }
                }
                else if (총품질검사수량 > 0)
                {
                    if (nowItem == 총품질검사수량)
                    {
                        var userId = await SessionStorage.GetAsync<string>("userId");
                        await Remote.Command.공통.포인트_설정(userId, 생산지시.생산계획.생산품공정코드, -1, 0);
                        await grid4.Grid.ClearRowSelection();
                        await grid3.SelectRow(0, true);
                    }
                    else
                    {
                        await grid3.SelectRow(nowItem, true);
                    }
                }
                else
                {
                    if (nowItem == Convert.ToInt32(생산지시.생산수량))
                    {
                        var userId = await SessionStorage.GetAsync<string>("userId");
                        await Remote.Command.공통.포인트_설정(userId, 생산지시.생산계획.생산품공정코드, -1, 0);
                        await grid4.Grid.ClearRowSelection();
                        await grid3.SelectRow(0, true);
                    }
                    else
                    {
                        await grid3.SelectRow(nowItem, true);
                    }
                }

                Console.WriteLine("RefreshCheckAfterAsync --> " + nowItem);

                await InvokeAsync(() =>
                {
                    StateHasChanged();
                });

            }
        }

        protected async override void OnReceivedMessage(Services.Message message, bool isGlobal, dynamic[] args)
        {
            base.OnReceivedMessage(message, isGlobal, args);


            if (message == Services.Message.StartScan)
            {
                var user_id = args[0] as string;

                var userId = await SessionStorage.GetAsync<string>("userId");
                if (user_id == userId.ToString())
                {
                    if (btnStartFlag == false)
                    {
                        btnStartFlag = true;

                        IsBtnStartEnable = true;
                        IsBtnEndEnable = false;

                        //btnStart.Disabled = true;
                        //btnEnd.Disabled = false;
                        var 검사항목수 = await grid4.Grid.GetCurrentViewRecords();
                        //await Remote.Command.공통.포인트_저장(userId.ToString(), 생산지시.생산계획.생산품공정코드, Convert.ToInt32(생산지시.생산수량));
                        await Remote.Command.공통.포인트_저장2(userId, 생산지시.생산계획.생산품공정코드, 총품질검사수량 > 0 ? 총품질검사수량 : Convert.ToInt32(생산지시.생산수량), 검사항목수.Count);
                        await Remote.Command.품질관리.품질검사시작_보유품목코드_저장(생산품코드, 생산지시.생산계획.생산품.품목구분코드, 총품질검사수량 > 0 ? 총품질검사수량 : Convert.ToInt32(생산지시.생산수량), 회사코드);

                        // 품질검사시작시 켈리퍼스 측정해야 보유품목정보 수량업데이트 됨
                        /* 20210513  재정의
                        await Remote.Command.품질관리.품질검사시작_보유품목코드_저장(생산품코드, 생산지시.생산계획.생산품.품목구분코드, Convert.ToInt32(생산지시.생산수량));

                        await Remote.Command.품질관리.보유품목일지_저장(생산지시.생산계획.생산품코드, Convert.ToInt32(생산지시.생산수량), 생산지시.생산지시코드);

                        await Remote.Command.품질관리.품질검사측정_보유품목일련번호생성_저장(생산지시.생산지시코드, 생산지시.생산계획.생산품코드, Convert.ToInt32(생산지시.생산수량));
                        */

                        NotifyMessage(Message.CheckStart);
                    }
                    else
                    {
                        await Remote.Command.공통.PDA_메시지(user_id);
                        ShowMessageBox(user_id, "검사중 입니다 확인바랍니다", MessageBoxResultType.Okay);
                    }

                }
                else
                {
                    await Remote.Command.공통.PDA_메시지(user_id);
                    NotifyMessage(Message.CheckStart);
                    ShowMessageBox(user_id, "사용자 ID가 틀립니다 확인바랍니다", MessageBoxResultType.Okay);
                }
            }

            if (message == Services.Message.EndScan)
            {
                var user_id = args[0] as string;

                var userId = await SessionStorage.GetAsync<string>("userId");
                if (user_id == userId.ToString())
                {
                    if (btnStartFlag == true)
                    {
                        btnStartFlag = false;

                        IsBtnStartEnable = false;
                        IsBtnEndEnable = true;
                        //btnStart.Disabled = false;
                        //btnEnd.Disabled = true;
                        await Remote.Command.공통.포인트_삭제(userId.ToString(), 생산지시.생산계획.생산품공정코드);

                        NotifyMessage(Message.CheckEnd);
                    }
                    else
                    {
                        await Remote.Command.공통.PDA_메시지(user_id);
                        ShowMessageBox(user_id, "검사중인 것이 없습니다 확인바랍니다", MessageBoxResultType.Okay);
                    }
                }
                else
                {
                    await Remote.Command.공통.PDA_메시지(user_id);
                    NotifyMessage(Message.CheckStart);
                    ShowMessageBox(user_id, "사용자 ID가 틀립니다 확인바랍니다", MessageBoxResultType.Okay);
                }
            }


            //공정시작
            if (message == Services.Message.StartProcScan)
            {
                var user_id = args[0] as string;

                var userId = await SessionStorage.GetAsync<string>("userId");
                if (user_id == userId.ToString())
                {
                    if (btnProcStartFlag == false)
                    {
                        btnProcStartFlag = true;

                        IsBtnProcStartEnable = true;
                        IsBtnProcEndEnable = false;


                        //await Remote.Command.생산관리.공정이력정보_저장(생산지시, 생산지시.생산지시공정차수목록[selected차수 - 1], 0, "공정시작", true);

                        NotifyMessage(Message.CheckProcStart);
                    }
                    else
                    {
                        await Remote.Command.공통.PDA_공정메시지(user_id);
                        ShowMessageBox(user_id, "공정작업중 입니다 확인바랍니다", MessageBoxResultType.Okay);
                    }
                }
                else
                {
                    await Remote.Command.공통.PDA_공정메시지(user_id);
                    NotifyMessage(Message.CheckProcStart);
                    ShowMessageBox(user_id, "사용자 ID가 틀립니다 확인바랍니다", MessageBoxResultType.Okay);
                }
            }

            //공정종료
            if (message == Services.Message.EndProcScan)
            {
                var user_id = args[0] as string;

                var userId = await SessionStorage.GetAsync<string>("userId");
                if (user_id == userId.ToString())
                {
                    if (btnProcStartFlag == true)
                    {
                        btnProcStartFlag = false;

                        IsBtnProcStartEnable = false;
                        IsBtnProcEndEnable = true;


                        NotifyMessage(Message.CheckProcEnd);
                    }
                    else
                    {
                        //await Remote.Command.공통.PDA_공정메시지(user_id);
                        ShowMessageBox(user_id, "공정작업중인 것이 없습니다 확인바랍니다", MessageBoxResultType.Okay);
                    }
                }
                else
                {
                    //await Remote.Command.공통.PDA_공정메시지(user_id);
                    NotifyMessage(Message.CheckProcStart);
                    ShowMessageBox(user_id, "사용자 ID가 틀립니다 확인바랍니다", MessageBoxResultType.Okay);
                }
            }

            if (isGlobal == true && message == Services.Message.ChangeCheckState)
            {
                RefreshCheckAfterAsync();
            }

        }

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
                //btnStart.Disabled = true;
                //btnEnd.Disabled = false;
                var userId = await SessionStorage.GetAsync<string>("userId");
                //await Remote.Command.공통.포인트_저장(userId, 생산지시.생산계획.생산품공정코드, Convert.ToInt32(생산지시.생산수량), 검사항목수.Count);
                await Remote.Command.공통.포인트_저장2(userId, 생산지시.생산계획.생산품공정코드, Convert.ToInt32(생산지시.생산수량), 검사항목수.Count);


                await Remote.Command.품질관리.품질검사시작_보유품목코드_저장(생산품코드, 생산지시.생산계획.생산품.품목구분코드, Convert.ToInt32(생산지시.생산수량), 회사코드);

                /* 보유품목 다시 정의  20210513

                await Remote.Command.품질관리.보유품목일지_저장(생산지시.생산계획.생산품코드, Convert.ToInt32(생산지시.생산수량), 생산지시.생산지시코드);
                await Remote.Command.품질관리.품질검사측정_보유품목일련번호생성_저장(생산지시.생산지시코드, 생산지시.생산계획.생산품코드, Convert.ToInt32(생산지시.생산수량));
                */

                NotifyMessage(Message.CheckStart);
            }

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
                //this.btnStart.Disabled = false;
                //btnEnd.Disabled = true;
                var userId = await SessionStorage.GetAsync<string>("userId");
                bool nowPoint = await Remote.Command.공통.포인트_삭제(userId, 생산지시.생산계획.생산품공정코드);

                //var server_Uri = await Remote.Command.공통.Server_Uri();
                //DefaultWampChannelFactory factory =       new DefaultWampChannelFactory();
                //IWampChannel channel = factory.CreateJsonChannel(server_Uri, "HiSFS.Api"); //"HongAppsRealm");

                //GoodbyeMessage closeResponse = await channel.Close("Goodbye", new GoodbyeDetails()).ConfigureAwait(false);

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

        private async Task OnPDFDown()
        {
            //HelloWorldPDFService service = new HelloWorldPDFService();
            //MemoryStream documentStream = CreatePdfDocument();
            //await JS.SaveAsFileAsync("Helloworld.pdf", documentStream.ToArray());
        }


        private string documentPath;
        private bool isShowPdfViewer;
        private QRCodeGenerator qrGenerator = new QRCodeGenerator();

        private async Task OnShowWorkOrder(생산지시공정차수정보 info)
        {
            //var detail = await Remote.Command.생산관리.작업지시상세_조회(info.생산지시코드);
            var buffer = MakeWorkOrderPdf(info);
            string filename = "작업지시서_" + DateTime.Now.ToString("yyyyMMddhhmmsstt");
            var pdfFilename = $"{filename}.pdf";
            var tempFilename = Path.Combine(Environment.WebRootPath, "Temp", pdfFilename);
            File.WriteAllBytes(tempFilename, buffer);

            documentPath = $"wwwroot/Temp/{pdfFilename}";

            isShowPdfViewer = true;

        }

        private byte[] MakeWorkOrderPdf(생산지시공정차수정보 info)
        {
            var finalDoc = new PdfDocument();
            var pages = new List<Stream>();

            var renderer = new XlsIORenderer();
            var settings = new XlsIORendererSettings();
            settings.IsConvertBlankPage = false;
            settings.LayoutOptions = LayoutOptions.FitSheetOnOnePage;
            settings.DisplayGridLines = GridLinesDisplayStyle.Invisible;


            using var ws = MakeWorkOrderExcel(info);
            settings.TemplateDocument = new PdfDocument();
            var document = renderer.ConvertToPDF(ws, settings);
            var sPdf = new MemoryStream();
            document.Save(sPdf);
            sPdf.Seek(0, SeekOrigin.Begin);
            pages.Add(sPdf);
            PdfDocumentBase.Merge(finalDoc, pages.ToArray());
            using var ms = new MemoryStream();
            finalDoc.Save(ms);

            return ms.ToArray();
        }

        private Stream MakeWorkOrderExcel(생산지시공정차수정보 공정차수)
        {
            var workOrderTemplate = Path.Combine(Environment.WebRootPath, "Forms", "작업지시서.xlsx");

            using var excelEngine = new ExcelEngine();
            var application = excelEngine.Excel;
            application.DefaultVersion = ExcelVersion.Excel2016;
            using var rs = new MemoryStream(File.ReadAllBytes(workOrderTemplate));
            //var workbook = application.Workbooks.Open(rs);
            //var sheet = workbook.Worksheets[0];


            IWorkbook workbook = application.Workbooks.Open(rs);
            IWorksheet sheet = workbook.Worksheets[0];

            /*
			sheet.Range["A2:I2"].Merge(true);

            sheet.Range["A2"].Text = "작업지시서";
			sheet.Range["A2"].CellStyle.Font.FontName = "Verdana";
			sheet.Range["A2"].CellStyle.Font.Bold = true;
			sheet.Range["A2"].CellStyle.Font.Size = 30;
			sheet.Range["A2"].CellStyle.Font.RGBColor = Syncfusion.Drawing.Color.FromArgb(0, 0, 0, 0);
			sheet.Range["A2"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

            //////////////////////
            // 항목 전개

            IListObject table1 = sheet.ListObjects.Create("Table1", sheet["A18:J40"]);
            table1.BuiltInTableStyle = TableBuiltInStyles.TableStyleLight4;

            

            sheet.Range["A18:C18"].Merge(true);
            sheet.Range[18, 1].CellStyle.Font.Bold = true;
            sheet.Range[18, 1].CellStyle.Font.Size = 16;
            sheet.Range[18, 3].CellStyle.Font.Size = 16;
            sheet.Range[18, 1].Text = "공정유형";
			sheet.Range[18, 3].Text = 공정차수.생산품공정차수.공정단위.공정.공정명;

            sheet.Range["A20:C20"].Merge(true);
            sheet.Range[20, 1].CellStyle.Font.Bold = true;
            sheet.Range[20, 1].CellStyle.Font.Size = 16;
            sheet.Range[20, 3].CellStyle.Font.Size = 16;
            sheet.Range[20, 1].Text = "공정";
			sheet.Range[20, 3].Text = 공정차수.생산품공정차수.공정단위.공정단위명;

            sheet.Range["A22:C22"].Merge(true);
            sheet.Range[22, 1].CellStyle.Font.Bold = true;
            sheet.Range[22, 1].CellStyle.Font.Size = 16;
            sheet.Range[22, 3].CellStyle.Font.Size = 16;
            sheet.Range[22, 1].Text = "생산품";
            sheet.Range[22, 3].Text = 공정차수.생산품공정차수.공정단위.완제품코드 + "    " + 공정차수.생산품공정차수.공정단위.완제품?.품목명;

            sheet.Range["A24:C24"].Merge(true);
            sheet.Range[24, 1].CellStyle.Font.Bold = true;
            sheet.Range[24, 1].CellStyle.Font.Size = 16;
            sheet.Range[24, 3].CellStyle.Font.Size = 16;
            sheet.Range[24, 1].Text = "공정품";
            sheet.Range[24, 3].Text = 공정차수.생산품공정차수.공정단위.공정품코드 + "    " + 공정차수.생산품공정차수.공정단위.공정품?.품목명;

            sheet.Range["A26:C26"].Merge(true);
            sheet.Range[26, 1].CellStyle.Font.Bold = true;
            sheet.Range[26, 1].CellStyle.Font.Size = 16;
            sheet.Range[26, 3].CellStyle.Font.Size = 16;
            sheet.Range[26, 1].Text = "공정기준시간";
            sheet.Range[26, 3].Text = 공정차수.생산품공정차수.공정단위.공정예상시간.ToString();

            sheet.Range["A28:C28"].Merge(true);
            sheet.Range[28, 1].CellStyle.Font.Bold = true;
            sheet.Range[28, 1].CellStyle.Font.Size = 16;
            sheet.Range[28, 3].CellStyle.Font.Size = 16;
            sheet.Range[28, 1].Text = "시작일";
            sheet.Range[28, 3].Text = 생산지시.시작일?.ToString("yyyy/MM/dd");

            sheet.Range["A30:C30,"].Merge(true);
            sheet.Range[30, 1].CellStyle.Font.Bold = true;
            sheet.Range[30, 1].CellStyle.Font.Size = 16;
            sheet.Range[30, 3].CellStyle.Font.Size = 16;
            sheet.Range[30, 1].Text = "완료목표일";
            sheet.Range[30, 3].Text = 생산지시.완료목표일?.ToString("yyyy/MM/dd");

            sheet.Range["A32:C32,"].Merge(true);
            sheet.Range[32, 1].CellStyle.Font.Bold = true;
            sheet.Range[32, 1].CellStyle.Font.Size = 16;
            sheet.Range[32, 3].CellStyle.Font.Size = 16;
            sheet.Range[32, 1].Text = "지시수량";
            sheet.Range[32, 3].Text = 생산지시.생산수량.ToString();

            sheet.Range["A34:C34,"].Merge(true);
            sheet.Range[34, 1].CellStyle.Font.Bold = true;
            sheet.Range[34, 1].CellStyle.Font.Size = 16;
            sheet.Range[34, 3].CellStyle.Font.Size = 16;
            sheet.Range[34, 1].Text = "공정담당자";
            sheet.Range[34, 3].Text = 공정차수.작업자?.사용자명;

            sheet.Range["A36:C36,"].Merge(true);
            sheet.Range[36, 1].CellStyle.Font.Bold = true;
            sheet.Range[36, 1].CellStyle.Font.Size = 16;
            sheet.Range[36, 3].CellStyle.Font.Size = 16;
            sheet.Range[36, 1].Text = "도면번호";
            sheet.Range[36, 3].Text = 공정차수.생산품공정차수.공정단위.도면?.도면번호;


            sheet.Range["A38:C38,"].Merge(true);
            sheet.Range[38, 1].CellStyle.Font.Bold = true;
            sheet.Range[38, 1].CellStyle.Font.Size = 16;
            sheet.Range[38, 3].CellStyle.Font.Size = 16;
            sheet.Range[38, 1].Text = "도면명";
            sheet.Range[38, 3].Text = 공정차수.생산품공정차수.공정단위.도면?.도면명;

            sheet.Range["A40:C40,"].Merge(true);
            sheet.Range[40, 1].CellStyle.Font.Bold = true;
            sheet.Range[40, 1].CellStyle.Font.Size = 16;
            sheet.Range[40, 3].CellStyle.Font.Size = 16;
            sheet.Range[40, 1].Text = "비고";
            sheet.Range[40, 3].Text = 공정차수.비고;

            //sheet.Range["A17:J40"].Merge(true);
            //sheet.Range["A17:J40"].CellStyle.Borders.LineStyle = ExcelLineStyle.Medium;
            //sheet.Range["A17:J40"].CellStyle.Borders[ExcelBordersIndex.DiagonalDown].ShowDiagonalLine = false;
            //sheet.Range["A17:J40"].CellStyle.Borders[ExcelBordersIndex.DiagonalUp].ShowDiagonalLine = false;




            if (gong_barcodeStart != null)
            {
                sheet.Range[8, 1].CellStyle.Font.Bold = true;
                sheet.Range[8, 1].CellStyle.Font.Size = 15;
                sheet[8, 1].Text = "공정 시작";
                var ms = new MemoryStream();
                MakeQrCode(gong_barcodeStart).Save(ms, ImageFormat.Png);
                sheet.Pictures.AddPicture(6, 2, ms, 30, 30);
                ms.Dispose();
            }
            if (gong_barcodeStart != null)
            {
                sheet.Range[50, 1].CellStyle.Font.Bold = true;
                sheet.Range[50, 1].CellStyle.Font.Size = 15;
                sheet[50, 1].Text = "공정 종료";
                var ms = new MemoryStream();
                MakeQrCode(gong_barcodeEnd).Save(ms, ImageFormat.Png);
                sheet.Pictures.AddPicture(48, 2, ms, 30, 30);
                ms.Dispose();
            }
            if (공정차수.생산품공정차수.공정단위.공정.설비사용유무 == true && 공정차수.생산품공정차수.공정단위.공정설비목록 != null)
            {
                sheet.Range[8, 5].CellStyle.Font.Bold = true;
                sheet.Range[8, 5].CellStyle.Font.Size = 15;
                sheet[8, 5].Text = "설비 코드";
                var ms = new MemoryStream();
                MakeQrCode(공정차수.생산품공정차수.공정단위.공정설비목록[0].설비코드).Save(ms, ImageFormat.Png);
                sheet.Pictures.AddPicture(6, 7, ms, 35, 35);
                ms.Dispose();
            }
            */

            sheet.IsGridLinesVisible = false;
            sheet.Range["A2:I2"].Merge(true);
            //Insert Rich Text
            sheet.Range["A2"].Text = "작업지시서";
            sheet.Range["A2"].CellStyle.Font.FontName = "Verdana";
            sheet.Range["A2"].CellStyle.Font.Bold = true;
            sheet.Range["A2"].CellStyle.Font.Size = 31;
            sheet.Range["A2"].CellStyle.Font.RGBColor = Syncfusion.Drawing.Color.FromArgb(0, 0, 0, 0);
            sheet.Range["A2"].HorizontalAlignment = ExcelHAlign.HAlignCenter;


            sheet.Range["F10:F24"].HorizontalAlignment = ExcelHAlign.HAlignLeft;
            sheet.Range["G14:H34"].HorizontalAlignment = ExcelHAlign.HAlignLeft;


            sheet.Range["F10:F24"].CellStyle.Font.FontName = "Arial";

            sheet.Range["B18"].Text = "  공정유형";
            sheet.Range["B18"].CellStyle.Font.Size = 18;
            sheet.Range["B18"].CellStyle.Font.Bold = true;

            sheet.Range["B20"].Text = "  공정";
            sheet.Range["B20"].CellStyle.Font.Size = 18;
            sheet.Range["B20"].CellStyle.Font.Bold = true;

            sheet.Range["B22"].Text = "  생산품";
            sheet.Range["B22"].CellStyle.Font.Size = 18;
            sheet.Range["B22"].CellStyle.Font.Bold = true;

            sheet.Range["B24"].Text = "  공정품";
            sheet.Range["B24"].CellStyle.Font.Size = 18;
            sheet.Range["B24"].CellStyle.Font.Bold = true;

            sheet.Range["B26"].Text = "  공정기준시간";
            sheet.Range["B26"].CellStyle.Font.Size = 18;
            sheet.Range["B26"].CellStyle.Font.Bold = true;

            sheet.Range["B28"].Text = "  시작일";
            sheet.Range["B28"].CellStyle.Font.Size = 18;
            sheet.Range["B28"].CellStyle.Font.Bold = true;

            sheet.Range["B30"].Text = "  완료목표일";
            sheet.Range["B30"].CellStyle.Font.Size = 18;
            sheet.Range["B30"].CellStyle.Font.Bold = true;

            sheet.Range["B32"].Text = "  지시수량";
            sheet.Range["B32"].CellStyle.Font.Size = 18;
            sheet.Range["B32"].CellStyle.Font.Bold = true;

            sheet.Range["B34"].Text = "  공정담당자";
            sheet.Range["B34"].CellStyle.Font.Size = 18;
            sheet.Range["B34"].CellStyle.Font.Bold = true;

            sheet.Range["B36"].Text = "  도면번호";
            sheet.Range["B36"].CellStyle.Font.Size = 18;
            sheet.Range["B36"].CellStyle.Font.Bold = true;

            sheet.Range["B38"].Text = "  비고";
            sheet.Range["B38"].CellStyle.Font.Size = 18;
            sheet.Range["B38"].CellStyle.Font.Bold = true;

            sheet.Range["F18"].Text = 공정차수.생산품공정차수.공정단위.공정.공정명;
            sheet.Range["F18"].CellStyle.Font.Size = 17;
            sheet.Range["F20"].Text = 공정차수.생산품공정차수.공정단위.공정단위명;
            sheet.Range["F20"].CellStyle.Font.Size = 17;
            sheet.Range["F22"].Text = 공정차수.생산품공정차수.공정단위.완제품코드 + "    " + 공정차수.생산품공정차수.공정단위.완제품?.품목명;
            sheet.Range["F22"].CellStyle.Font.Size = 17;
            sheet.Range["F24"].Text = 공정차수.생산품공정차수.공정단위.공정품코드 + "    " + 공정차수.생산품공정차수.공정단위.공정품?.품목명;
            sheet.Range["F24"].CellStyle.Font.Size = 17;
            sheet.Range["F26"].Text = 공정차수.생산품공정차수.공정단위.공정예상시간.ToString();
            sheet.Range["F26"].CellStyle.Font.Size = 17;
            sheet.Range["F28"].Text = 생산지시.시작일?.ToString("yyyy/MM/dd");
            sheet.Range["F28"].CellStyle.Font.Size = 17;
            sheet.Range["F30"].Text = 생산지시.완료목표일?.ToString("yyyy/MM/dd");
            sheet.Range["F30"].CellStyle.Font.Size = 17;
            sheet.Range["F32"].Text = 생산지시.생산수량.ToString();
            sheet.Range["F32"].CellStyle.Font.Size = 17;
            sheet.Range["F34"].Text = 공정차수.작업자?.사용자명;
            sheet.Range["F34"].CellStyle.Font.Size = 17;
            sheet.Range["F36"].Text = 공정차수.생산품공정차수.공정단위.도면?.도면번호;
            sheet.Range["F36"].CellStyle.Font.Size = 17;
            sheet.Range["F38"].Text = 공정차수.생산품공정차수.공정단위.도면?.도면명;
            sheet.Range["F38"].CellStyle.Font.Size = 17;
            sheet.Range["F40"].Text = 공정차수.비고;
            sheet.Range["F40"].CellStyle.Font.Size = 17;

            if (gong_barcodeStart != null)
            {
                sheet.Range[8, 1].CellStyle.Font.Bold = true;
                sheet.Range[8, 1].CellStyle.Font.Size = 15;
                sheet[8, 1].Text = "공정 시작";
                var ms = new MemoryStream();
                MakeQrCode(gong_barcodeStart).Save(ms, ImageFormat.Png);
                sheet.Pictures.AddPicture(6, 2, ms, 30, 30);
                ms.Dispose();
            }
            if (gong_barcodeStart != null)
            {
                sheet.Range[50, 1].CellStyle.Font.Bold = true;
                sheet.Range[50, 1].CellStyle.Font.Size = 15;
                sheet[50, 1].Text = "공정 종료";
                var ms = new MemoryStream();
                MakeQrCode(gong_barcodeEnd).Save(ms, ImageFormat.Png);
                sheet.Pictures.AddPicture(48, 2, ms, 30, 30);
                ms.Dispose();
            }
            //  공정차수.생산품공정차수.공정단위.공정설비목록 NULL 이면 다시처리
            if (공정차수.생산품공정차수.공정단위.공정.설비사용유무 == true && 공정차수.생산품공정차수.공정단위.공정설비목록.Count > 0)
            {
                sheet.Range[8, 5].CellStyle.Font.Bold = true;
                sheet.Range[8, 5].CellStyle.Font.Size = 15;
                sheet[8, 5].Text = "설비 코드";
                var ms = new MemoryStream();
                MakeQrCode(공정차수.생산품공정차수.공정단위.공정설비목록[0].설비코드).Save(ms, ImageFormat.Png);
                sheet.Pictures.AddPicture(6, 7, ms, 35, 35);
                ms.Dispose();
            }

            //if (실적등록바코드 != null)
            //{
            //    sheet.Range[50, 5].CellStyle.Font.Bold = true;
            //    sheet.Range[50, 5].CellStyle.Font.Size = 15;
            //    sheet[50, 5].Text = "실적 등록";
            //    var ms = new MemoryStream();
            //    MakeQrCode(gong_barcodeEnd).Save(ms, ImageFormat.Png);
            //    sheet.Pictures.AddPicture(48, 7, ms, 30, 30);
            //    ms.Dispose();
            //}

            //IListObject table1 = sheet.ListObjects.Create("Table1", sheet["A17:J40"]);
            //table1.BuiltInTableStyle = TableBuiltInStyles.TableStyleLight1;

            sheet.Range["A18:J19"].BorderAround(ExcelLineStyle.Medium, Syncfusion.Drawing.Color.LightGray);
            sheet.Range["A20:J21"].BorderAround(ExcelLineStyle.Medium, Syncfusion.Drawing.Color.LightGray);
            sheet.Range["A22:J23"].BorderAround(ExcelLineStyle.Medium, Syncfusion.Drawing.Color.LightGray);
            sheet.Range["A24:J25"].BorderAround(ExcelLineStyle.Medium, Syncfusion.Drawing.Color.LightGray);
            sheet.Range["A26:J27"].BorderAround(ExcelLineStyle.Medium, Syncfusion.Drawing.Color.LightGray);
            sheet.Range["A28:J29"].BorderAround(ExcelLineStyle.Medium, Syncfusion.Drawing.Color.LightGray);
            sheet.Range["A30:J31"].BorderAround(ExcelLineStyle.Medium, Syncfusion.Drawing.Color.LightGray);
            sheet.Range["A32:J33"].BorderAround(ExcelLineStyle.Medium, Syncfusion.Drawing.Color.LightGray);
            sheet.Range["A34:J35"].BorderAround(ExcelLineStyle.Medium, Syncfusion.Drawing.Color.LightGray);
            sheet.Range["A36:J37"].BorderAround(ExcelLineStyle.Medium, Syncfusion.Drawing.Color.LightGray);
            sheet.Range["A38:J39"].BorderAround(ExcelLineStyle.Medium, Syncfusion.Drawing.Color.LightGray);

            var ws = new MemoryStream();
            workbook.SaveAs(ws);
            ws.Seek(0, SeekOrigin.Begin);

            return ws;
        }

        private Bitmap MakeQrCode(string text)
        {
            var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }


        public ObservableCollection<품질검사생산품> 품질검사생산품목록 { get; set; }

        public List<품질검사생산품> list3 = new List<품질검사생산품>();

        private 품질검사생산품 selected품질검사생산품;

        private SfGrid<품질검사생산품> grid3;


        public SfButton btnTest, btnCheck, btnSave;

        public class 품질검사생산품 : INotifyPropertyChanged
        {
            public int seq { get; set; }
            public string 생산지시코드 { get; set; }
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




        #region 실적등록
        public class 재작업필드
        {
            public int ID { get; set; }
            public string Text { get; set; }
        }
        public List<재작업필드> list재작업여부 = new List<재작업필드>() {
        new 재작업필드(){ ID=0, Text="정상작업"},
        new 재작업필드(){ ID=1, Text="재작업"},
    };

        public SfButton 실적등록;
        private string 실적등록바코드 = "";
        private string 공정실적등록바코드 = "";

        public Query Query;
        public Query Query2;
        public Query Query3;

        public List<직원정보> 직원정보List { get; set; }
        public List<부서정보> 부서정보List { get; set; }

        public List<장소정보> 실적공정코드_창고코드List { get; set; }
        public List<장소위치정보> 실적작업장코드_장소코드List { get; set; }

        public List<장소정보> 실적공정코드List { get; set; }
        public List<장소위치정보> 실적작업장코드List { get; set; }

        public string Edit사원코드 = "";
        public string Edit부서코드 = "";
        public string Edit실적공정코드_창고코드 = "";
        public string Edit실적작업장코드_장소코드 = "";
        public string Edit재작업필드 = "";
        public decimal Edit실적수량 = 0;
        public decimal Edit사용수량 = 0;
        public decimal Edit불량수량 = 0;
        public decimal Edit목표수량 = 0;
        public decimal Edit검사수량 = 0;
        public string Edit비고 = "";

        public string Validate = "e-multi-column";
        private async Task 공정실적등록클릭()
        { }

        private bool isDialogVisible;

        private string barcodeValue;

        private string barcodeValueLOT = null;


        private DateTime 실적등록일selected { get; set; } = DateTime.Now;

        public void ValueChangeHandler(Syncfusion.Blazor.Calendars.ChangedEventArgs<DateTime?> args)
        {
            // Here you can customize your code
            실적등록일selected = new(args.Value.Value.Year, args.Value.Value.Month, args.Value.Value.Day);
        }


        public string Edit실적공정코드 = "";
        public string Edit실적작업장코드 = "";

        private async Task 실적등록클릭()
        {
            await Task.Delay(500);
            생산지시 = await Remote.Command.생산관리.작업지시상세_조회(생산지시.생산지시코드, 생산지시.회사코드);

            isDialogVisible = true;

            var result = await this.Remote.Command.기준정보.직원_조회(true, 회사코드);
            직원정보List = result.ToList();
            await Task.Delay(100);
            var result2 = await Remote.Command.기준정보.부서정보_조회(회사코드);
            부서정보List = result2.ToList();
            await Task.Delay(100);
            var result3 = await Remote.Command.기준정보.장소_조회(회사코드);
            실적공정코드_창고코드List = result3.ToList();
            await Task.Delay(100);
            var result4 = await Remote.Command.기준정보.장소위치2_조회(회사코드, "1000");
            실적작업장코드_장소코드List = result4.ToList();


            var 품목 = await Remote.Command.기준정보.품목구분_조회(생산지시.생산계획.생산품코드);

            if (품목.품목구분코드 == "B1201" || 품목.품목구분코드 == "B1202")
            {
                Edit실적작업장코드_장소코드 = "1000";
            }
            else if (품목.품목구분코드 == "B1203")
            {
                Edit실적작업장코드_장소코드 = "1001";
            }
            else if (품목.품목구분코드 == "B1204")
            {
                Edit실적작업장코드_장소코드 = "1002";
            }

            Edit실적공정코드_창고코드 = "1000";
            Query = new Query().Select(new List<string> { "장소코드" }).Where("장소코드", "equal", Edit실적공정코드_창고코드);
            //Query2 = new Query().Select(new List<string> { "위치코드" }).Where("위치코드", "equal", Edit실적작업장코드_장소코드);


            var result5 = await Remote.Command.기준정보.장소_조회(회사코드);
            실적공정코드List = result5.ToList();

            var result6 = await Remote.Command.기준정보.장소위치2_조회(회사코드, "1000");
            실적작업장코드List = result6.ToList();
            Query3 = new Query().Select(new List<string> { "장소코드" }).Where("장소코드", "equal", "1000");


            await Task.Delay(100);
            //var 공정단위자재목록 = await Remote.Command.기준정보.작업지시공정단위자재현황_조회(회사코드, 공정단위코드);
            //var 공정단위자재목록 = await Remote.Command.기준정보.공정단위자재현황_조회(회사코드, 공정단위코드);

            listBom?.Clear();

            Edit재작업필드 = 생산지시.재작업여부;

            Edit목표수량 = 생산지시.생산수량;
            Edit불량수량 = 생산지시.불량수량;
            Edit실적수량 = 생산지시.실생산량 - 생산지시.불량수량;
            Edit검사수량 = 생산지시.검사수량;

            listBom = Enumerable.Range(0, 생산지시.생산지시공정차수목록.Count - 1).Select(x => new 공정단위자재현황()
            {
                공정단위코드 = 생산지시.생산지시공정차수목록[x].생산품공정차수.공정단위코드,
                자재코드 = 생산지시.생산지시공정차수목록[x].생산품공정차수.공정단위.공정품코드,
                필요수량 = 1,
                사용수량 = Edit실적수량,
                실적수량 = Edit실적수량,
                불량수량 = 생산지시.불량수량,
            }).ToList();
            listBomOB = new ObservableCollection<공정단위자재현황>(listBom);

        }

        private async Task 실적등록저장()
        {

            isDialogVisible = true;

            if (Edit사원코드 == "" || Edit부서코드 == "" || Edit실적공정코드_창고코드 == "" || Edit실적작업장코드_장소코드 == "" || Edit재작업필드 == "")
            {
                this.Validate = "e-error e-multi-column";
                StateHasChanged();
                return;

            }

            string lot = await OnQRPrinte(4, Edit실적수량, 생산품코드);

            var 생산실적헤더정보 = new 생산실적헤더정보
            {
                회사코드 = 회사코드,
                생산지시코드 = 생산지시.생산지시코드,
                공정단위코드 = 공정단위코드,
                생산품코드 = 생산품코드,
                생산품공정코드 = 생산지시.생산계획.생산품공정.생산품공정코드,
                사업장코드 = "1000",
                실적공정코드_창고코드 = Edit실적공정코드_창고코드,
                실적작업장코드_장소코드 = Edit실적작업장코드_장소코드,
                재작업여부 = Edit재작업필드,
                생산지시명 = 생산지시.생산지시명,
                실적수량 = Edit실적수량,
                불량수량 = Edit불량수량,
                LOTNO = lot,

                주문번호 = 생산지시.생산계획.주문번호,
                실적구분 = "0",
                실적공정코드 = Edit실적공정코드,
                실적작업장코드 = Edit실적작업장코드


            };
            var 생산실적상세정보 = new 생산실적상세정보
            {
                회사코드 = 회사코드,
                생산지시코드 = 생산지시.생산지시코드,
                작업자사번 = Edit사원코드,
                부서코드 = Edit부서코드,
                //사용수량 = Edit사용수량,
                실적등록일 = 실적등록일selected,
                비고 = Edit비고,
                LOT번호 = lot,
                주문번호 = 생산지시.생산계획.주문번호,

                사용공정_사용창고 = "2000",
                사용작업장_사용장소 = "2001",

            };


            try
            {
                bool result1 = await Remote.Command.생산관리.품질검사_작업생산실적_저장(생산실적헤더정보, 생산실적상세정보, listBom);
                Task.Delay(200);
                bool result2 = await Remote.Command.생산관리.품질검사_생산제품입고처리_등록(생산실적헤더정보, 생산실적상세정보, listBom);
                Task.Delay(200);
                bool result3 = await Remote.Command.생산관리.품질검사_생산제품_위치등록(생산실적헤더정보, 생산실적상세정보, listBom);
                Task.Delay(200);
                bool result4 = await Remote.Command.생산관리.품질검사_더존_생산실적헤더정보_등록(생산실적헤더정보, 생산실적상세정보, listBom);
                Task.Delay(200);
                bool result5 = await Remote.Command.생산관리.품질검사_더존_불량실적헤더정보_등록(생산실적헤더정보, 생산실적상세정보, listBom);


                if (!result1 || !result2 || !result3 || !result4 || !result5)
                {
                    NotifyMessage(Message.반영실패);
                    isDialogVisible = false;
                    return;
                }
            }
            catch (Exception ex)
            {
            }

            NotifyMessage(Message.반영성공);


            Edit사원코드 = "";
            Edit부서코드 = "";
            Edit실적공정코드_창고코드 = "";
            Edit실적작업장코드_장소코드 = "";
            Edit재작업필드 = "";
            Edit실적수량 = 0;
            Edit사용수량 = 0;
            Edit불량수량 = 0;
            Edit목표수량 = 0;
            Edit검사수량 = 0;
            Edit실적공정코드 = "";
            Edit실적작업장코드 = "";
            Edit비고 = "";
            실적등록일selected = DateTime.Now;

            isDialogVisible = false;

            this.Validate = "e-multi-column";

        }

        public async Task 품질검사수량Changed(Syncfusion.Blazor.Inputs.ChangeEventArgs<Decimal> args)
        {

            var gridbom = await GridBom.GetCurrentViewRecords();

            listBom = Enumerable.Range(0, gridbom.Count).Select(x => new 공정단위자재현황()
            {
                공정단위코드 = gridbom[x].공정단위코드,
                자재코드 = gridbom[x].자재코드,
                필요수량 = gridbom[x].필요수량,
                사용수량 = Edit실적수량,
                불량수량 = Edit불량수량,
            }).ToList();
            listBomOB = new ObservableCollection<공정단위자재현황>(listBom);


        }

        #endregion


        #region 공정단위실적등록

        private bool 공정단위실적다이어로그 { get; set; } = false;
        public string Edit공정단위생산품공정코드 = "";
        public bool 공정버튼활성여부 { get; set; } = false;
        public bool 실적등록버튼활성유무 { get; set; } = false;


        public async Task 공정실적불량Changed(Syncfusion.Blazor.Inputs.ChangeEventArgs<Decimal> args)
        {

            var gridbom = await GridBom.GetCurrentViewRecords();

            listBom = Enumerable.Range(0, gridbom.Count).Select(x => new 공정단위자재현황()
            {
                공정단위코드 = gridbom[x].공정단위코드,
                자재코드 = gridbom[x].자재코드,
                필요수량 = gridbom[x].필요수량,
                사용수량 = Edit실적수량 * gridbom[x].필요수량,
                불량수량 = Edit불량수량 * gridbom[x].필요수량,
            }).ToList();
            listBomOB = new ObservableCollection<공정단위자재현황>(listBom);


        }

        private async Task 공정단위실적클릭(생산지시공정차수정보 info)
        {

            await Task.Delay(600);
            생산지시 = await Remote.Command.생산관리.작업지시상세_조회(생산지시.생산지시코드, 생산지시.회사코드);

            공정단위실적다이어로그 = true;

            var result = await this.Remote.Command.기준정보.직원_조회(true, 회사코드);
            직원정보List = result.ToList();
            await Task.Delay(100);
            var result2 = await Remote.Command.기준정보.부서정보_조회(회사코드);
            부서정보List = result2.ToList();
            await Task.Delay(100);
            var result3 = await Remote.Command.기준정보.장소_조회(회사코드);
            실적공정코드_창고코드List = result3.ToList();
            await Task.Delay(100);
            var result4 = await Remote.Command.기준정보.장소위치2_조회(회사코드, "1000");
            실적작업장코드_장소코드List = result4.ToList();

            var 품목 = await Remote.Command.기준정보.품목구분_조회(생산지시.생산계획.생산품코드);

            if (품목.품목구분코드 == "B1201" || 품목.품목구분코드 == "B1202" || 품목.품목구분코드 == "B1203")
            {
                Edit실적작업장코드_장소코드 = "1000";
            }
            else if (품목.품목구분코드 == "B1204")
            {
                Edit실적작업장코드_장소코드 = "1002";
            }

            Edit실적공정코드_창고코드 = "1000";
            Query = new Query().Select(new List<string> { "장소코드" }).Where("장소코드", "equal", Edit실적공정코드_창고코드);
            //Query2 = new Query().Select(new List<string> { "위치코드" }).Where("위치코드", "equal", Edit실적작업장코드_장소코드);


            var result5 = await Remote.Command.기준정보.장소_조회(회사코드);
            실적공정코드List = result5.ToList();

            var result6 = await Remote.Command.기준정보.장소위치2_조회(회사코드, "1000");
            실적작업장코드List = result6.ToList();
            Query3 = new Query().Select(new List<string> { "장소코드" }).Where("장소코드", "equal", "1000");


            await Task.Delay(100);
            var 공정단위자재목록 = await Remote.Command.기준정보.작업지시공정단위자재현황_조회(회사코드, 공정단위코드);
            listBom?.Clear();

            //var 공정단위자재목록 = await Remote.Command.기준정보.공정단위자재현황_조회(회사코드, 공정단위코드);
            var result7 = await Remote.Command.기준정보.작업지시상세_생산실적헤더정보_조회(회사코드, 생산지시.생산지시코드);

            var result8 = await Remote.Command.생산관리.작업지시공정완료이력현황_조회(회사코드, 생산지시.생산지시코드, 공정단위코드);

            if (result8 != null)
            {
                Edit목표수량 = result8.목표수량;
                Edit불량수량 = result8.불량수량;
                Edit실적수량 = result8.생산수량;

                listBom = Enumerable.Range(0, 공정단위자재목록.Count).Select(x => new 공정단위자재현황()
                {
                    공정단위코드 = 공정단위자재목록[x].공정단위코드,
                    자재코드 = 공정단위자재목록[x].자재코드,
                    필요수량 = 공정단위자재목록[x].필요수량,
                    사용수량 = Edit실적수량 * 공정단위자재목록[x].필요수량,
                    실적수량 = (Edit실적수량 - Edit불량수량) * 공정단위자재목록[x].필요수량,
                    불량수량 = result8.불량수량 * 공정단위자재목록[x].필요수량,
                }).ToList();
            }
            else
            {
                Edit목표수량 = 생산지시.생산수량;
                Edit불량수량 = 생산지시.불량수량;
                Edit실적수량 = 생산지시.생산수량 - 생산지시.불량수량;

                listBom = Enumerable.Range(0, 공정단위자재목록.Count).Select(x => new 공정단위자재현황()
                {
                    공정단위코드 = 공정단위자재목록[x].공정단위코드,
                    자재코드 = 공정단위자재목록[x].자재코드,
                    필요수량 = 공정단위자재목록[x].필요수량,
                    사용수량 = (생산지시.생산수량) * 공정단위자재목록[x].필요수량,
                    실적수량 = (생산지시.생산수량 - 생산지시.불량수량) * 공정단위자재목록[x].필요수량,
                    불량수량 = 생산지시.불량수량 * 공정단위자재목록[x].필요수량,
                }).ToList();
            }
            Edit재작업필드 = 생산지시.재작업여부;

            Edit공정단위생산품공정코드 = info.생산품공정차수.공정단위.공정품코드;

            listBomOB = new ObservableCollection<공정단위자재현황>(listBom);
        }

        private async Task 공정단위실적저장()
        {


            if (Edit사원코드 == "" || Edit부서코드 == "" || Edit실적공정코드_창고코드 == "" || Edit실적작업장코드_장소코드 == "" || Edit재작업필드 == "")
            {
                this.Validate = "e-error e-multi-column";
                StateHasChanged();
                return;
            }

            string lot = await OnQRPrinte(4, Edit실적수량, Edit공정단위생산품공정코드);

            var 생산실적헤더정보 = new 생산실적헤더정보
            {
                회사코드 = 회사코드,
                생산지시코드 = 생산지시.생산지시코드,
                공정단위코드 = 공정단위코드,
                생산품코드 = Edit공정단위생산품공정코드,
                생산품공정코드 = 생산지시.생산계획.생산품공정.생산품공정코드,
                사업장코드 = "1000",
                실적공정코드_창고코드 = Edit실적공정코드_창고코드,
                실적작업장코드_장소코드 = Edit실적작업장코드_장소코드,
                재작업여부 = Edit재작업필드,
                생산지시명 = 생산지시.생산지시명,
                실적수량 = Edit실적수량,
                불량수량 = Edit불량수량,
                LOTNO = lot,

                주문번호 = 생산지시.생산계획.주문번호,
                실적구분 = "0",
                실적공정코드 = Edit실적공정코드,
                실적작업장코드 = Edit실적작업장코드


            };
            var 생산실적상세정보 = new 생산실적상세정보
            {
                회사코드 = 회사코드,
                생산지시코드 = 생산지시.생산지시코드,
                작업자사번 = Edit사원코드,
                부서코드 = Edit부서코드,
                //사용수량 = Edit사용수량,
                실적등록일 = 실적등록일selected,
                비고 = Edit비고,
                LOT번호 = lot,
                주문번호 = 생산지시.생산계획.주문번호,

                사용공정_사용창고 = "2000",
                사용작업장_사용장소 = "2001",

            };

            try
            {
                //bool results = await Remote.Command.생산관리.공정단위실적처리_저장(생산실적헤더정보, 생산실적상세정보, listBom);


                //공정실적 처리

                bool result1 = await Remote.Command.생산관리.작업생산실적_저장(생산실적헤더정보, 생산실적상세정보, listBom);
                Task.Delay(200);
                bool result2 = await Remote.Command.생산관리.생산제품입고처리_등록(생산실적헤더정보, 생산실적상세정보, listBom);
                Task.Delay(200);
                bool result3 = await Remote.Command.생산관리.생산제품_위치등록(생산실적헤더정보, 생산실적상세정보, listBom);
                Task.Delay(200);
                bool result4 = false;
                bool result5 = false;
                if (listBom.Count == 1)
                {
                    result4 = await Remote.Command.생산관리.더존_생산실적헤더정보_등록(생산실적헤더정보, 생산실적상세정보, listBom);
                    Task.Delay(200);
                    result5 = await Remote.Command.생산관리.더존_불량실적헤더정보_등록(생산실적헤더정보, 생산실적상세정보, listBom);
                }
                else
                {
                    result4 = await Remote.Command.생산관리.더존멀티_생산실적헤더정보_등록(생산실적헤더정보, 생산실적상세정보, listBom);
                    Task.Delay(200);
                    result5 = await Remote.Command.생산관리.더존멀티_불량실적헤더정보_등록(생산실적헤더정보, 생산실적상세정보, listBom);
                }



                if (!result1 || !result2 || !result3 || !result4 || !result5)
                {
                    NotifyMessage(Message.반영실패);
                    isDialogVisible = false;
                    return;
                }



            }
            catch (Exception ex)
            {
            }

            NotifyMessage(Message.반영성공);

            Edit사원코드 = "";
            Edit부서코드 = "";
            Edit실적공정코드_창고코드 = "";
            Edit실적작업장코드_장소코드 = "";
            Edit재작업필드 = "";
            Edit실적수량 = 0;
            Edit사용수량 = 0;
            Edit불량수량 = 0;
            Edit목표수량 = 0;
            Edit검사수량 = 0;
            Edit실적공정코드 = "";
            Edit실적작업장코드 = "";
            Edit비고 = "";
            실적등록일selected = DateTime.Now;

            공정단위실적다이어로그 = false;

            this.Validate = "e-multi-column";
        }

        #endregion


        private async Task<string> OnQRPrinte(int type, decimal _수량, string 품목코드)
        {
            string lotNo = "";
            var sawon = await SessionStorage.GetAsync<string>("userId");

            //var YesOrNo = await ShowMessageBox("LOT번호발급", "LOT번호를 발급하시겠습니까?", MessageBoxResultType.YesOrNo);

            bool YesOrNo = true;

            var 수량 = Convert.ToInt32(Convert.ToInt32(_수량)).ToString();

            if (YesOrNo)
            {
                lotNo = await Remote.Command.자재관리.품목코드_바코드발급(품목코드, 회사코드, 수량, sawon, YesOrNo, "2");
            }

            barcodeValueLOT = barcodeValue + ':' + lotNo;

            //StateHasChanged();

            //await Task.Delay(1000);

            //await QRPrinte_바코드Act(barcodeValueLOT, 수량, 2);
            //NotifyMessage(Message.반영성공);

            return lotNo;

        }


    }
    public static class JSInteropExt
    {
        public static async Task SaveAsFileAsync(this IJSRuntime js, string filename, byte[] data, string type = "application/octet-stream;")
        {
            await js.InvokeAsync<object>("JSInteropExt.saveAsFile", filename, type, Convert.ToBase64String(data));
        }
    }




}
