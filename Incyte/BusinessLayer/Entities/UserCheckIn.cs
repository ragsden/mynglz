using System;

namespace Incyte.Entities
{
    public class UserCheckIn
    {

        public int UserID { get; set; }

        public System.DateTime StartTime { get; set; }

        public System.Nullable<System.DateTime> EndTime { get; set; }

        public int BusinessId { get; set; }

        public int CheckinID { get; set; }
    }
}
