using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Incyte.Entities
{
    public class Businesses
    {
        public Business[] businesses;
    }
    public class Business
    {

        public int BusinessId {get;set;}

        [JsonProperty("id")]
        public string ExternalID { get; set; }

        public int SourceID { get; set; }

        public string Rating_img_url { get; set; }
        
        public string Country_code { get; set; }

        public bool Is_closed { get; set; }
        
        public string City { get; set; }
        
        public string Mobile_url { get; set; }
        
        public int Review_count { get; set; }
        
        public string Zip { get; set; }
        
        public string State { get; set; }
        
        public string Rating_img_url_small { get; set; }
        
        public string Address1 { get; set; }
        
        public string Address2 { get; set; }
        
        public string Address3 { get; set; }
        
        public string Phone { get; set; }
        
        public string State_code { get; set; }
        
        //public List<Category> categories { get; set; }
        
        public string Photo_url { get; set; }
        
        public double Distance { get; set; }
        
        public string Name { get; set; }
        
        //public List<object> neighborhoods { get; set; }
        
        public string Url { get; set; }
        
        public string Country { get; set; }
        
        public double Avg_rating { get; set; }
        
        public double Longitude { get; set; }
        
        public string Nearby_url { get; set; }
        
        //public List<Review> reviews { get; set; }
        
        public string Photo_url_small { get; set; }

        public int CheckinCount { get; set; }

        public bool Disabled {get;set;}
    }
}
