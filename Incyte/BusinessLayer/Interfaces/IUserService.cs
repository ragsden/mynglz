using System;
using Incyte.Entities;
using System.Collections.Generic;
using Incyte;

namespace Incyte.Interfaces
{
    public interface IUserService
    {
        /********   LOGIN  ********************************/

        UserLogin LoginGet(UserLogin userLogin);

        UserLogin LoginCreate(UserLogin userLogin);

        UserLogin LoginPasswordValidateCreate(UserLogin userLogin);

        UserLogin LoginPasswordReset(UserLogin userLogin);

        /********   USERINFO ********************************/

        UserInfo UserInfoGet(int userID);

        UserInfo UserInfoCreate(UserInfo userInfo);

        bool UserInstantTitleUpdate(string userid, string instantTitle);

        bool UserOnlineStatusUpdate(string userid, string status);

        bool UserImageLocationUpdate(string userid, string imageURL);


        List<UserInfo> VisibleProfilesGet(string userID);

        /********   CHECKIN  ********************************/

        bool CheckinCreate(int userID, int businessID);

        bool CheckoutCreate(int userID, int businessID);

        bool InterestAction(int fromUserID, int toUserID, InterestActionType actionType);

        bool UserSelectionUpdate(string fromUserID, string toUserID, UserSelectionType selectionType);

        UserInfo[] UserSelectionGet(string userID, UserSelectionType userSelectionType);


        /******** CHAT **********************/
        Chat UserChatAdd(string fromUserID, string toUserID, string message);

        Chat[] UserChatGet(string fromUserID, string toUserID, string chatId, string NoOfChat);


        //UserCheckIn[] CheckInsGet(int userID);

        /********   LINKUSER  ********************************/

        //UserLink LinkUserCreate(int InitiatorCheckinID, int RecieverCheckinID);

        //UserLink LinkUserRevoke(int InitiatorCheckinID);

        //UserLink LinkUserResponse(int RecieverCheckinID, ResponseType responseType);

        ////UserLink GetUserLink(int InitiatorCheckinID, int RecieverCheckinID);

        //UserLink[] UserLinksGetByCheckInForInitiator(int checkinID);

        //UserLink[] UserLinksGetByCheckInForReciever(int checkinID);

        //UserLink[] UserLinksGetByInitiator(int userID);

        //UserLink[] UserLinksGetByReciever(int userID);

        /********   ENGAGEMENT  ********************************/

        //UserEngaged UserEngagementCreateByInitiator(int userLinkID);

        //UserEngaged UserEngagementCreateByReciever(int userLinkID);

        //UserEngaged UserGetEngagementGetByLinkID(int userLinkID);

        //UserEngaged UserGetEngagementEndByLinkID(int userLinkID);


    }
}
