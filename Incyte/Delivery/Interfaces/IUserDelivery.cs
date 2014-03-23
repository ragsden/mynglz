using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Incyte;
using Incyte.Entities;
using Incyte.Services;
using System.IO;

namespace Delivery
{
    // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
    // a single instance of the service to process all calls.	
    [ServiceContract]
    // NOTE: If the service is renamed, remember to update the global.asax.cs file
    public interface IUserDelivery
    {
        // TODO: Implement the collection resource that will contain the SampleItem instances

        [WebGet(UriTemplate = "{id}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        UserInfo Get(string id);

        [WebGet(UriTemplate = "checkin/{userid}/{businessid}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool Checkin(string userid, string businessid);

        [WebGet(UriTemplate = "checkout/{userid}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool Checkout(string userid);

        [WebGet(UriTemplate = "browse/{userid}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        UserInfo[] Browse(string userid);

        [WebGet(UriTemplate = "allz/{userid}/{NoOfChat}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        UserInfo[] BrowseWithChat(string userid, string NoOfChat);

        [WebGet(UriTemplate = "title/{userid}/{title}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool InstantTitleUpdate(string userid,string title);

        [WebGet(UriTemplate = "status/{userid}/{status}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool OnlineStatusUpdate(string userid, string status);

        [WebGet(UriTemplate = "add/{externalid}/{source}/{gender}/{preference}/{age}/{onlinestatus}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        UserInfo UserLoginInfoCreate(string externalid, string source, string gender, string preference, string age, string onlinestatus);

        [WebGet(UriTemplate = "ui/{userid}/{gender}/{preference}/{age}/{onlinestatus}/{handle}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        UserInfo UserInfoCreate(string userid, string gender, string preference, string age, string onlinestatus, string handle);

        //[WebGet(UriTemplate = "reg/{externalid}/{source}/{gender}/{preference}/{age}/{onlinestatus}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        //UserLogin UserLoginCreate_old(string email, string password1, string password2);

        [WebGet(UriTemplate = "update/{userid}/{gender}/{preference}/{age}/{email}/{handle}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        UserInfo UserInfoUpdate(string userid, string gender, string preference, string age, string email, string handle);

        [WebGet(UriTemplate = "login/{externalid}/{source}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        UserLogin UserLoginCreate(string externalid, string source);

        [WebGet(UriTemplate = "log/{userName}/{password}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        UserLogin LoginPasswordValidateGet(string userName, string password);

        [WebGet(UriTemplate = "logpvc/{userName}/{password}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        UserLogin LoginPasswordValidateCreate(string userName, string password);

        [WebGet(UriTemplate = "logpvr/{username}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool LoginPasswordReset(string username);

        [WebGet(UriTemplate = "like/{fromUserID}/{toUserID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool Like(string fromUserID, string toUserID);

        [WebGet(UriTemplate = "dislike/{fromUserID}/{toUserID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool DisLike(string fromUserID, string toUserID);

        [WebGet(UriTemplate = "hits/{UserID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        UserInfo[] Hits(string UserID);

        [WebGet(UriTemplate = "miss/{UserID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        UserInfo[] Miss(string UserID);

        [WebGet(UriTemplate = "likes/{UserID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        UserInfo[] Likes(string UserID);

        [WebGet(UriTemplate = "dislikes/{UserID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        UserInfo[] DisLikes(string UserID);

        [OperationContract, WebInvoke(Method = "POST", UriTemplate = "upload/{userID}/{ext}", BodyStyle = WebMessageBodyStyle.Bare)]
        void UploadFile(string userID, string ext);

        [OperationContract, WebInvoke(Method = "POST", UriTemplate = "upload1/{userID}/{ext}", BodyStyle = WebMessageBodyStyle.Bare)]
        void UploadFile1(string userID, string ext, Stream fileContents);

        [WebGet(UriTemplate = "chat/add/{FromUserID}/{ToUserID}/{Message}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        long UserChatAdd(string FromUserID, string ToUserID, string Message);

        [WebGet(UriTemplate = "chat/{FromUserID}/{ToUserID}/{ChatId}/{NoOfChat}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        Chat[] UserChatGet(string FromUserID, string ToUserID, string ChatId, string NoOfChat);
    }
}
