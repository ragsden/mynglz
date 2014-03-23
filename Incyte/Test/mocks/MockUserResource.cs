using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Incyte.Interfaces;
using Incyte;

namespace Test.mocks
{
    public class MockUserResource : IUserResource
    {
        public int CheckedInUserID;
        public int CheckedOutUserID;
        public int CheckedInBussinessID;
        public int CheckedOutBussinessID;
        public int ShowInterestFromUserID;
        public int ShowInterestToUserID;
        public int RevertInterestFromUserID;
        public int RevertInterestToUserID;
        public int HangupFromUserID;
        public int HangupToUserID;

        public void LoginCreate(string userName, string password, SourceType UserSourceType, string ExternalID, Converter converter, object dataObject)
        {
            throw new NotImplementedException();
        }

        public void UserInfoCreate(int userID, GenderType gender, PreferenceType preferenceType, short age, string pictureLocation, string status, string email, string handle, Converter converter, object dataObject)
        {
            throw new NotImplementedException();
        }

        public void CheckinCreate(int userID, int businessID)
        {
            this.CheckedInUserID = userID;
            this.CheckedInBussinessID = businessID;
        }
        public void CheckoutCreate(int userID, int businessID)
        {
            this.CheckedOutUserID = userID;
            this.CheckedOutBussinessID = businessID;
        }

        public void InterestAction(int fromUserID, int toUserID, InterestActionType actionType)
        {
            if (actionType == InterestActionType.Revert)
            {
                this.RevertInterestFromUserID = fromUserID;
                this.RevertInterestToUserID = toUserID;
            }
            else if (actionType == InterestActionType.Hangup)
            {
                this.HangupFromUserID = fromUserID;
                this.HangupToUserID = toUserID;
            }
            else if (actionType == InterestActionType.Link)
            {
                this.ShowInterestFromUserID = fromUserID;
                this.ShowInterestToUserID = toUserID;
            }
        }
        public void VisibleProfilesGet(string userID, Converter converter, object dataObject)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("UserID", typeof(int));
            dt.Columns.Add("PictureLocation", typeof(string));
            dt.Columns.Add("OnlineStatusID", typeof(short));
            dt.Columns.Add("InstantTitle", typeof(string));
            dt.Columns.Add("Gender", typeof(short));
            dt.Columns.Add("Age", typeof(int));
            dt.Columns.Add("Preference", typeof(short));

            for (int i = 0; i < 10; i++)
            {
                DataRow dr = dt.NewRow();
                dr["UserID"] = i;
                dr["PictureLocation"] = "http://" + i + ".com";
                dr["OnlineStatusID"] = 1;
                dr["InstantTitle"] = "InstantTitle" + i;
                dr["Gender"] = 1;
                dr["Age"] = 33;
                dr["Preference"] = 1; 
                dt.Rows.Add(dr);
            }
            DataTableReader dtr = dt.CreateDataReader();
            while (dtr.Read())
            {
                converter(dtr, dataObject);
            }
            //dt.Select().ToList().ForEach(x => converter(x, dataObject));
        }


        #region IUserResource Members


        public void UserInfoGet(int userID, Converter converter, object dataObject)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IUserResource Members


        public void UserInstantTitleUpdate(string userid, string instantTitle)
        {
            throw new NotImplementedException();
        }

        public void UserOnlineStatusUpdate(string userid, short status)
        {
            throw new NotImplementedException();
        }

        public void UserSelectionUpdate(string fromUserID, string toUserID, UserSelectionType selectionType)
        {
            throw new NotImplementedException();
        }

        public void UserSelectionGet(string userID, UserSelectionType userSelectionType, Converter converter, object dataObject)
        {
            throw new NotImplementedException();
        }

        public void UserImageLocationUpdate(string userid, string imageURL)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IUserResource Members


        public void UserChatGet(string fromUserID, string toUserID, string chatId, string NoOfChat, Converter converter, object dataObject)
        {
            throw new NotImplementedException();
        }

        public void UserChatAdd(string fromUserID, string toUserID, string message, Converter converter, object dataObject)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
