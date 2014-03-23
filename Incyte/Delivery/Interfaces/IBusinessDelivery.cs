using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Runtime.Serialization.Json;
using System.Text;
using Incyte;
using Incyte.Entities;
using Incyte.Services;

namespace Delivery
{
    // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
    // a single instance of the service to process all calls.	
    [ServiceContract]
    // NOTE: If the service is renamed, remember to update the global.asax.cs file
    public interface IBusinessDelivery
    {
        // TODO: Implement the collection resource that will contain the SampleItem instances

        //[WebGet(UriTemplate = "{longitude}/{latitude}/{search}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]

        [WebInvoke(Method = "GET",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "{longitude}/{latitude}/{search}")]
        Businesses GetBusiness(string longitude, string latitude, string search);

        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          UriTemplate = "add/{businessids}")]
        Businesses AddBusiness(string businessids);
        
    }
}
