using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace Incyte.Entities
{
    public class Message
    {
        public string text { get; set; }
        public int code { get; set; }
        public string version { get; set; }
    }

    public class Category
    {
        public string category_filter { get; set; }
        public string search_url { get; set; }
        public string name { get; set; }
    }

    public class Review
    {
        public string rating_img_url_small { get; set; }
        public string user_photo_url_small { get; set; }
        public string rating_img_url { get; set; }
        public int rating { get; set; }
        public string user_url { get; set; }
        public string url { get; set; }
        public string mobile_uri { get; set; }
        public string text_excerpt { get; set; }
        public string user_photo_url { get; set; }
        public string date { get; set; }
        public string user_name { get; set; }
        public string id { get; set; }
    }


    public class RootObject
    {
        public Message message { get; set; }
        public List<Business> Businesses { get; set; }
    }
}