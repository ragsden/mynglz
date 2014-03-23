using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incyte
{
    public enum SourceType
    {
        Facebook = 1,
        Internal = 0
    }
    public enum PreferenceType
    {
        MALE = 1,
        FEMALE = 2,
        BOTH = 3

    }

    public enum UserSelectionType
    {
        LIKE = 1,
        DISLIKE = 2,
        HIT = 3,
        MISS = 4,
        MY  = 5,
        OTHERS = 6

    }

    public enum GenderType
    {
        MALE = 1,
        FEMALE = 2

    }

    public enum InterestType
    {
        LIKE = 1,
        DISLIKE = 0

    }

    public enum UIScanType
    {
        BROWSE = 0,
        LIKE = 1,
        HIT = 2,
        MISS = 3,
        TRASH = 4

    }
    public enum InterestActionType
    {
        Revert = 1,
        Hangup = 2,
        Link = 3
    }

    public enum UserOnlineStatus
    {
        Offline = 0,
        Online = 1,
        Anonymous = 2
    }
    public struct BusinessLight
    {
        public BusinessLight(string ExternalID, string Source)
        {
            this.ExternalID = ExternalID;
            this.Source = Source;
        }
        public string ExternalID;
        public string Source;
    }

    public struct SelectedUser
    {
        public SelectedUser(int UserId, int StatusId)
        {
            this.UserId = UserId;
            this.StatusId = StatusId;
        }
        public int UserId;
        public int StatusId;
    }
}
