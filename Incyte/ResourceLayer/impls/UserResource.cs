using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Incyte.Interfaces;
using System.Data.SqlClient;
using System.Data;

namespace Incyte.Resource
{
    public class UserResource : IUserResource
    {
        private readonly string connectionString = string.Empty;

        public UserResource(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void LoginCreate(string userName, string password, SourceType UserSourceType, string ExternalID, Converter converter, object dataObject)
        {
            using (DBAccess dbaccess = new DBAccess())
            {

                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@UserName";
                parameter1.Value = userName;
                parameter1.SqlDbType = SqlDbType.VarChar;

                SqlParameter parameter2 = new SqlParameter();
                parameter2.ParameterName = "@UserPassword";
                parameter2.Value = userName;
                parameter2.SqlDbType = SqlDbType.VarChar;

                SqlParameter parameter3 = new SqlParameter();
                parameter3.ParameterName = "@ExternalID";
                parameter3.Value = ExternalID;
                parameter3.SqlDbType = SqlDbType.NVarChar;

                SqlParameter parameter4 = new SqlParameter();
                parameter4.ParameterName = "@SourceID";
                parameter4.Value = UserSourceType;
                parameter4.SqlDbType = SqlDbType.Int;

                SqlParameter[] parameters = new SqlParameter[4] { parameter1, parameter2, parameter3, parameter4 };

                SqlDataReader reader = dbaccess.ExecuteProcedure("LoginCreate", this.connectionString, parameters);
                while (reader.Read())
                {
                    converter(reader, dataObject);
                }

            }
        }

        public void LoginPasswordValidateCreate(string userName, string password, Converter converter, object dataObject)
        {
            using (DBAccess dbaccess = new DBAccess())
            {

                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@UserName";
                parameter1.Value = userName;
                parameter1.SqlDbType = SqlDbType.NVarChar;

                SqlParameter parameter2 = new SqlParameter();
                parameter2.ParameterName = "@UserPassword";
                parameter2.Value = userName;
                parameter2.SqlDbType = SqlDbType.NVarChar;

                SqlParameter[] parameters = new SqlParameter[2] { parameter1, parameter2 };

                SqlDataReader reader = dbaccess.ExecuteProcedure("LoginPasswordValidateCreate", this.connectionString, parameters);
                while (reader.Read())
                {
                    converter(reader, dataObject);
                }

            }
        }

        public void LoginPasswordGet(string userName, string password, Converter converter, object dataObject)
        {
            using (DBAccess dbaccess = new DBAccess())
            {

                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@UserName";
                parameter1.Value = userName;
                parameter1.SqlDbType = SqlDbType.NVarChar;

                SqlParameter parameter2 = new SqlParameter();
                parameter2.ParameterName = "@UserPassword";
                parameter2.Value = userName;
                parameter2.SqlDbType = SqlDbType.NVarChar;

                SqlParameter[] parameters = new SqlParameter[2] { parameter1, parameter2 };

                SqlDataReader reader = dbaccess.ExecuteProcedure("LoginPasswordGet", this.connectionString, parameters);
                while (reader.Read())
                {
                    converter(reader, dataObject);
                }

            }
        }

        public void LoginPasswordValidateReset(string userid, string userName, string password)
        {
            using (DBAccess dbaccess = new DBAccess())
            {

                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@UserName";
                parameter1.Value = userName;
                parameter1.SqlDbType = SqlDbType.VarChar;

                SqlParameter parameter2 = new SqlParameter();
                parameter2.ParameterName = "@UserPassword";
                parameter2.Value = userName;
                parameter2.SqlDbType = SqlDbType.VarChar;

                SqlParameter parameter3 = new SqlParameter();
                parameter3.ParameterName = "@UserId";
                parameter3.Value = userid;
                parameter3.SqlDbType = SqlDbType.Int;

                SqlParameter[] parameters = new SqlParameter[3] { parameter1, parameter2, parameter3 };

                SqlDataReader reader = dbaccess.ExecuteProcedure("LoginPasswordReset", this.connectionString, parameters);

            }
        }

        public void UserInfoGet(int userID, Converter converter, object dataObject)
        {
            using (DBAccess dbaccess = new DBAccess())
            {

                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@UserId";
                parameter1.Value = userID;
                parameter1.SqlDbType = SqlDbType.Int;

                SqlParameter[] parameters = new SqlParameter[1] { parameter1 };

                SqlDataReader reader = dbaccess.ExecuteProcedure("UserInfoGet", this.connectionString, parameters);
                while (reader.Read())
                {
                    converter(reader, dataObject);
                }

            }
        }

        public void UserInfoCreate(int userID, GenderType gender, PreferenceType preferenceType, short age, string pictureLocation, string status, string email, string handle, Converter converter, object dataObject)
        {
            using (DBAccess dbaccess = new DBAccess())
            {

                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@UserID";
                parameter1.Value = userID;
                parameter1.SqlDbType = SqlDbType.Int;

                SqlParameter parameter2 = new SqlParameter();
                parameter2.ParameterName = "@Gender";
                parameter2.Value = gender;
                parameter2.SqlDbType = SqlDbType.SmallInt;

                SqlParameter parameter3 = new SqlParameter();
                parameter3.ParameterName = "@Preference";
                parameter3.Value = preferenceType;
                parameter3.SqlDbType = SqlDbType.SmallInt;

                SqlParameter parameter4 = new SqlParameter();
                parameter4.ParameterName = "@Age";
                parameter4.Value = age;
                parameter4.SqlDbType = SqlDbType.SmallInt;

                //SqlParameter parameter5 = new SqlParameter();
                //parameter5.ParameterName = "@PictureLocation";
                //parameter5.Value = pictureLocation;
                //parameter5.SqlDbType = SqlDbType.VarChar;


                SqlParameter parameter6 = new SqlParameter();
                parameter6.ParameterName = "@Status";
                parameter6.Value = status;
                parameter6.SqlDbType = SqlDbType.SmallInt;

                SqlParameter parameter7 = new SqlParameter();
                parameter7.ParameterName = "@Email";
                parameter7.Value = email;
                parameter7.SqlDbType = SqlDbType.NVarChar;

                SqlParameter parameter8 = new SqlParameter();
                parameter8.ParameterName = "@Handle";
                parameter8.Value = handle;
                parameter8.SqlDbType = SqlDbType.VarChar;

                SqlParameter[] parameters = new SqlParameter[7] { parameter1, parameter2, parameter3, parameter4, parameter6, parameter7, parameter8 };

                SqlDataReader reader = dbaccess.ExecuteProcedure("UserInfoCreate", this.connectionString, parameters);

                if (converter != null)
                {
                    while (reader.Read())
                    {
                        converter(reader, dataObject);
                    }
                }

            }
        }

        public void UserInstantTitleUpdate(string userid, string instantTitle)
        {
            using (DBAccess dbaccess = new DBAccess())
            {

                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@UserID";
                parameter1.Value = userid;
                parameter1.SqlDbType = SqlDbType.Int;

                SqlParameter parameter2 = new SqlParameter();
                parameter2.ParameterName = "@Title";
                parameter2.Value = instantTitle;
                parameter2.SqlDbType = SqlDbType.NVarChar;
                
                SqlParameter[] parameters = new SqlParameter[2] { parameter1, parameter2 };

                SqlDataReader reader = dbaccess.ExecuteProcedure("UserTitleUpdate", this.connectionString, parameters);

            }
        }


        public void UserImageLocationUpdate(string userid, string imageURL)
        {
            Logging.WriteToFileLog(string.Format("INTO UserImageLocationUpdate for user id : {0} & picloc : {1}",userid,imageURL));
            using (DBAccess dbaccess = new DBAccess())
            {

                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@UserID";
                parameter1.Value = userid;
                parameter1.SqlDbType = SqlDbType.Int;

                SqlParameter parameter2 = new SqlParameter();
                parameter2.ParameterName = "@ImageURL";
                parameter2.Value = imageURL;
                parameter2.SqlDbType = SqlDbType.VarChar;

                SqlParameter[] parameters = new SqlParameter[2] { parameter1, parameter2 };

                SqlDataReader reader = dbaccess.ExecuteProcedure("UserImageURLUpdate", this.connectionString, parameters);

            }
            Logging.WriteToFileLog("Done updating");
        }
        public void UserOnlineStatusUpdate(string userid, short status)
        {
            using (DBAccess dbaccess = new DBAccess())
            {

                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@UserID";
                parameter1.Value = userid;
                parameter1.SqlDbType = SqlDbType.Int;

                SqlParameter parameter2 = new SqlParameter();
                parameter2.ParameterName = "@StatusId";
                parameter2.Value = status;
                parameter2.SqlDbType = SqlDbType.SmallInt;

                SqlParameter[] parameters = new SqlParameter[2] { parameter1, parameter2 };

                SqlDataReader reader = dbaccess.ExecuteProcedure("UserOnlineStatusUpdate", this.connectionString, parameters);

            }
        }

        public void CheckinCreate(int userID, int businessID)
        {
            using (DBAccess dbaccess = new DBAccess())
            {

                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@UserID";
                parameter1.Value = userID;
                parameter1.SqlDbType = SqlDbType.Int;

                SqlParameter parameter2 = new SqlParameter();
                parameter2.ParameterName = "@BusinessID";
                parameter2.Value = businessID;
                parameter2.SqlDbType = SqlDbType.Int;

                SqlParameter[] parameters = new SqlParameter[2] { parameter1, parameter2};

                SqlDataReader reader = dbaccess.ExecuteProcedure("CheckIn", this.connectionString, parameters);
            }
        }

        public void CheckoutCreate(int userID, int businessID)
        {
            using (DBAccess dbaccess = new DBAccess())
            {

                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@UserID";
                parameter1.Value = userID;
                parameter1.SqlDbType = SqlDbType.Int;

                SqlParameter parameter2 = new SqlParameter();
                parameter2.ParameterName = "@BusinessID";
                parameter2.Value = businessID;
                parameter2.SqlDbType = SqlDbType.Int;

                SqlParameter parameter3 = new SqlParameter();
                parameter3.ParameterName = "@Gender";
                parameter3.Value = 0;
                parameter3.SqlDbType = SqlDbType.Int;

                SqlParameter[] parameters = new SqlParameter[3] { parameter1, parameter2, parameter3 };

                SqlDataReader reader = dbaccess.ExecuteProcedure("CheckOut", this.connectionString, parameters);
            }
        }

        public void VisibleProfilesGet(string userID, Converter converter, object dataObject)
        {
            using (DBAccess dbaccess = new DBAccess())
            {

                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@UserId";
                parameter1.Value = userID;
                parameter1.SqlDbType = SqlDbType.Int;

                SqlParameter[] parameters = new SqlParameter[1] { parameter1 };

                SqlDataReader reader = dbaccess.ExecuteProcedure("VisibleProfilesByBusinessGet", this.connectionString, parameters);
                while (reader.Read())
                {
                    converter(reader, dataObject);
                }

            }
        }

        public void UserSelectionUpdate(string fromUserID, string toUserID, UserSelectionType selectionType)
        {
            using (DBAccess dbaccess = new DBAccess())
            {
                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@FromUserID";
                parameter1.Value = fromUserID;
                parameter1.SqlDbType = SqlDbType.Int;

                SqlParameter parameter2 = new SqlParameter();
                parameter2.ParameterName = "@ToUserID";
                parameter2.Value = toUserID;
                parameter2.SqlDbType = SqlDbType.Int;

                SqlParameter parameter3 = new SqlParameter();
                parameter3.ParameterName = "@StatusId";
                parameter3.Value = selectionType;
                parameter3.SqlDbType = SqlDbType.SmallInt;
                SqlParameter[] parameters = new SqlParameter[3] { parameter1, parameter2, parameter3 };

                SqlDataReader reader = dbaccess.ExecuteProcedure("UserSelectionUpdate", this.connectionString, parameters);
            }
        }

        public void UserSelectionGet(string userID, UserSelectionType userSelectionType, Converter converter, object dataObject)
        {
            using (DBAccess dbaccess = new DBAccess())
            {

                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@UserId";
                parameter1.Value = userID;
                parameter1.SqlDbType = SqlDbType.Int;

                SqlParameter parameter2 = new SqlParameter();
                parameter2.ParameterName = "@TypeId";
                parameter2.Value = userSelectionType;
                parameter2.SqlDbType = SqlDbType.Int;

                SqlParameter[] parameters = new SqlParameter[2] { parameter1, parameter2 };

                SqlDataReader reader = dbaccess.ExecuteProcedure("UserSelectionGet", this.connectionString, parameters);
                while (reader.Read())
                {
                    converter(reader, dataObject);
                }

            }
        }

        public void UserChatGet(string fromUserID, string toUserID, string chatId, string NoOfChat, Converter converter, object dataObject)
        {
            using (DBAccess dbaccess = new DBAccess())
            {

                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@FromUserId";
                parameter1.Value = fromUserID;
                parameter1.SqlDbType = SqlDbType.Int;

                SqlParameter parameter2 = new SqlParameter();
                parameter2.ParameterName = "@ToUserId";
                parameter2.Value = toUserID;
                parameter2.SqlDbType = SqlDbType.Int;

                SqlParameter parameter3 = new SqlParameter();
                parameter3.ParameterName = "@ChatId";
                parameter3.Value = chatId;
                parameter3.SqlDbType = SqlDbType.BigInt;

                SqlParameter parameter4 = new SqlParameter();
                parameter4.ParameterName = "@NoOfChat";
                parameter4.Value = NoOfChat;
                parameter4.SqlDbType = SqlDbType.Int;
                Logging.WriteToFileLog("no of chat: " + NoOfChat);
                SqlParameter[] parameters = new SqlParameter[4] { parameter1, parameter2, parameter3, parameter4 };

                SqlDataReader reader = dbaccess.ExecuteProcedure("UserChatGet", this.connectionString, parameters);
                while (reader.Read())
                {
                    converter(reader, dataObject);
                }

            }
        }

        public void UserChatAdd(string fromUserID, string toUserID, string message, Converter converter, object dataObject)
        {
            using (DBAccess dbaccess = new DBAccess())
            {

                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@FromUserId";
                parameter1.Value = fromUserID;
                parameter1.SqlDbType = SqlDbType.Int;

                SqlParameter parameter2 = new SqlParameter();
                parameter2.ParameterName = "@ToUserId";
                parameter2.Value = toUserID;
                parameter2.SqlDbType = SqlDbType.Int;

                SqlParameter parameter3 = new SqlParameter();
                parameter3.ParameterName = "@Message";
                parameter3.Value = message;
                parameter3.SqlDbType = SqlDbType.NVarChar;

                SqlParameter[] parameters = new SqlParameter[3] { parameter1, parameter2, parameter3 };

                SqlDataReader reader = dbaccess.ExecuteProcedure("UserChatAdd", this.connectionString, parameters);
                while (reader.Read())
                {
                    converter(reader, dataObject);
                }

            }
        }

        public void InterestAction(int fromUserID, int toUserID, InterestActionType actionType)
        {
            throw new NotImplementedException();
        }

    }
}
