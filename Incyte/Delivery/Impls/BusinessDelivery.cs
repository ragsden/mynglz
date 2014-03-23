using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
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

    public class BusinessDelivery : IBusinessDelivery //: IBusinessServie
    {
        public Businesses GetBusiness(string longitude, string latitude, string search)
        {
            try
            {
                BusinessService businessService = new BusinessService();
                Location location = new Location();
                location.Latitude = latitude;
                location.Longitude = longitude;
                if(!search.ToLower().Trim().Equals("null"))
                    location.BusinessName = search;

                Logging.WriteToFileLog(string.Format( "long : {0} , lat : {1}, search : {2}", longitude, latitude, search));
                
                // Serialize the results as JSON
                /*DataContractJsonSerializer serializer = new DataContractJsonSerializer(results.GetType());
                MemoryStream memoryStream = new MemoryStream();
                serializer.WriteObject(memoryStream, results);

                // Return the results serialized as JSON
                string json = Encoding.Default.GetString(memoryStream.ToArray());
                return json;
                */

                WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
                return new Businesses() { businesses = businessService.BusinessGet(location).ToArray() };
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("Business GetBusiness " + e.Message);
            }
            return null;
        }

        public Businesses AddBusiness(string businessids)
        {
            try
            {
                Logging.WriteToFileLog(string.Format("businesses ids : {0}", businessids));
                if (!string.IsNullOrEmpty(businessids.Trim()))
                {
                    List<Business> businesses = new List<Business>();
                    businessids.Trim().Split(",".ToCharArray(),StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(x => businesses.Add(new Business() { ExternalID = x }));

                    Logging.WriteToFileLog(string.Format("businesses count : {0}", businesses.Count));
                    BusinessService businessService = new BusinessService();
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
                    return new Businesses() { businesses = businessService.BusinessGet(businesses).ToArray() };
                }
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("Business GetBusiness " + e.Message);
            }
            return null;
        }

        public string GetMyBusiness(string longitude, string latitude)
        {
            BusinessService businessService = new BusinessService();
            Location location = new Location();
            location.Latitude = latitude;
            location.Longitude = longitude;
            List<Business> businesses = businessService.BusinessGet(location);
            // Serialize the results as JSON
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(businesses.GetType());
            MemoryStream memoryStream = new MemoryStream();
            serializer.WriteObject(memoryStream, businesses);

            // Return the results serialized as JSON
            string json = Encoding.Default.GetString(memoryStream.ToArray());
            return json;

        }
    }
}
