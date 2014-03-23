using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace test1wcf
{
    // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
    // a single instance of the service to process all calls.	
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    // NOTE: If the service is renamed, remember to update the global.asax.cs file
    public class Service1
    {
        // TODO: Implement the collection resource that will contain the SampleItem instances

        [WebGet(UriTemplate = "")]
        public List<SampleItem> GetCollection()
        {
            // TODO: Replace the current implementation to return a collection of SampleItem instances
            return new List<SampleItem>() { new SampleItem() { Id = 1, StringValue = "Hello" }, new SampleItem() { Id = 2, StringValue = "Hello1" } };
        }

        [WebInvoke(UriTemplate = "", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public SampleItem Create(SampleItem instance)
        {
            // TODO: Add the new instance of SampleItem to the collection
            throw new NotImplementedException();
        }

        [WebGet(UriTemplate = "{id}")]
        public SampleItem Get(string id)
        {
            // TODO: Return the instance of SampleItem with the given id
            return new SampleItem() { Id = int.Parse(id), StringValue = "Hello " + id };
        }

        [WebInvoke(UriTemplate = "{id}", Method = "PUT",RequestFormat= WebMessageFormat.Json, ResponseFormat=WebMessageFormat.Json)]
        public SampleItem Update(string id, string instance)
        {
            // TODO: Update the given instance of SampleItem in the collection
            return new SampleItem() { Id = int.Parse(id), StringValue = "Hello " + id };
        }

        [WebInvoke(UriTemplate = "{id}", Method = "DELETE")]
        public void Delete(string id)
        {
            // TODO: Remove the instance of SampleItem with the given id from the collection
            throw new NotImplementedException();
        }

        [WebGet(
          ResponseFormat = WebMessageFormat.Json, // Return results as JSON
          UriTemplate = "/categories/{id}/{instance}")]
        public SampleItem TestGet(string id, string instance)
        {
            // TODO: Update the given instance of SampleItem in the collection
            return new SampleItem() { Id = int.Parse(id), StringValue = "Hello " + id };
        }

        [WebInvoke(
            Method="PUT",
         ResponseFormat = WebMessageFormat.Json, // Return results as JSON
         RequestFormat = WebMessageFormat.Json,
         UriTemplate = "/test")]
        public SampleItem Tester(string id, string instance)
        {
            // TODO: Update the given instance of SampleItem in the collection
            return new SampleItem() { Id = int.Parse(id), StringValue = "Hello " + id };
        }

    }
}
