using HiSFS.WebApp.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiSFS.WebApp.Component
{
    public class CustomRouteView : RouteView
    {
        [Inject]
        private Context Context { get; set; }

        private RouteData routeData;

        [Parameter]
        public RouteData RoutedData
        {
            get => routeData;
            set
            {
                routeData = value;

                Context.PageType = routeData.PageType;
            }
        }
    }
}
