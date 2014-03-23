using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Incyte.Interfaces;
using Incyte.Resource;
using Incyte.Entities;

namespace Incyte.Services
{
    public class UserService : IUserService
    {
        private IUserResource userResource = null;

        public UserService()
        {

            if (this.userResource == null)
            {
                this.userResource = new UserResource(ConfigurationManager.ConnectionStrings[Constants.UserResourceConnection].ConnectionString);
            }
        }

        public UserService(IUserResource userResource)
        {
            this.userResource = userResource;
        }

        public UserLogin LoginGet(UserLogin userLogin)
        {
            this.userResource.LoginPasswordGet(userLogin.UserName, userLogin.Password, this.UserLoginConverter, userLogin);

            return userLogin;
        }

        public UserLogin LoginCreate(UserLogin userLogin)
        {
            this.userResource.LoginCreate(userLogin.UserName, userLogin.Password, userLogin.UserSourceType, userLogin.ExternalID, this.UserLoginConverter, userLogin);

            return userLogin;
        }

        public UserLogin LoginPasswordValidateCreate(UserLogin userLogin)
        {
            this.userResource.LoginPasswordValidateCreate(userLogin.UserName, userLogin.Password, this.UserLoginConverter, userLogin);

            return userLogin;
        }

        public UserLogin LoginPasswordReset(UserLogin userLogin)
        {
            userLogin.Password = new Random().Next(1000).ToString();
            
            this.userResource.LoginPasswordValidateReset(userLogin.UserID.ToString(), userLogin.UserName, userLogin.Password);

            return userLogin;
        }

        public UserInfo UserInfoGet(int userID)
        {
            UserInfo userInfo = new UserInfo();
            List<UserInfo> userInfos = new List<UserInfo>();
            
            userInfo.UserID = userID;
            this.userResource.UserInfoGet(userInfo.UserID, this.UserInfoConverter, userInfos);

            return userInfos[0];
        }

        public UserInfo UserInfoCreate(UserInfo userInfo)
        {
            this.userResource.UserInfoCreate(userInfo.UserID, userInfo.Gender, userInfo.Preference, userInfo.Age, userInfo.PictureLocation, userInfo.OnlineStatusID.ToString(), userInfo.EmailAddress, userInfo.Handle, this.UserLoginConverter, userInfo);
                
            return userInfo;
        }

        public bool UserInstantTitleUpdate(string userid, string instantTitle)
        {
            
            this.userResource.UserInstantTitleUpdate(userid, instantTitle);
            return true;
        }

        public bool UserOnlineStatusUpdate(string userid, string status)
        {
            try
            {
                this.userResource.UserOnlineStatusUpdate(userid, short.Parse(status));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CheckinCreate(int userID, int businessID)
        {
            this.userResource.CheckinCreate(userID, businessID);
            return true;
        }

        public bool CheckoutCreate(int userID, int businessID)
        {
            this.userResource.CheckoutCreate(userID, businessID);
            return true;
        }
        
        public bool UserImageLocationUpdate(string userid, string imageURL)
        {
            this.userResource.UserImageLocationUpdate(userid, imageURL);
            return true;
            
        }

        public List<UserInfo> VisibleProfilesGet(string userID)
        {
            List<UserInfo> users = new List<UserInfo>();
            List<SelectedUser> usertypes = new List<SelectedUser>();
            Hashtable userhsh = new Hashtable();

            this.userResource.VisibleProfilesGet(userID, this.UserInfoConverter, users);
            if (users != null && users.Count > 0)
                users.ForEach(x =>
                                {
                                    userhsh[x.UserID] = x;
                                    //if(usertypes.Contains(x.UserID))
                                            
                                }
                            );

            //Get All Trashed
            MergeUserInfos(userID, userhsh, usertypes, UserSelectionType.MY);

            MergeUserInfos(userID, userhsh, usertypes, UserSelectionType.OTHERS);
            //GET ALL MISS
            //MergeUserInfos(userID, userhsh, usertypes, UserSelectionType.MISS, UIScanType.MISS);
            //GET ALL MISS
            //MergeUserInfos(userID, userhsh, usertypes, UserSelectionType.HIT, UIScanType.HIT);
            return users;
        }

        private void MergeUserInfos(string userID, Hashtable userhsh, List<SelectedUser> usertypes, UserSelectionType userSelectionType)
        {
            this.userResource.UserSelectionGet(userID, userSelectionType, this.UserSelectionConverter, usertypes);
            if (userSelectionType == UserSelectionType.MY)
            {
                if (usertypes != null && usertypes.Count > 0)
                {
                    usertypes.ForEach(x =>
                    {
                        if (userhsh.Contains(x.UserId))
                            ((UserInfo)userhsh[x.UserId]).InterestType = ConvertSelectionTypeToScanType((UserSelectionType)Enum.Parse(typeof(UserSelectionType), x.StatusId.ToString()), ((UserInfo)userhsh[x.UserId]).InterestType, userSelectionType);

                    }
                    );
                }
            }
            else if (userSelectionType == UserSelectionType.OTHERS)
            {
                if (usertypes != null && usertypes.Count > 0)
                {
                    usertypes.ForEach(x =>
                    {
                        if (userhsh.Contains(x.UserId))
                            ((UserInfo)userhsh[x.UserId]).InterestType = ConvertSelectionTypeToScanType((UserSelectionType)Enum.Parse(typeof(UserSelectionType), x.StatusId.ToString()), ((UserInfo)userhsh[x.UserId]).InterestType, userSelectionType);

                    }
                    );
                }
            }

            usertypes.Clear();
        }

        private UIScanType ConvertSelectionTypeToScanType(UserSelectionType selectionType, UIScanType scanType, UserSelectionType actionType)
        {
            if (actionType == UserSelectionType.MY)
            {
                if (selectionType == UserSelectionType.LIKE)
                    return UIScanType.LIKE;
                else if (selectionType == UserSelectionType.DISLIKE)
                    return UIScanType.TRASH;
                else
                    return UIScanType.BROWSE;
            }
            else
            {
                if (scanType == UIScanType.BROWSE)
                {
                    if (selectionType == UserSelectionType.LIKE)
                        return UIScanType.MISS;
                }
                else if (scanType == UIScanType.LIKE)
                {
                    if (selectionType == UserSelectionType.LIKE)
                        return UIScanType.HIT;
                }
                else if (scanType == UIScanType.TRASH)
                {
                    if (selectionType == UserSelectionType.LIKE)
                        return UIScanType.MISS;
                }
            }
            
            return scanType;
        }

        public bool UserSelectionUpdate(string fromUserID, string toUserID, UserSelectionType selectionType)
        {
            this.userResource.UserSelectionUpdate(fromUserID, toUserID, selectionType);
            return true;
        }
        
        public bool InterestAction(int fromUserID, int toUserID, InterestActionType actionType)
        {
            if (fromUserID == toUserID)
                return false;

            this.userResource.InterestAction(fromUserID, toUserID, actionType);
            return true;
        }

        public UserInfo[] UserSelectionGet(string userID, UserSelectionType userSelectionType)
        {
            List<int> users = new List<int>();
            this.userResource.UserSelectionGet(userID, userSelectionType, this.UserSelectionConverter, users);
            List<UserInfo> userinfos = VisibleProfilesGet(userID);
            if (userinfos != null)
                return userinfos.Where(x => users.Contains(x.UserID)).ToArray();
            else
                return null;
        }

        public Chat UserChatAdd(string fromUserID, string toUserID, string message)
        {
            Chat chat = new Chat() { FromUserId = int.Parse(fromUserID), ToUserId = int.Parse(toUserID), Message = message };

            this.userResource.UserChatAdd(fromUserID, toUserID, message, this.UserChatConverter, chat);

            return chat;
        }

        public Chat[] UserChatGet(string fromUserID, string toUserID, string chatId, string NoOfChat)
        {
            List<Chat> chats = new List<Chat>();
            this.userResource.UserChatGet(fromUserID, toUserID, int.Parse("0" + chatId).ToString(),int.Parse("0" + NoOfChat).ToString(), this.UserChatConverter, chats);

            return chats.ToArray();
        }

        private object UserChatConverter(IDataReader dr, object dataObject)
        {
            if (dataObject.GetType().Equals(typeof(List<Chat>)))
            {
                List<Chat> chats = dataObject as List<Chat>;
                if(dr["IsHigherMessage"].ToString().Equals("false",StringComparison.InvariantCultureIgnoreCase))
                    chats.Add(new Chat() {  FromUserId = int.Parse(dr["LowerUserId"].ToString()), 
                                            ToUserId = int.Parse(dr["HigherUserId"].ToString()), 
                                            Message = dr["UserMessage"].ToString(),
                                            ChatId = long.Parse(dr["ChatId"].ToString())
                                            });
                else
                    chats.Add(new Chat()
                    {
                        FromUserId = int.Parse(dr["HigherUserId"].ToString()),
                        ToUserId = int.Parse(dr["LowerUserId"].ToString()),
                        Message = dr["UserMessage"].ToString(),
                        ChatId = long.Parse(dr["ChatId"].ToString())
                    });

                return dataObject;
            }
            else
            {
                Chat chat = dataObject as Chat;
                if (chat != null)
                {
                    chat.ChatId = long.Parse(dr["ChatId"].ToString());
                }
                return chat;
            }

        }

        private object UserSelectionConverter(IDataReader dr, object dataObject)
        {
            if(dataObject.GetType().Equals(typeof(List<int>)))
            {
                List<int> users = dataObject as List<int>;
                users.Add(int.Parse(dr["ToUserId"].ToString()));
                return dataObject;
            }
            else
            {
                List<SelectedUser> users = dataObject as List<SelectedUser>;
                users.Add(new SelectedUser(int.Parse(dr["ToUserId"].ToString()), int.Parse(dr["StatusId"].ToString())));
                return dataObject;
            }
        }

        private object UserInfoConverter(IDataReader dr, object dataObject)
        {
            List<Entities.UserInfo> users = dataObject as List<Entities.UserInfo>;
            UserInfo user = new UserInfo();

            users.Add(user);
            
            user.UserID = int.Parse(dr["UserID"].ToString());
            user.PictureLocation = dr["PictureLocation"].ToString();
            user.OnlineStatusID = short.Parse(dr["OnlineStatusID"].ToString());
            user.InstantTitle = dr["InstantTitle"].ToString();
            user.Gender = (GenderType) Enum.Parse(typeof(GenderType), dr["Gender"].ToString());
            user.Preference = (PreferenceType) Enum.Parse(typeof(PreferenceType), dr["Preference"].ToString());
            user.Age = short.Parse(dr["Age"].ToString());
            user.InterestType = UIScanType.BROWSE;
            user.Handle = dr["Handle"].ToString();
            user.EmailAddress = dr["Email"].ToString();
            return dataObject;
        }

        private object UserLoginConverter(IDataReader dr, object dataObject)
        {
            UserLogin userLogin = dataObject as UserLogin;

            if (userLogin == null)
            {
                userLogin = new UserLogin();
                userLogin.ExternalID = dr.GetString(1);
                userLogin.UserSourceType = (SourceType)Enum.Parse(typeof(SourceType), dr[2].ToString());
                dataObject = userLogin;
            }
            //Console.WriteLine("before isnew");
            DataTable dt = dr.GetSchemaTable();
            if (dt.Columns.Contains("IsNEW"))
            {
                userLogin.IsNew = bool.Parse(dr["IsNEW"].ToString());
            }

            if (dt.Columns.Contains("Exists"))
            {
                userLogin.IsNew = bool.Parse(dr["Exists"].ToString());
            }
            userLogin.UserID = dr.GetInt32(0);
            userLogin.Password = string.Empty;

            return dataObject;
        }
    }
}
