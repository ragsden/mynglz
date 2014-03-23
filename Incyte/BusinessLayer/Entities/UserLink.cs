using System;

namespace Incyte.Entities
{
    public class UserLink
    {
        public int InitiatorCheckinID {get;set;}

        public int InitiatorUserID {get;set;}

        public int RecieverCheckinID { get; set; }

        public int RecieverUserID {get;set;}

        public System.DateTime InitiatedDtTm {get;set;}

        public System.Nullable<System.DateTime> ResponseDtTm {get;set;}

        public short ResponseTypeID {get;set;}

        public int UserLinkID {get;set;}
    }
}
