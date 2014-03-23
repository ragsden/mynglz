using System;
using System.Collections;


namespace Incyte.Entities
{
    public class LocationQueryString
    {
        public readonly string Name;
        public readonly string QueryName;

        public LocationQueryString(string name, string queryName)
        {
            this.Name = name;
            this.QueryName = queryName;
        }
    }

    public class Location
    {
        public Hashtable SetValues = new Hashtable();

        public string Latitude
        {
            get { return (SetValues["Latitude"] as LocationQueryString).Name; }
            set { SetValues["Latitude"] = new LocationQueryString(value, "lat"); }
        }

        public string Longitude
        {
            get { return (SetValues["Longitude"] as LocationQueryString).Name; }
            set { SetValues["Longitude"] = new LocationQueryString(value, "long"); }
        }

        public string Radius
        {
            get { return (SetValues["Radius"] as LocationQueryString).Name; }
            set { SetValues["Radius"] = new LocationQueryString(value, "radius"); }
        }

        public string Zipcode
        {
            get { return (SetValues["Zipcode"] as LocationQueryString).Name; }
            set { SetValues["Zipcode"] = new LocationQueryString(value, "location"); }
        }

        public string BusinessName
        {
            get { return (SetValues["BusinessName"] as LocationQueryString).Name; }
            set { SetValues["BusinessName"] = new LocationQueryString(value, "term"); }
        }

        public string CityName
        {
            get { return (SetValues["CityName"] as LocationQueryString).Name; }
            set { SetValues["CityName"] = new LocationQueryString(value, "location"); }
        }

        public string Category
        {
            get { return (SetValues["Category"] as LocationQueryString).Name; }
            set { SetValues["Category"] = new LocationQueryString(value, "category"); }
        }
    }
}
