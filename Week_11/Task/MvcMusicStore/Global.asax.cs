using MvcMusicStore.Infrastructure;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MvcMusicStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Error(Server.GetLastError().Message);
        }

        protected void Application_End()
        {
            using (var counter =
                PerformanceCounterHelper.PerformanceHelper.CreateCounterHelper<UserLoginCounter>("Login Counter"))
            {
                counter.Reset(UserLoginCounter.LogIn);
                counter.Reset(UserLoginCounter.LogOff);
                counter.Reset(UserLoginCounter.Error);
            }
                
        }
    }
}
