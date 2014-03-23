using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Configuration;
using System.Text;
using System.IO;
using System.Diagnostics;
using Incyte;
using Incyte.Entities;
using Incyte.Services;
using Newtonsoft.Json;
namespace Delivery
{
    // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
    // a single instance of the service to process all calls.	
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    // NOTE: If the service is renamed, remember to update the global.asax.cs file

    public class UserDelivery : IUserDelivery  //: IBusinessServie
    {
        public UserInfo Get(string id)
        {
            try
            {
                UserService userService = new UserService();
                return userService.UserInfoGet(int.Parse(id));
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER GET " + e.Message);
                return null;
            }
        }

        public UserLogin UserLoginCreate(string email, string password1, string password2)
        {
            return new UserLogin();
        }

        public UserInfo UserInfoUpdate(string userid, string gender, string preference, string age, string email, string handle)
        {
            try
            {
                UserService userService = new UserService();
                UserInfo userInfo = new UserInfo()
                {
                    UserID = int.Parse(userid),
                    Age = short.Parse(age),
                    Preference = (PreferenceType)Enum.Parse(typeof(PreferenceType), preference),
                    Gender = (GenderType)Enum.Parse(typeof(GenderType), gender),
                    OnlineStatusID = 1,
                    Handle = handle,
                    EmailAddress = email
                };
                Logging.WriteToFileLog(string.Format("userid : {0}, gender : {1}, preference : {2}, age : {3}, email: {4}, handle: {5}", userid, gender, preference, age, email, handle));
                return userService.UserInfoCreate(userInfo);
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER UserInfoUpdate " + e.Message);
                return null;
            }
        }
        public UserInfo UserLoginInfoCreate(string externalid, string source, string gender, string preference, string age, string onlinestatus)
        {
            try
            {
                UserLogin userLogin = new UserLogin()
                                        {
                                            ExternalID = externalid,
                                            UserSourceType = (SourceType)Enum.Parse(typeof(SourceType), source)
                                        };
                UserService userService = new UserService();
                userLogin = userService.LoginCreate(userLogin);

                UserInfo userInfo = new UserInfo()
                                        {
                                            UserID = userLogin.UserID,
                                            Age = short.Parse(age),
                                            Preference = (PreferenceType)Enum.Parse(typeof(PreferenceType), preference),
                                            Gender = (GenderType)Enum.Parse(typeof(GenderType), gender),
                                            OnlineStatusID = short.Parse("0" + onlinestatus)
                                        };
                return userService.UserInfoCreate(userInfo);
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER UserLoginInfoCreate " + e.Message);
                return null;
            }

        }

        public UserInfo UserInfoCreate(string userid, string gender, string preference, string age, string onlinestatus, string handle)
        {
            try
            {
                UserService userService = new UserService();
                UserInfo userInfo = new UserInfo()
                {
                    UserID = int.Parse("0" + userid),
                    Age = short.Parse(age),
                    Preference = (PreferenceType)Enum.Parse(typeof(PreferenceType), preference),
                    Gender = (GenderType)Enum.Parse(typeof(GenderType), gender),
                    OnlineStatusID = short.Parse("0" + onlinestatus),
                    Handle = handle
                };
                return userService.UserInfoCreate(userInfo);
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER UserInfoCreate " + e.Message);
                return null;
            }

        }

        public UserLogin UserLoginCreate(string externalid, string source)
        {
            try
            {
                UserLogin userLogin = new UserLogin()
                {
                    ExternalID = externalid,
                    UserSourceType = (SourceType)Enum.Parse(typeof(SourceType), source)
                };
                UserService userService = new UserService();
                userLogin = userService.LoginCreate(userLogin);

                return userLogin;
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER UserLoginCreate " + e.Message);
                return null;
            }

        }

        public UserLogin LoginPasswordValidateCreate(string userName, string password)
        {
            try
            {
                UserLogin userLogin = new UserLogin()
                {
                    UserName = userName,
                    Password = password,
                    ExternalID = "0",
                    UserSourceType = SourceType.Internal
                };
                UserService userService = new UserService();
                userLogin = userService.LoginPasswordValidateCreate(userLogin);

                userLogin.Password = "";

                return userLogin;
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER LoginPasswordValidateCreate " + e.Message);
                return null;
            }

        }

        public UserLogin LoginPasswordValidateGet(string userName, string password)
        {
            try
            {
                UserLogin userLogin = new UserLogin()
                {
                    UserName = userName,
                    Password = password,
                    ExternalID = "0",
                    UserSourceType = SourceType.Internal
                };
                UserService userService = new UserService();
                userLogin = userService.LoginGet(userLogin);

                userLogin.Password = "";

                return userLogin;
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER LoginPasswordValidateCreate " + e.Message);
                return null;
            }

        }

        public bool LoginPasswordReset(string userName)
        {
            try
            {
                UserLogin userLogin = new UserLogin()
                {
                    UserName = userName,
                };
                UserService userService = new UserService();
                userLogin = userService.LoginPasswordReset(userLogin);

                return true;
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER LoginPasswordReset " + e.Message);
                return false;
            }

        }

        public bool Checkin(string userid, string businessid)
        {
            try
            {
                
                UserService userService = new UserService();
                Logging.WriteToFileLog("Status : " + (int)UserOnlineStatus.Online);
                userService.UserOnlineStatusUpdate(userid, ((int)UserOnlineStatus.Online).ToString());
                return userService.CheckinCreate(int.Parse(userid), int.Parse(businessid));
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER Checkin " + e.Message);
                return false;
            }
        }

        public bool Checkout(string userid)
        {
            try
            {
            UserService userService = new UserService();
            return userService.CheckoutCreate(int.Parse(userid),0);
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER Checkout " + e.Message);
                return false;
            }
        }

        public UserInfo[] Browse(string userid)
        {
            try
            {
                UserService userService = new UserService();
                return userService.VisibleProfilesGet(userid).ToArray();
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER BROWSE " + e.Message);
                return null;
            }
        }

        public UserInfo[] BrowseWithChat(string userid, string NoOfChat)
        {
            try
            {
                UserService userService = new UserService();
                UserInfo[] userInfos = userService.VisibleProfilesGet(userid).ToArray();

                if (userInfos != null)
                {
                    foreach(UserInfo userinfo in userInfos)
                    {
                        userinfo.Chats = userService.UserChatGet(userid, userinfo.UserID.ToString(), "0", NoOfChat);
                    }
                }
                return userInfos;
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER BROWSE " + e.Message);
                return null;
            }
        }

        public bool InstantTitleUpdate(string userid, string title)
        {
            try
            {
                UserService userService = new UserService();
                return userService.UserInstantTitleUpdate(userid, title);
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER InstantTitleUpdate " + e.Message);
                return false;
            }
        }

        public bool OnlineStatusUpdate(string userid, string status)
        {
            try
            {
                UserService userService = new UserService();
                return userService.UserOnlineStatusUpdate(userid, status);
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER OnlineStatusUpdate " + e.Message);
                return false;
            }
        }

        public bool Like(string fromUserID, string toUserID)
        {
            try
            {
                UserService userService = new UserService();
                return userService.UserSelectionUpdate(fromUserID, toUserID, UserSelectionType.LIKE);
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER Like  " + e.Message);
                return false;
            }
        }

        public bool DisLike(string fromUserID, string toUserID)
        {
            try
            {
                UserService userService = new UserService();
                return userService.UserSelectionUpdate(fromUserID, toUserID, UserSelectionType.DISLIKE);
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER DISLike  " + e.Message);
                return false;
            }
        }

        public UserInfo[] Hits(string UserID)
        {
            try
            {
                UserService userService = new UserService();
                return userService.UserSelectionGet(UserID, UserSelectionType.HIT);
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER HITS " + e.Message);
                return null;
            }
        }

        public UserInfo[] Miss(string UserID)
        {
            try
            {
                UserService userService = new UserService();
                return userService.UserSelectionGet(UserID, UserSelectionType.MISS);
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER MISS " + e.Message);
                return null;
            }
        }

        public UserInfo[] Likes(string UserID)
        {
            try
            {
                UserService userService = new UserService();
                return userService.UserSelectionGet(UserID, UserSelectionType.LIKE);
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER LIKESSS " + e.Message);
                return null;
            }
        }

        public UserInfo[] DisLikes(string UserID)
        {
            try
            {
                UserService userService = new UserService();
                return userService.UserSelectionGet(UserID, UserSelectionType.DISLIKE);
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER DISLIKESSS " + e.Message);
                return null;
            }
        }

        public long UserChatAdd(string FromUserID, string ToUserID, string Message)
        {
            try
            {
                Logging.WriteToFileLog("user chat add message : " + Message);
                UserService userService = new UserService();
                return userService.UserChatAdd(FromUserID, ToUserID, Message).ChatId;
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER UserChatAdd " + e.Message);
                return 0;
            }
        }

        public Chat[] UserChatGet(string FromUserID, string ToUserID, string ChatId, string NoOfChat)
        {
            try
            {
                UserService userService = new UserService();
                return userService.UserChatGet(FromUserID, ToUserID, ChatId, NoOfChat);
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog("USER UserChatAdd " + e.Message);
                return null;
            }
        }

        public void UploadFile(string userID,string ext)
        {
            
            try
            {
                
                string newFileName = Guid.NewGuid().ToString().Replace("-", "") + "." + ext;

                
                string uploadFolder = ConfigurationManager.ConnectionStrings["ImageLocation"].ConnectionString;

                
                string filePath = Path.Combine(uploadFolder, newFileName);
                string imgBaseUrl = ConfigurationManager.ConnectionStrings["ImageURL"].ConnectionString;
                string imgUrl = imgBaseUrl + "/" + newFileName;

                UserService userService = new UserService();
                userService.UserImageLocationUpdate(userID, imgUrl);
                
                
            }
            catch (Exception e)
            {
                Logging.WriteToEventLog(e.Message);
            }

        }

        public void UploadFile1(string userID, string ext, Stream fileContents)
        {

            try
            {
                    Logging.WriteToFileLog("starting...");

                    FileStream targetStream = null;
                    string newFileName = Guid.NewGuid().ToString().Replace("-", "") + "." + ext;

                    Logging.WriteToFileLog("newFileName = " + newFileName);

                    string uploadFolder = ConfigurationManager.ConnectionStrings["ImageLocation"].ConnectionString;

                    Logging.WriteToFileLog("uploadFolder = " + uploadFolder);

                    string filePath = Path.Combine(uploadFolder, newFileName);
                    string imgBaseUrl = ConfigurationManager.ConnectionStrings["ImageURL"].ConnectionString;
                    string imgUrl = imgBaseUrl + "/" + newFileName;

                    Logging.WriteToFileLog("filePath = " + filePath);
                    Logging.WriteToFileLog("imgBaseUrl = " + imgBaseUrl);
                    Logging.WriteToFileLog("imgUrl = " + imgUrl);

                    using (targetStream = new FileStream(filePath, FileMode.Create,
                                              FileAccess.Write, FileShare.None))
                    {
                        Logging.WriteToFileLog("inside targetstream");
                        //read from the input stream in 4K chunks
                        //and save to output stream
                        const int bufferLen = 4096;
                        byte[] buffer = new byte[bufferLen];
                        int count = 0;
                        Logging.WriteToFileLog("BEFORE LOOP");
                        Logging.WriteToFileLog("BUFFER LEN = " + bufferLen.ToString());
                        if(fileContents == null)
                            Logging.WriteToFileLog("fileContents IS NULL");
                        else
                            Logging.WriteToFileLog("fileContents IS NOT NULL");

                        count = fileContents.Read(buffer, 0, bufferLen);

                        Logging.WriteToFileLog("count = " + count);
                        while (count > 0)
                        {
                            Logging.WriteToFileLog("inside LOOP");
                            targetStream.Write(buffer, 0, count);
                            count = fileContents.Read(buffer, 0, bufferLen);
                        }
                    

                    UserService userService = new UserService();
                    Logging.WriteToFileLog("Before user pic loc update");
                    userService.UserImageLocationUpdate(userID, imgUrl);
                    Logging.WriteToFileLog("AFTER user pic loc update");
                }
            }
            catch (Exception e)
            {
                Logging.WriteToFileLog(e.Message);
                Logging.WriteToEventLog(e.Message);
            }

        }
    }
}
