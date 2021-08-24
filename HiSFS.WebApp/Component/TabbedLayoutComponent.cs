using Syncfusion.Blazor.Navigations;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable BL0005

namespace HiSFS.WebApp.Component
{
    public abstract class TabbedLayoutComponent : CustomLayoutComponent
    {
        public SfTab Tab { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            await Tab.AddTab(new List<TabItem>
            {
                new TabItem
                {
                    Header = new TabHeader { Text = "AAA" },
                    ContentTemplate = builder =>
                    {
                        builder.AddContent(1, Body);
                    }
                },
            }, Tab.Items?.Count ?? 0);
            Tab.SelectedItem = Tab.Items.Count;
        }
    }
}