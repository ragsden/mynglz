using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Incyte;
using Incyte.Entities;
using Incyte.Services;
using Incyte.Resource;
using Incyte.Interfaces;
using Test.mocks;

namespace Test
{
    [TestClass]
    public class BusinessServiceTest
    {
        [TestMethod]
        public void BusinessGetWithMock()
        {
            IBusinessResource br = new YelpBusinessResource();
            //br = new BusinessResource();

            BusinessService bs = new BusinessService(
                "http://api.yelp.com/business_review_search?limit=20&ywsid=HJPpruk8klLzsyUyumugXA"
                , br);

            Location loc = new Location();
            //loc.Category = "bars";
            //loc.Latitude = "47.57434809999999";
            //loc.Longitude = "-122.1248621";
            loc.CityName = "bothell";
            loc.Zipcode = "98021";
            loc.Radius = "1";
            loc.BusinessName = "panera";

            bs.BusinessGet(loc);
        }
        [TestMethod]
        public void BusinessGetWithDB()
        {


            BusinessService bs = new BusinessService();

            Location loc = new Location();
            //loc.Category = "bars";
            //loc.Latitude = "47.57434809999999";
            //loc.Longitude = "-122.1248621";
            loc.CityName = "bothell";
            loc.Zipcode = "98021";
            loc.Radius = "1";
            loc.BusinessName = "panera";

            bs.BusinessGet(loc);
        }
    }

}
