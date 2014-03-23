using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Incyte.Entities
{
    public class Settings
    {
        public string Radius {get;set;}
        public List<string> Categories { get; set; }
        public string BRR { get; set; } //Browse refresh rate
        public string CRR { get; set; } //Chat refresh rate
    }
}
