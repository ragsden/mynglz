using System;

namespace Incyte.Entities
{
    public class State
    {
        public int StateID {get;set;}

        public string StateShortName {get;set;}

        public string StateLongName {get;set;}

        public System.DateTime UpdatedDtTm {get;set;}

        public string UpdatedBy {get;set;}

        public int CountryId {get;set;}
    }
}
