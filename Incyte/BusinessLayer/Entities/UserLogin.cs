using System;

namespace Incyte.Entities
{
    public partial class UserLogin
    {

        public int UserID {get;set;}

        public string UserName {get;set;}

        public string Password {get;set;}

        public SourceType UserSourceType {get;set;}

        public string ExternalID {get;set;}

        public bool IsNew { get; set; }

        public bool Exists { get; set; }
        
    }
}
