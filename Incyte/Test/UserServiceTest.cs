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
    public class UserServiceTest
    {
        [TestMethod]
        public void VisibleProfilesGet()
        {
            IUserResource userResource = new MockUserResource();
            IUserService userService = new UserService(userResource);
            List<UserInfo> users = userService.VisibleProfilesGet("1");
        }
        [TestMethod]
        public void CheckinCreate()
        {
            MockUserResource userResource = new MockUserResource();
            IUserService userService = new UserService(userResource);
            userService.CheckinCreate(1, 1);
            Assert.AreEqual(userResource.CheckedInUserID, 1);
        }
        [TestMethod]
        public void CheckoutCreate()
        {
            MockUserResource userResource = new MockUserResource();
            IUserService userService = new UserService(userResource);
            userService.CheckoutCreate(1, 1);
            Assert.AreEqual(userResource.CheckedOutUserID, 1);
        }
        [TestMethod]
        public void ShowInterest()
        {
            MockUserResource userResource = new MockUserResource();
            IUserService userService = new UserService(userResource);
            userService.InterestAction(1, 2, Incyte.InterestActionType.Link);
            Assert.AreEqual(userResource.ShowInterestFromUserID, 1);
            Assert.AreEqual(userResource.ShowInterestToUserID, 2);
        }
        [TestMethod]
        public void ShowInterestFail()
        {
            MockUserResource userResource = new MockUserResource();
            IUserService userService = new UserService(userResource);
            userService.InterestAction(1, 1,Incyte.InterestActionType.Link);
            Assert.AreEqual(userResource.ShowInterestFromUserID, 0);
            Assert.AreEqual(userResource.ShowInterestToUserID, 0);
        }
        [TestMethod]
        public void RevertInterest()
        {
            MockUserResource userResource = new MockUserResource();
            IUserService userService = new UserService(userResource);
            //userService.RevertInterest(1, 2);
            userService.InterestAction(1, 2, Incyte.InterestActionType.Revert);
            Assert.AreEqual(userResource.RevertInterestFromUserID, 1);
            Assert.AreEqual(userResource.RevertInterestToUserID, 2);
        }
        [TestMethod]
        public void RevertInterestFail()
        {
            MockUserResource userResource = new MockUserResource();
            IUserService userService = new UserService(userResource);
            //userService.RevertInterest(1, 1);
            userService.InterestAction(1, 1, Incyte.InterestActionType.Revert);
            Assert.AreEqual(userResource.RevertInterestFromUserID, 0);
            Assert.AreEqual(userResource.RevertInterestToUserID, 0);
        }
        [TestMethod]
        public void Hangup()
        {
            MockUserResource userResource = new MockUserResource();
            IUserService userService = new UserService(userResource);
            //userService.RevertInterest(1, 2);
            userService.InterestAction(1, 2, Incyte.InterestActionType.Hangup);
            Assert.AreEqual(userResource.HangupFromUserID, 1);
            Assert.AreEqual(userResource.HangupToUserID, 2);
        }
        [TestMethod]
        public void HangupFail()
        {
            MockUserResource userResource = new MockUserResource();
            IUserService userService = new UserService(userResource);
            //userService.RevertInterest(1, 1);
            userService.InterestAction(1, 1, Incyte.InterestActionType.Hangup);
            Assert.AreEqual(userResource.HangupFromUserID, 0);
            Assert.AreEqual(userResource.HangupToUserID, 0);
        }
    }
}
