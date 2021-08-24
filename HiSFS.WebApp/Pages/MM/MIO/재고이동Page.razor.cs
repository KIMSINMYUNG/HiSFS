using HiSFS.Api.Shared.Models;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace HiSFS.WebApp.Pages.MM.MIO
{
    public partial class 재고이동Page 
    {
        private SfGrid<외주생산위치정보> GridBom;
        private List<외주생산위치정보> listBom { get; set; }
        private ObservableCollection<외주생산위치정보> listBomOB = new ObservableCollection<외주생산위치정보>();
        protected  async Task RefreshAsync3()
        {
           
            //await RefreshAsync();
        }


        private async Task OnRowSelected3(RowSelectEventArgs<외주생산위치정보>  e)
        {
            
        }



    }
}
