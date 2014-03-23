using System;
using Incyte;

namespace Incyte.Entities
{
    public partial class UserInfo
    {

        public int UserID {get;set;}

        public string InstantTitle {get;set;}

        public string PictureLocation {get;set;}

        public short OnlineStatusID {get;set;}

        public GenderType Gender {get;set;}

        public PreferenceType Preference {get;set;}

        public short Age {get;set;}

        public UIScanType InterestType { get; set; }

        public string Handle { get; set; }

        public string EmailAddress { get; set; }

        public Chat[] Chats { get; set; }

    }

    public class FBUser
    {
        //picture,id,email,first_name,last_name,gender
        public string id { get; set; }
        public string name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string picture { get; set; }
        public string username { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
    }
}
