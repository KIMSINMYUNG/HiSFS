using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiSFS.WebApp.Component.Common
{
    public class DGrid3<TValue> : SfGrid<TValue>
    {
        //[Parameter]
        //public EventCallback OnInitAfter { get; set; }

        //[Parameter]
        //public EventCallback OnRenderAfter { get; set; }

        [Parameter]
        public bool DefaultMode { get; set; }

        [Parameter]
        public DGridEditMode EditMode { get; set; }

        [Parameter]
        public PageMode PageMode { get; set; }

        [Parameter]
        public int PageSize { get; set; }

        [Parameter]
        public List<object> AddToolbar { get; set; }

        [Parameter]
        public string[] SearchFields { get; set; }


        private static readonly List<object> editModeToolbars = new List<object>
        {
            "Add", "Edit", "Delete", "Cancel", "Update", "Search", "ExcelExport",

            new Syncfusion.Blazor.Navigations.ItemModel { Text="새로고침", TooltipText = "새로고침", PrefixIcon = "e-reload", Align = ItemAlign.Left, Id = "Reload" }
        };

        private static readonly List<object> readModeToolbars = new List<object>
        {
            "Search", "ExcelExport",

            new Syncfusion.Blazor.Navigations.ItemModel { Text="새로고침", TooltipText = "새로고침", PrefixIcon = "e-reload", Align = ItemAlign.Left, Id = "Reload" }
        };

        private static readonly List<object> popupModeToolbars = new List<object>
        {
            "Search"
        };

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "BL0005:Component parameter should not be set outside of its component.", Justification = "<보류 중>")]
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (DefaultMode == true)
            {
                if (PageMode != PageMode.Inline && PageSize != -1)
                    AllowPaging = true;

                PageSettings.PageCount = 10;

                var toolbar = new List<object>();

                if (PageMode == PageMode.Default)
                {
                    PageSettings.PageSize = PageSize == 0 ? 19 : PageSize;

                    AllowExcelExport = true;

                    if (PageMode != PageMode.Inline)
                        toolbar.AddRange(readModeToolbars);
                }
                else
                {
                    PageSettings.PageSize = PageSize == 0 ? 10 : PageSize;

                    if (PageMode != PageMode.Inline)
                        toolbar.AddRange(popupModeToolbars);
                }
                SelectionSettings.Type = SelectionType.Single;
                SelectionSettings.EnableToggle = false;

                if (SearchFields != null)
                    SearchSettings.Fields = SearchFields;
                SearchSettings.Operator = Syncfusion.Blazor.Operator.Contains;
                SearchSettings.IgnoreCase = true;

                if (AddToolbar != null)
                    toolbar.AddRange(AddToolbar);

                Toolbar = toolbar;
            }

            if (EditMode > DGridEditMode.None)
            {
                EditSettings.AllowAdding = EditMode.HasFlag(DGridEditMode.Add);
                EditSettings.AllowDeleting = EditMode.HasFlag(DGridEditMode.Delete);
                EditSettings.AllowEditing = EditMode.HasFlag(DGridEditMode.Edit);
                EditSettings.Mode = Syncfusion.Blazor.Grids.EditMode.Dialog;
                //EditSettings.Mode = Syncfusion.Blazor.Grids.EditMode.Normal;
                EditSettings.NewRowPosition = NewRowPosition.Bottom;
                EditSettings.ShowDeleteConfirmDialog = true;

                Toolbar = editModeToolbars;
                ContextMenuItems = new List<object> { "Delete" };
            }

            //await OnInitAfter.InvokeAsync(null);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender == true)
            {
                //await ShowSpinner();
                //await OnRenderAfter.InvokeAsync(null);
                //await HideSpinner();
            }
        }
    }

    [Flags]
    public enum DGridEditMode
    {
        None = 0b000,
        All = 0b111,
        Add = 0b100,
        Edit = 0b010,
        AddEdit = 0b110,
        Delete = 0b001,
        AddDelete = 0b101,
        EditDelete = 0b011
    }
}
