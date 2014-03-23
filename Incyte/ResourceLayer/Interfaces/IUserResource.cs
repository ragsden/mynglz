using System;
using System.Collections.Generic;

namespace Incyte.Interfaces
{
    public interface IUserResource
    {
        /********   LOGIN  ********************************/

        void LoginCreate(string userName, string password, SourceType UserSourceType, string ExternalID, Converter converter, object dataObject);
        
        void LoginPasswordGet(string userName, string password,Converter converter, object dataObject);

        void LoginPasswordValidateCreate(string userName, string password, Converter converter, object dataObject);

        void LoginPasswordValidateReset(string userid, string userName, string password);

        void UserInfoGet(int userID, Converter converter, object dataObject);

        void UserInfoCreate(int userID, GenderType gender, PreferenceType preferenceType, short age, string pictureLocation, string status, string email, string handle, Converter converter, object dataObject);

        void UserInstantTitleUpdate(string userid, string instantTitle);

        void UserOnlineStatusUpdate(string userid, short status);

        void UserImageLocationUpdate(string userid, string imageURL);

        void CheckinCreate(int userID, int businessID);

        void CheckoutCreate(int userID, int businessID);

        void InterestAction(int fromUserID, int toUserID, InterestActionType actionType);

        void VisibleProfilesGet(string userID, Converter converter, object dataObject);

        void UserSelectionUpdate(string fromUserID, string toUserID, UserSelectionType selectionType);

        void UserSelectionGet(string userID, UserSelectionType userSelectionType, Converter converter, object dataObject);

        void UserChatGet(string fromUserID, string toUserID,string chatId, string NoOfChat, Converter converter, object dataObject);

        void UserChatAdd(string fromUserID, string toUserID, string message, Converter converter, object dataObject);

    }

}
