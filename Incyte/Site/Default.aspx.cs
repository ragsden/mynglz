using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Incyte
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string str = @"http://api.yelp.com/business_review_search?term=yelp&lat={0}&long={1}&radius=1&limit=20&ywsid=HJPpruk8klLzsyUyumugXA&category=bars";

            
        }
    }
}