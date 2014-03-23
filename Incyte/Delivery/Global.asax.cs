using System;
using System.ServiceModel.Activation;

using System.Web;
using System.Web.Routing;
using Incyte;
using System.Linq;
using System.Configuration;


namespace Delivery
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes();
        }


        private void RegisterRoutes()
        {
            // Edit the base address of Service1 by replacing the "Service1" string below
            //RouteTable.Routes.Add(new ServiceRoute("Service1", new WebServiceHostFactory(), typeof(Service1)));

            RouteTable.Routes.Add(new ServiceRoute("business", new WebScriptServiceHostFactory(), typeof(BusinessDelivery)));
            RouteTable.Routes.Add(new ServiceRoute("user", new WebScriptServiceHostFactory(), typeof(UserDelivery)));
            RouteTable.Routes.Add(new ServiceRoute("app", new WebScriptServiceHostFactory(), typeof(ApplicationDelivery)));
            RouteTable.Routes.Add(new ServiceRoute("checkin", new WebScriptServiceHostFactory(), typeof(UserDelivery)));
            
        }

        void Application_Error()
        {
            
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();

            EnableCrossDmainAjaxCall();

            HttpContext.Current.Response.Write("Invalid Request");
            HttpContext.Current.Response.End();
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            
            Logging.WriteToFileLog(ConfigurationManager.AppSettings["ShudAuth"]);

            string authkey = HttpContext.Current.Request.Headers["authkey"];
            Logging.WriteToFileLog(string.Format("authkey : {0}", authkey));
            if (ConfigurationManager.AppSettings["ShudAuth"].Equals("1") && (!HttpContext.Current.Request.Headers.AllKeys.Contains("authkey") ||
                string.IsNullOrEmpty(authkey) ||
                !authkey.Trim().Equals("MynglzRUs", StringComparison.InvariantCultureIgnoreCase)))
                //Application_Error();

             

            Logging.WriteToFileLog("authkey success");
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();

            EnableCrossDmainAjaxCall();
            
        }

        private void EnableCrossDmainAjaxCall()
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");

            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
        }   

    }
}
