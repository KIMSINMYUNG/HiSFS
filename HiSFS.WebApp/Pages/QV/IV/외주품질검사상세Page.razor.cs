using HiSFS.Api.Shared.Models;
using HiSFS.Api.Shared.Models.View;
using HiSFS.WebApp.Component.Common;
using HiSFS.WebApp.Services;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
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
    public partial class 외주품질검사상세Page
    {
        private FGrid<외주지시별품질검사장비> grid5;
        private ObservableCollection<외주지시별품질검사장비> list5 = new ObservableCollection<외주지시별품질검사장비>();

        //private TGrid<공정단위검사정보> grid4;
        //private ObservableCollection<공정단위검사정보> list4 = new ObservableCollection<공정단위검사정보>();
        //private 공정단위검사정보 selected공정단위검사정보;

        private HGrid<외주지시별검사정보> grid4;
        private ObservableCollection<외주지시별검사정보> list4 = new ObservableCollection<외주지시별검사정보>();
        private 외주지시별검사정보 selected외주지시별검사정보;

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
                await Remote.Command.공통.외주포인트_저장2(userId, 외주작업지시.품번, Convert.ToInt32(외주작업지시.수량), 검사항목수.Count);

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
                bool nowPoint = await Remote.Command.공통.외주포인트_삭제(userId, 외주작업지시.품번);

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
        private async Task OnConnection(외주지시별품질검사장비 info, bool isConn, int BtnSid)
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
                if (Convert.ToInt32(검사수량입력) > 외주작업지시.수량)
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
                    생산지시코드 = 외주작업지시.지시번호,
                    생산품공정코드 = 외주작업지시.품번,
                    생산품공정명 = 외주작업지시.공정명,
                    공정단위코드 = 외주작업지시.공정단위코드,
                    Result = "",
                    보유품목코드 = 외주작업지시.품번,
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


                int nowPoint = await Remote.Command.공통.외주포인트_조회(userId, 외주작업지시.품번, 외주작업지시.지시번호, 외주작업지시.회사코드);

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
                        외주품질검사측정정보 측정정보 = new 외주품질검사측정정보();
                        측정정보.시리얼넘버 = 품목[nowItem].Seq;
                        측정정보.지시번호 = 외주작업지시.지시번호;
                        측정정보.공정단위코드 = null;
                        측정정보.품질검사코드 = 측정.품질검사코드;                   //"Q000001";
                        측정정보.검사단위코드 = 측정.검사단위코드;                   //"B2401";
                        측정정보.생산품공정코드 = 외주작업지시.품번;         //"KMFA-518986:1";
                        측정정보.생산품공정명 = 품목[nowItem].생산품공정명;     // "테스트 생산품공정"
                        측정정보.검사기준값 = 측정.검사기준값;
                        측정정보.오차범위 = 측정.오차범위;
                        측정정보.검사측정값 = 측정.검사측정값;
                        측정정보.합격여부 = 측정.합격여부;
                        측정정보.보유품목코드 = 외주작업지시.품번;
                        측정정보.회사코드 = 외주작업지시.회사코드;

                        var result = await Remote.Command.품질관리.외주품질검사측정정보유무_조회(품목[nowItem].Seq, 측정정보);

                        if (result != null)
                            측정정보.RowVersion = result.RowVersion;


                        if (측정.검사측정값 != null)
                        {
                            bool multiChk = await Remote.Command.품질관리.외주품질검사측정정보_저장(측정정보, result == null ? true : false);
                            if (!multiChk)
                                throw new Exception("이미처리되었습니다.");
                            //일련번호생성
                            //await Remote.Command.품질관리.품질검사측정_보유품목일련정보_저장(생산품코드, 품목[nowItem].Seq);

                            //var 검사카운트 = 품질검사목록.GroupBy(x => x.시리얼넘버 == 품목[nowItem].Seq).FirstOrDefault().Count();
                            //if (품질검사목록.Count() == (검사항목전체.Count() * Convert.ToInt32(생산지시.생산수량)))

                            var 품질검사목록 = await Remote.Command.품질관리.외주품질검사측정완료유무_조회(외주작업지시.지시번호, 회사코드);
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
                        await Remote.Command.품질관리.외주품질검사측정_외주지시측정수량_저장(품목[nowItem].생산지시코드, strResult2 , 외주작업지시.회사코드);
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

                        await Remote.Command.공통.외주포인트_설정(userId, 외주작업지시.품번, -1, 0);
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
            var result = await Remote.Command.품질관리.외주품질검사측정정보_조회(외주작업지시.지시번호 , selected품질검사생산품.Seq);
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

        public void QueryCellInfoHandler(QueryCellInfoEventArgs<외주지시별검사정보> Args)
        {
            if (Args.Column.Type == ColumnType.CheckBox && (Args.Data.합격여부 == "합격" || Args.Data.합격여부 == "불합격"))
            {
                Args.Cell.AddClass(new string[] { "e-checkbox-disabled" });
            }

        }

        #region 실적등록


        private DateTime 실적등록일selected { get; set; } = DateTime.Now;

        private DateTime 이동일자selected { get; set; } = DateTime.Now;

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
        private string 실적등록바코드;

        private int 입고창고Index = 0;
        private int 입고장소Index = 0;

        public Query Query;
        public Query Query2;
        public List<직원정보> 직원정보List { get; set; }
        public List<부서정보> 부서정보List { get; set; }

        public List<장소정보> 이동공정코드_이동창고코드List { get; set; }
        public List<장소위치정보> 이동공정코드_이동장소코드List { get; set; }

        public string Edit사원코드 = "";
        public string Edit부서코드 = "";
        public string Edit이동공정_입고창고코드 = "";
        public string Edit이동작업장_입고장소코드 = "";
        public string Edit재작업필드 = "";
        public decimal Edit실적수량 = 0;
        public decimal Edit사용수량 = 0;
        public decimal Edit불량수량 = 0;
        public decimal Edit검사수량 = 0;

        public string Edit반입입고장소코드 = "";
        public string Edit비고 = "";

        private string Edit_공정단위코드;
        private string Edit_지시번호;

        public string Validate = "e-multi-column";

        private bool isDialogVisible;

        public void ValueChangeHandler(Syncfusion.Blazor.Calendars.ChangedEventArgs<DateTime?> args)
        {
            // Here you can customize your code
            실적등록일selected = new(args.Value.Value.Year, args.Value.Value.Month, args.Value.Value.Day);
        }
        private async Task 실적등록클릭()
        {
            //if (selectedRow == null)
            //    return;
            await Task.Delay(500);

            외주작업지시 = await Remote.Command.기준정보.외주작업지시서번호정보_조회Dz(회사코드, Code);

            isDialogVisible = true;

            var result = await this.Remote.Command.기준정보.직원_조회(true, 회사코드);
            직원정보List = result.ToList();

            var result2 = await Remote.Command.기준정보.부서정보_조회(회사코드);
            부서정보List = result2.ToList();

            var 품목 = await Remote.Command.기준정보.품목구분_조회(외주작업지시.품번);

            // ACCT_FG 0.원재료 1.부재료 2.제품 4.반제품 5.상품 6.저장품 7.비용 8.수익

            var result3 = await Remote.Command.기준정보.장소_조회(회사코드);
            이동공정코드_이동창고코드List = result3.ToList();

            var result4 = await Remote.Command.기준정보.장소위치2_조회(회사코드, "1000");
            이동공정코드_이동장소코드List = result4.ToList();

            if (품목.품목구분코드 == "B1201" || 품목.품목구분코드 == "B1202" || 품목.품목구분코드 == "B1203")
            {
                Edit이동작업장_입고장소코드 = "1000";
            }
            else if (품목.품목구분코드 == "B1204")
            {
                Edit이동작업장_입고장소코드 = "1002";
            }


            Edit이동공정_입고창고코드 = "1000";
            Query = new Query().Select(new List<string> { "장소코드" }).Where("장소코드", "equal", Edit이동공정_입고창고코드);
            //Query2 = new Query().Select(new List<string> { "위치코드" }).Where("위치코드", "equal", Edit이동작업장_입고장소코드);

           

            Edit_지시번호 = 외주작업지시.지시번호;

            Edit불량수량 = 외주작업지시.불량수량;
            Edit실적수량 = 외주작업지시.수량 - 외주작업지시.불량수량;
            Edit검사수량 = 외주작업지시.검사수량;
            listBom?.Clear();

            var BOMALL = await Remote.Command.기준정보.NEW공정단위BOM_정보_조회(회사코드);

            var BOM_Single = BOMALL.Where(x => x.모품번 == 외주작업지시.품번).FirstOrDefault();

            var BOM_Multi = BOMALL.Where(x => x.모품번 == 외주작업지시.품번).ToList();

            var 더존비오엠 = BOM_Single;

            if (BOM_Multi.Count > 1)
            {
                listBom = Enumerable.Range(0, BOM_Multi.Count).Select(x => new 공정단위자재현황()
                {
                    공정단위코드 = null,
                    자재코드 = BOM_Multi[x].자품번,
                    필요수량 = BOM_Multi[x].필요수량,
                    사용수량 = (외주작업지시.수량 - 외주작업지시.불량수량) * BOM_Multi[x].필요수량,
                    실적수량 = (외주작업지시.수량 - 외주작업지시.불량수량) * BOM_Multi[x].필요수량,
                    불량수량 = 외주작업지시.불량수량 * BOM_Multi[x].필요수량,
                }).ToList();

                listBomOB = new ObservableCollection<공정단위자재현황>(listBom);
            }
            else
            {
                if (BOM_Single != null)
                {
                    var CharCheck = BOM_Single.자품번.EndsWith("I");
                    if (CharCheck)
                    {
                        더존비오엠 = BOMALL.Where(x => x.모품번 == BOM_Single.자품번).FirstOrDefault();
                    }
                    else
                    {
                        더존비오엠 = BOM_Single;
                    }
                   

                    listBom = Enumerable.Range(0, 1).Select(x => new 공정단위자재현황()
                    {
                        공정단위코드 = null,
                        자재코드 = 더존비오엠.자품번,
                        필요수량 = 더존비오엠.필요수량,
                        사용수량 = 외주작업지시.수량 * 더존비오엠.필요수량,
                        실적수량 = (외주작업지시.수량 - 외주작업지시.불량수량) * 더존비오엠.필요수량,
                        불량수량 = 외주작업지시.불량수량 * 더존비오엠.필요수량,
                    }).ToList();

                    listBomOB = new ObservableCollection<공정단위자재현황>(listBom);
                }
            }

           
            //var 외주자재리스트 = await Remote.Command.기준정보.외주생산위치정보_조회(외주작업지시.지시번호);

           
        }

        private async Task 실적등록저장()
        {
            string lotNo = await OnQRPrinte(4, Edit실적수량, 외주작업지시.품번);
            await Task.Delay(1000);
            isDialogVisible = true;

            if (Edit사원코드 == "" || Edit부서코드 == "" || Edit이동공정_입고창고코드 == "" || Edit이동작업장_입고장소코드 == "" || Edit재작업필드 == "")
            {
                this.Validate = "e-error e-multi-column";
                StateHasChanged();
                return;

            }

            var 작업외주생산실적 = new 작업외주생산실적등록정보
            {
                회사코드 = 외주작업지시.회사코드,
                부서코드 = Edit부서코드,
                사원코드 = Edit사원코드,
                사업장코드 = "1000",
                작업일자 = DateTime.Now,
                실적일자 = 실적등록일selected,

                지시번호 = 외주작업지시.지시번호,
                지시전개순번 = 외주작업지시.전개순번,
                실적수량 = Edit실적수량,
                처리구분 = "1", //입고
                이동공정_입고창고코드 = Edit이동공정_입고창고코드, //입고시 처리
                이동작업장_입고장소코드 = Edit이동작업장_입고장소코드, //입고시처리
                검사구분 = 외주작업지시.검사구분,
                재작업여부 = Edit재작업필드,
                실적구분 = "0",
                설비코드 = 외주작업지시.설비코드,
                실적공정코드 = 외주작업지시.공정,
                실적작업장코드 = 외주작업지시.작업장,
                비고 = 외주작업지시.비고_DOC_DC,
                LOT번호 = lotNo,


            };

            try
            {
                var result1 = await Remote.Command.기준정보.외주제품입고처리_등록(작업외주생산실적, 외주작업지시);
                Task.Delay(200);
                var result2 = await Remote.Command.기준정보.외주제품_위치등록(작업외주생산실적, 외주작업지시, Edit불량수량);
                Task.Delay(200);
                bool result3 = await Remote.Command.기준정보.MES외주생산실적_작업외주생산실적등록정보_등록(작업외주생산실적, true);
                Task.Delay(200);
                bool result4 = false;

                if(listBomOB.Count > 1)
                {
                    result4 = await Remote.Command.기준정보.MES외주생산실적_멀티외주생산실적소유자재털기_등록(작업외주생산실적, 외주작업지시, listBom);
                }
                else
                {
                    result4 = await Remote.Command.기준정보.MES외주생산실적_외주생산실적소유자재털기_등록(작업외주생산실적, 외주작업지시, listBom);
                }


                //bool result5 = await Remote.Command.기준정보.외주생산실적소유자재털기_등록(작업외주생산실적, 외주작업지시, listBom);
                if (!result1 || !result2 || !result3 || !result4)
                {
                    NotifyMessage(Message.반영실패);
                    isDialogVisible = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                NotifyMessage(Message.반영실패);
                isDialogVisible = false;
                return;
            }

            NotifyMessage(Message.반영성공);

            Edit사원코드 = "";
            Edit부서코드 = "";
            Edit이동공정_입고창고코드 = "";
            Edit이동작업장_입고장소코드 = "";
            Edit재작업필드 = "";
            Edit실적수량 = 0;
            Edit사용수량 = 0;
            Edit불량수량 = 0;
            Edit비고 = "";
            실적등록일selected = DateTime.Now;

            isDialogVisible = false;

            반입처리Disabled = false;

            this.Validate = "e-multi-column";

        }

        private SfGrid<공정단위자재현황> GridBom;
        private List<공정단위자재현황> listBom { get; set; }
        private ObservableCollection<공정단위자재현황> listBomOB = new ObservableCollection<공정단위자재현황>();
        public async Task 실적수량Changed(Syncfusion.Blazor.Inputs.ChangeEventArgs<Decimal> args)
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

        #endregion



        #region 반입처리
        private SfGrid<외주생산위치정보> GridBan;
        private List<외주생산위치정보> listBan { get; set; }
        private ObservableCollection<외주생산위치정보> listBanOB = new ObservableCollection<외주생산위치정보>();

        public bool 반입처리Disabled = true;
        public bool 반입처리다이어로그;
        public Decimal Edit양산품이동수량 = 0;
        public Decimal Edit자재불량수량 = 0;

    

        protected object QuantityPriceEditParams { get; set; } = new
        {
           @params = new SfNumericTextBox<decimal>() { Decimals = 3, Format = "C3", ValidateDecimalOnType = true, Step = 0.003m }
        };

        public void OnChange(Syncfusion.Blazor.Inputs.ChangeEventArgs<decimal> args)
        {
            //decimal 필요수량 = 0.0003M;
            //Edit양산품이동수량 = args.Value - 필요수량;
            StateHasChanged();
        }

        private async Task 반입등록클릭()
        {
            반입처리다이어로그 = true;

            //if (selectedRow == null)
            //    return;
            await Task.Delay(500);

            외주작업지시 = await Remote.Command.기준정보.외주작업지시서번호정보_조회Dz(회사코드, Code);

            var result = await this.Remote.Command.기준정보.직원_조회(true, 회사코드);
            직원정보List = result.ToList();

            var result2 = await Remote.Command.기준정보.부서정보_조회(회사코드);
            부서정보List = result2.ToList();

            var 품목 = await Remote.Command.기준정보.품목구분_조회(외주작업지시.품번);

            // ACCT_FG 0.원재료 1.부재료 2.제품 4.반제품 5.상품 6.저장품 7.비용 8.수익

            var result3 = await Remote.Command.기준정보.장소_조회(회사코드);
            이동공정코드_이동창고코드List = result3.ToList();

            var result4 = await Remote.Command.기준정보.장소위치2_조회(회사코드, "1000");
            이동공정코드_이동장소코드List = result4.ToList();

            if (품목.품목구분코드 == "B1201" || 품목.품목구분코드 == "B1202" || 품목.품목구분코드 == "B1203")
            {
                Edit이동작업장_입고장소코드 = "1000";
            }
            else if (품목.품목구분코드 == "B1204")
            {
                Edit이동작업장_입고장소코드 = "1002";
            }
            
            Edit이동공정_입고창고코드 = "1000";
            Query = new Query().Select(new List<string> { "장소코드" }).Where("장소코드", "equal", Edit이동공정_입고창고코드);
            //Query2 = new Query().Select(new List<string> { "위치코드" }).Where("위치코드", "equal", Edit이동작업장_입고장소코드);
            Edit이동작업장_입고장소코드  = "1000";
            Edit반입입고장소코드 = "1009";

            Edit_지시번호 = 외주작업지시.지시번호;

            var BOMALL = await Remote.Command.기준정보.NEW공정단위BOM_정보_조회(회사코드);

            var BOM_Single = BOMALL.Where(x => x.모품번 == 외주작업지시.품번).FirstOrDefault();

            var 더존비오엠 = BOM_Single;
            if (BOM_Single != null)
            {
                var CharCheck = BOM_Single.자품번.EndsWith("I");
                if (CharCheck)
                {
                    더존비오엠 = BOMALL.Where(x => x.모품번 == BOM_Single.자품번).FirstOrDefault();
                }
                else
                {
                    더존비오엠 = BOM_Single;
                }
            }

            var result1 = await Remote.Command.기준정보.외주생산위치정보_조회(회사코드, 외주작업지시.지시번호);

            listBan?.Clear();

            listBan = Enumerable.Range(0, result1.Count).Select(x => new 외주생산위치정보()
            {
                보유품목코드 = result1[x].보유품목코드,
                장소위치코드 = result1[x].장소위치코드,
                수량 = result1[x].수량 * 더존비오엠.필요수량,
                불량수량 = result1[x].불량수량 * 더존비오엠.필요수량,
                필요수량 = 더존비오엠.필요수량,
                외주작업장명 = result1[x].외주작업장명,
                외주창고코드 = result1[x].외주창고코드,
                외주장소코드 = result1[x].외주장소코드,
                LOT번호 = result1[x].LOT번호,
                품목_LOT번호 = result1[x].품목_LOT번호,
            }).ToList();

            listBanOB = new ObservableCollection<외주생산위치정보>(listBan);
        }

        private async Task 반입처리저장()
        {

            
            if (Edit사원코드 == "" || Edit부서코드 == "" )
            {
                this.Validate = "e-error e-multi-column";
                StateHasChanged();
                return;
            }

            var gridban = await GridBan.GetCurrentViewRecords();

            //불량고창고아닌경우
            /*
            int i = 0;
            string 작업번호 = "";
            foreach(var item in gridban)
            {
                if(i == 0 )
                {
                    var 재고이동헤더정보 = new 재고이동헤더정보
                    {
                        회사코드 = 외주작업지시.회사코드,
                        작업일자 = 이동일자selected,
                        이동일자 = 이동일자selected,
                        이동구분 = "1",
                        입출고구분 = "2",
                        사원코드 = Edit사원코드,
                        부서코드 = Edit부서코드,
                        사업장코드 = 외주작업지시.사업장코드,
                        출고창고코드 = item.외주창고코드,
                        출고장소코드 = item.외주장소코드,
                        입고공정_창고코드 = Edit이동공정_입고창고코드,
                        입고작업장_장소코드 = Edit이동작업장_입고장소코드,
                        처리구분 = "1"

                    };
                    작업번호 = await Remote.Command.기준정보.반입처리_재고이동헤더정보_등록(재고이동헤더정보);
                }

                var 재고이동상세정보 = new 재고이동상세정보
                {
                    회사코드 = 외주작업지시.회사코드,
                    작업번호 = 작업번호,
                    작업순번 = 1,
                    품번 = item.보유품목코드,
                    청구수량 = 외주작업지시.수량 * item.수량,
                    이동수량 = item.수량,
                    모품목코드 = 외주작업지시.품번,
                    재공운영여부 = "1",
                    사용여부 = "1",
                    만료여부 = "1",
                    APP_FG = "0",
                    LOT번호 = item.LOT번호,
                    품목_LOT번호 = item.품목_LOT번호,
                };

                await Remote.Command.기준정보.반입처리_재고이동상세정보_등록(재고이동상세정보, item.필요수량);


                if (item.불량수량 > 0)
                {
                    재고이동상세정보 = new 재고이동상세정보
                    {
                        회사코드 = 외주작업지시.회사코드,
                        작업번호 = 작업번호,
                        작업순번 = 1,
                        품번 = item.보유품목코드,
                        청구수량 = 외주작업지시.수량 * item.수량,
                        이동수량 = -item.불량수량,
                        모품목코드 = 외주작업지시.품번,
                        재공운영여부 = "1",
                        사용여부 = "1",
                        만료여부 = "1",
                        APP_FG = "0",
                        LOT번호 = item.LOT번호,
                        상세_비고 = "자재불량",
                        품목_LOT번호 = item.품목_LOT번호,
                    };
                    await Remote.Command.기준정보.불량반입처리_재고이동상세정보_등록(재고이동상세정보, item.필요수량);
                }
                i++;
            }
            */


            // 불량창고로 처리
            int i = 0;
            string 작업번호 = "";
            foreach (var item in gridban)
            {
                if (i == 0)
                {
                    var 재고이동헤더정보 = new 재고이동헤더정보
                    {
                        회사코드 = 외주작업지시.회사코드,
                        작업일자 = 이동일자selected,
                        이동일자 = 이동일자selected,
                        이동구분 = "1",
                        입출고구분 = "2",
                        사원코드 = Edit사원코드,
                        부서코드 = Edit부서코드,
                        사업장코드 = 외주작업지시.사업장코드,
                        출고창고코드 = item.외주창고코드,
                        출고장소코드 = item.외주장소코드,
                        입고공정_창고코드 = Edit이동공정_입고창고코드,
                        입고작업장_장소코드 = Edit이동작업장_입고장소코드,
                        처리구분 = "1"

                    };
                    작업번호 = await Remote.Command.기준정보.반입처리_재고이동헤더정보_등록(재고이동헤더정보);
                }

                var 재고이동상세정보 = new 재고이동상세정보
                {
                    회사코드 = 외주작업지시.회사코드,
                    작업번호 = 작업번호,
                    작업순번 = 1,
                    품번 = item.보유품목코드,
                    청구수량 = 외주작업지시.수량 * item.수량,
                    이동수량 = item.수량,
                    모품목코드 = 외주작업지시.품번,
                    재공운영여부 = "1",
                    사용여부 = "1",
                    만료여부 = "1",
                    APP_FG = "0",
                    LOT번호 = item.LOT번호,
                    품목_LOT번호 = item.품목_LOT번호,
                };

                await Remote.Command.기준정보.반입처리_재고이동상세정보_등록(재고이동상세정보, item.필요수량);

                i++;
            }

            i = 0;
            작업번호 = "";
            foreach (var item in gridban)
            {
                if (item.불량수량 > 0)
                {
                    if (i == 0)
                    {
                        var 재고이동헤더정보 = new 재고이동헤더정보
                        {
                            회사코드 = 외주작업지시.회사코드,
                            작업일자 = 이동일자selected,
                            이동일자 = 이동일자selected,
                            이동구분 = "1",
                            입출고구분 = "2",
                            사원코드 = Edit사원코드,
                            부서코드 = Edit부서코드,
                            사업장코드 = 외주작업지시.사업장코드,
                            출고창고코드 = item.외주창고코드,
                            출고장소코드 = item.외주장소코드,
                            입고공정_창고코드 = Edit이동공정_입고창고코드,
                            입고작업장_장소코드 = Edit반입입고장소코드, //불량품창고
                            처리구분 = "1"

                        };
                        작업번호 = await Remote.Command.기준정보.반입처리_재고이동헤더정보_등록(재고이동헤더정보);
                    }

                    var 재고이동상세정보 = new 재고이동상세정보
                    {
                        회사코드 = 외주작업지시.회사코드,
                        작업번호 = 작업번호,
                        작업순번 = 1,
                        품번 = item.보유품목코드,
                        청구수량 = 외주작업지시.수량 * item.수량,
                        이동수량 = -item.불량수량,
                        모품목코드 = 외주작업지시.품번,
                        재공운영여부 = "1",
                        사용여부 = "1",
                        만료여부 = "1",
                        APP_FG = "0",
                        LOT번호 = item.LOT번호,
                        품목_LOT번호 = item.품목_LOT번호,
                        상세_비고 = "자재불량",
                    };
                    await Remote.Command.기준정보.불량반입처리_재고이동상세정보_등록(재고이동상세정보, item.필요수량);
                }
                i++;
            }


            Edit사원코드 = "";
            Edit부서코드 = "";

            this.Validate = "e-multi-column";
            반입처리다이어로그 = false;
            NotifyMessage(Message.반영성공);


            
        }

        protected async Task RefreshAsync3()
        {
            //await RefreshAsync();
        }
        public async Task 이동수량Changed(Syncfusion.Blazor.Inputs.ChangeEventArgs<Decimal> args)
        {
        }

        private async Task OnRowSelected3(RowSelectEventArgs<외주생산위치정보> e)
        {
        }


        public void ValueChangeHandler2(Syncfusion.Blazor.Calendars.ChangedEventArgs<DateTime?> args)
        {
            // Here you can customize your code
            이동일자selected = new(args.Value.Value.Year, args.Value.Value.Month, args.Value.Value.Day);
        }
        #endregion

        class Notifier
        {
            public event EventHandler SomethingHappened;
            public void DoSomething(Dictionary<int, decimal> dic)
            {
                SomethingHappened(dic);
            }
        }
        private string ConfirmsData(외주지시별검사정보 found)
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
        {}

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
    }
}
