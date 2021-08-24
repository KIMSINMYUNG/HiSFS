using HiSFS.Api.Shared.Models;
using HiSFS.Api.Shared.Models.View_DZICUBE;
using HiSFS.WebApp.Component.Common;
using HiSFS.WebApp.Services;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HiSFS.WebApp.Pages.QV.IV
{
    public partial class 외주품질검사목록Page
    {

        private TGrid<공정단위검사장비> grid5;
        private ObservableCollection<공정단위검사장비> list5 = new ObservableCollection<공정단위검사장비>();


        public ObservableCollection<품질검사생산품> 품질검사생산품목록 { get; set; }

        public List<품질검사생산품> list3 = new List<품질검사생산품>();

        private 품질검사생산품 selected품질검사생산품;

        private SfGrid<품질검사생산품> grid3;


        private void OnRowSelected(RowSelectEventArgs<외주작업지시서정보> args)
        {
            selectedRow = args.Data;

            Edit실적수량 = selectedRow.수량;
        }
        private async Task OnConnection(공정단위검사장비 info, bool isConn)
        {

        }

        public void QueryCellInfoHandler(QueryCellInfoEventArgs<공정단위검사정보> Args)
        {
            if (Args.Column.Type == ColumnType.CheckBox && (Args.Data.합격여부 == "합격" || Args.Data.합격여부 == "불합격"))
            {
                Args.Cell.AddClass(new string[] { "e-checkbox-disabled" });
            }

        }



        private void OnActionBegin(ActionEventArgs<공정단위검사장비> args)
        {
            //await Task.Yield();

            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Add)
            {
                //var parentRow = grid.Grid.SelectedRecords.FirstOrDefault();
                //if (parentRow == null || selected공정단위검사정보 == null)
                //{
                //    args.Cancel = true;
                //    return;
                //}
            }
        }
        private async Task OnActionComplete(ActionEventArgs<공정단위검사장비> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Add)
            {
                //state = "Add";
                // var parentRow = grid3.GetCurrentViewRecords.

                var newRow = args.Data;
                newRow.공정단위코드 = "PU0004:1"; //생산지시.생산지시공정차수목록[selected차수 - 1].생산품공정차수.공정단위코드;  //parentRow.공정단위코드;

                var result = await Remote.Command.생산관리.공정단위검사_조회(newRow.공정단위코드);

                var rsesult2 = from 품질검사 in result
                               where 품질검사.공정단위코드 == newRow.공정단위코드
                               select 품질검사;

                foreach (var item in rsesult2)
                {
                    newRow.품질검사코드 = item.품질검사코드;
                }
            }
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
            {
                //state = "BeginEdit";
            }
            // 삭제 처리
            else if (args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
            {
                await Remote.Command.생산관리.공정단위검사장비_삭제(args.Data, true);

                await RefreshAsync();

                NotifyMessage(Message.ModifiedDeleteData);
            }
            // 저장 관련 처리
            else if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                var newData = args.Data;

                await Remote.Command.생산관리.공정단위검사장비_저장(newData, args.Action == "Add" ? true : false);
                if (args.Action == "Add")
                    await RefreshAsync();

                ModifyList(list5, (info, map) =>
                {
                    if (info.검사장비.연동장비유형코드 != null)
                        info.검사장비.연동장비유형 = map[info.검사장비.연동장비유형코드];
                }, false);

                NotifyMessage(args.Action == "Add" ? Message.ModifiedAddData : Message.ModifiedUpdateData);
            }
        }


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
