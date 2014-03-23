using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Web;
using Incyte;
using Incyte.Entities;
using Incyte.Services;


namespace Delivery
{
    // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
    // a single instance of the service to process all calls.	
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    // NOTE: If the service is renamed, remember to update the global.asax.cs file

    public class ApplicationDelivery : IApplicationDelivery //: IBusinessServie
    {
        public string GetVersion()
        {
            
            string authkey = HttpContext.Current.Request.Headers["authkey"];
            Logging.WriteToFileLog(string.Format("authkey : {0}", authkey));
            if (!HttpContext.Current.Request.Headers.AllKeys.Contains("authkey") ||
                string.IsNullOrEmpty(authkey) ||
                !authkey.Trim().Equals("MynglzRUs", StringComparison.InvariantCultureIgnoreCase))
                return Guid.NewGuid().ToString();
            
            return "1";


        }

        public Settings GetSettings()
        {

            Settings settings = new Settings();
            settings.BRR = ConfigurationManager.AppSettings["BrowseRefreshRate"];
            settings.CRR = ConfigurationManager.AppSettings["ChatRefreshRate"];
            settings.Radius = ConfigurationManager.AppSettings["Radius"];

            List<string> categories = ConfigurationManager.AppSettings["PlaceCategories"].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

            settings.Categories = categories;

            return settings;
        }
    }
}
