using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TrendReports
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            RouteTable.Routes.MapRoute(
                "ServiceData",
                "Service/ServiceData/{startDate}/{endDate}/{dataPoint}/{location}", // URL with parameters
                new { controller = "Service", action = "ServiceData", startDate = UrlParameter.Optional, endDate = UrlParameter.Optional, dataPoint = UrlParameter.Optional, location = UrlParameter.Optional }
            );

            RouteTable.Routes.MapRoute(
                "SalesData",
                "Sales/SalesData/{startDate}/{endDate}/{dataPoint}/{location}", // URL with parameters
                new { controller = "Sales", action = "SalesData", startDate = UrlParameter.Optional, endDate = UrlParameter.Optional, dataPoint = UrlParameter.Optional, location = UrlParameter.Optional }
            );
        }
    }
}
