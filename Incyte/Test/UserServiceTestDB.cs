using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Incyte.Entities;
using Incyte.Services;
using Incyte.Resource;
using Incyte.Interfaces;
using Test.mocks;

namespace Test
{
    [TestClass]
    public class UserServiceTestDB
    {
        [TestMethod]
        public void UserLoginCreateDB()
        {
            //IUserResource userResource = new MockUserResource();
            IUserService userService = new UserService();
            int i = 2;
            UserLogin ul = new UserLogin();
            ul.ExternalID = "wwwwwwwwww" + i.ToString();
            ul.UserSourceType = Incyte.SourceType.Facebook;
            ul.UserName = "";
            ul.Password = "";
            //userService.LoginCreate(ul);
        }

        [TestMethod]
        public void UserInfoCreateDB()
        {
            //IUserResource userResource = new MockUserResource();
            IUserService userService = new UserService();
            UserInfo userInfo = new UserInfo();
            userInfo.UserID = CreateUserLogin();
            userInfo.PictureLocation = "unit test";
            userInfo.Gender = Incyte.GenderType.MALE;
            userInfo.Age = 35;
            userInfo.Preference = Incyte.PreferenceType.FEMALE;
            userService.UserInfoCreate(userInfo);
        }
        [TestMethod]
        public void UserInfoFemaleCreateDB()
        {
            //IUserResource userResource = new MockUserResource();
            IUserService userService = new UserService();
            UserInfo userInfo = new UserInfo();
            userInfo.UserID = CreateUserLogin();
            userInfo.PictureLocation = "unit test";
            userInfo.Gender = Incyte.GenderType.FEMALE;
            userInfo.Age = 35;
            userInfo.Preference = Incyte.PreferenceType.MALE;
            userService.UserInfoCreate(userInfo);
        }
        [TestMethod]
        public void CheckinCreateDB()
        {
            IUserService userService = new UserService();
            userService.CheckinCreate(1, 1);
            userService.CheckinCreate(2, 2);
        }
        [TestMethod]
        public void CheckoutCreateDB()
        {
            IUserService userService = new UserService();
            userService.CheckoutCreate(1, 0);
            userService.CheckoutCreate(2, 0);
        }

        [TestMethod]
        public void GetVisibleProfilesDB()
        {
            IUserService userService = new UserService();
            userService.VisibleProfilesGet("1");
            
        }

        [TestMethod]
        public void UserChatAddDB()
        {
            IUserService userService = new UserService();
            Chat chat = userService.UserChatAdd("45","3","xxyy");

        }

        [TestMethod]
        public void UserChatGetDB()
        {
            IUserService userService = new UserService();
            Chat[] chats = userService.UserChatGet("2", "1", "0", "0");

        }
        private int CreateUserLogin()
        {
            //IUserResource userResource = new MockUserResource();
            IUserService userService = new UserService();
            int i = new Random().Next(1000);
            UserLogin ul = new UserLogin();
            ul.ExternalID = "wwwwwwwwww" + i.ToString();
            ul.UserSourceType = Incyte.SourceType.Facebook;
            ul.UserName = "";
            ul.Password = "";
            userService.LoginCreate(ul);
            return ul.UserID;

        }


    }
}