using System;

namespace Incyte.Entities
{
    public class UserEngaged
    {

        public int UserLinkID {get;set;}

        public int InitiatorCheckinID { get; set; }

        public int RecieverCheckinID { get; set; }

        public System.DateTime StartTime {get;set;}

        public System.Nullable<System.DateTime> EndTime {get;set;}

    }
}
