using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backend.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Controllers.Backend.Controllers;
using Backend.Models.DTOs;
using Backend.Services;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Backend.Controllers.Tests
{
    [TestClass()]
    public class GroupControllerTests
    {

        [TestMethod()]
        public void CreateClosedTest()
        {
            var mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(new GetGroupDTO());

            var mockGroupController = new GroupController(mockGroupService.Object);

            var result = mockGroupController.CreateClosed(1).Result;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.GetById(It.IsAny<int>()))
                .Throws<ExceptionWithDescription>();
            mockGroupController = new GroupController(mockGroupService.Object);

            var resultEWD = mockGroupController.CreateClosed(1).Result;

            Assert.IsInstanceOfType(resultEWD, typeof(BadRequestObjectResult));

            mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.GetById(It.IsAny<int>()))
                .Throws<UserNotAuthorized>();
            mockGroupController = new GroupController(mockGroupService.Object);

            var resultUNA = mockGroupController.CreateClosed(1).Result;

            Assert.IsInstanceOfType(resultUNA, typeof(UnauthorizedResult));
        }

        [TestMethod()]
        public void GetPrivateGroupsTest()
        {
            var mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.GetPrivateGroups(It.IsAny<int>()))
                .ReturnsAsync(true);

            var mockGroupController = new GroupController(mockGroupService.Object);

            var result = mockGroupController.GetPrivateGroups(1).Result;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.GetPrivateGroups(It.IsAny<int>()))
                .Throws<ExceptionWithDescription>();
            mockGroupController = new GroupController(mockGroupService.Object);

            var resultEWD = mockGroupController.GetPrivateGroups(1).Result;

            Assert.IsInstanceOfType(resultEWD, typeof(BadRequestObjectResult));

            mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.GetPrivateGroups(It.IsAny<int>()))
                .Throws<UserNotAuthorized>();
            mockGroupController = new GroupController(mockGroupService.Object);

            var resultUNA = mockGroupController.GetPrivateGroups(1).Result;

            Assert.IsInstanceOfType(resultUNA, typeof(UnauthorizedResult));
        }

        [TestMethod()]
        public void GetInvitedGroupsTest()
        {
            var mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.GetInvitedGroups(It.IsAny<int>()))
                .ReturnsAsync(true);

            var mockGroupController = new GroupController(mockGroupService.Object);

            var result = mockGroupController.GetInvitedGroups(1).Result;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.GetInvitedGroups(It.IsAny<int>()))
                .Throws<ExceptionWithDescription>();
            mockGroupController = new GroupController(mockGroupService.Object);

            var resultEWD = mockGroupController.GetInvitedGroups(1).Result;

            Assert.IsInstanceOfType(resultEWD, typeof(BadRequestObjectResult));

            mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.GetInvitedGroups(It.IsAny<int>()))
                .Throws<UserNotAuthorized>();
            mockGroupController = new GroupController(mockGroupService.Object);

            var resultUNA = mockGroupController.GetInvitedGroups(1).Result;

            Assert.IsInstanceOfType(resultUNA, typeof(UnauthorizedResult));
        }

        [TestMethod()]
        public void GetJoinedGroupsTest()
        {
            var mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.GetJoinedGroups(It.IsAny<int>()))
                .ReturnsAsync(true);

            var mockGroupController = new GroupController(mockGroupService.Object);

            var result = mockGroupController.GetJoinedGroups(1).Result;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.GetJoinedGroups(It.IsAny<int>()))
                .Throws<ExceptionWithDescription>();
            mockGroupController = new GroupController(mockGroupService.Object);

            var resultEWD = mockGroupController.GetJoinedGroups(1).Result;

            Assert.IsInstanceOfType(resultEWD, typeof(BadRequestObjectResult));

            mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.GetJoinedGroups(It.IsAny<int>()))
                .Throws<UserNotAuthorized>();
            mockGroupController = new GroupController(mockGroupService.Object);

            var resultUNA = mockGroupController.GetJoinedGroups(1).Result;

            Assert.IsInstanceOfType(resultUNA, typeof(UnauthorizedResult));
        }

        [TestMethod()]
        public void GetSmallDetailsTest()
        {
            var mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.GetOwnGroup(It.IsAny<int>()))
                .ReturnsAsync(1);

            var mockGroupController = new GroupController(mockGroupService.Object);

            var result = mockGroupController.GetSmallDetails(1).Result;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.GetOwnGroup(It.IsAny<int>()))
                .Throws<ExceptionWithDescription>();
            mockGroupController = new GroupController(mockGroupService.Object);

            var resultEWD = mockGroupController.GetSmallDetails(1).Result;

            Assert.IsInstanceOfType(resultEWD, typeof(BadRequestObjectResult));

            mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.GetOwnGroup(It.IsAny<int>()))
                .Throws<UserNotAuthorized>();
            mockGroupController = new GroupController(mockGroupService.Object);

            var resultUNA = mockGroupController.GetSmallDetails(1).Result;

            Assert.IsInstanceOfType(resultUNA, typeof(UnauthorizedResult));
        }


        [TestMethod()]
        public void InviteTest()
        {
            var mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.Invite(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var mockGroupController = new GroupController(mockGroupService.Object);

            var result = mockGroupController.Invite(1, new InviteDTO()).Result;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.Invite(It.IsAny<int>(), It.IsAny<string>()))
                .Throws<ExceptionWithDescription>();
            mockGroupController = new GroupController(mockGroupService.Object);

            var resultEWD = mockGroupController.Invite(1, new InviteDTO()).Result;

            Assert.IsInstanceOfType(resultEWD, typeof(BadRequestObjectResult));

            mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.Invite(It.IsAny<int>(), It.IsAny<string>()))
                .Throws<UserNotAuthorized>();
            mockGroupController = new GroupController(mockGroupService.Object);

            var resultUNA = mockGroupController.Invite(1,new InviteDTO()).Result;

            Assert.IsInstanceOfType(resultUNA, typeof(UnauthorizedResult));
        }

        [TestMethod()]
        public void IsInvitedTest()
        {
            var mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.IsInvited(It.IsAny<int>()))
                .ReturnsAsync(true);

            var mockGroupController = new GroupController(mockGroupService.Object);

            var result = mockGroupController.IsInvited(1).Result;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.IsInvited(It.IsAny<int>()))
                .Throws<ExceptionWithDescription>();
            mockGroupController = new GroupController(mockGroupService.Object);

            var resultEWD = mockGroupController.IsInvited(1).Result;

            Assert.IsInstanceOfType(resultEWD, typeof(BadRequestObjectResult));

            mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.IsInvited(It.IsAny<int>()))
                .Throws<UserNotAuthorized>();
            mockGroupController = new GroupController(mockGroupService.Object);

            var resultUNA = mockGroupController.IsInvited(1).Result;

            Assert.IsInstanceOfType(resultUNA, typeof(UnauthorizedResult));
        }

        [TestMethod()]
        public void DeleteInviteTest()
        {
            var mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.DeleteInvite(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var mockGroupController = new GroupController(mockGroupService.Object);

            var result = mockGroupController.DeleteInvite(1, new InviteDTO()).Result;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.DeleteInvite(It.IsAny<int>(), It.IsAny<string>()))
                .Throws<ExceptionWithDescription>();
            mockGroupController = new GroupController(mockGroupService.Object);

            var resultEWD = mockGroupController.DeleteInvite(1, new InviteDTO()).Result;

            Assert.IsInstanceOfType(resultEWD, typeof(BadRequestObjectResult));

            mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.DeleteInvite(It.IsAny<int>(), It.IsAny<string>()))
                .Throws<UserNotAuthorized>();
            mockGroupController = new GroupController(mockGroupService.Object);

            var resultUNA = mockGroupController.DeleteInvite(1, new InviteDTO()).Result;

            Assert.IsInstanceOfType(resultUNA, typeof(UnauthorizedResult));
        }

        [TestMethod()]
        public void AcceptInviteTest()
        {
            var mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.AcceptInvite(It.IsAny<int>()))
                .ReturnsAsync(true);

            var mockGroupController = new GroupController(mockGroupService.Object);

            var result = mockGroupController.AcceptInvite(1).Result;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.AcceptInvite(It.IsAny<int>()))
                .Throws<ExceptionWithDescription>();
            mockGroupController = new GroupController(mockGroupService.Object);

            var resultEWD = mockGroupController.AcceptInvite(1).Result;

            Assert.IsInstanceOfType(resultEWD, typeof(BadRequestObjectResult));

            mockGroupService = new Mock<GroupService>(null!, null!);
            mockGroupService.Setup(x => x.AcceptInvite(It.IsAny<int>()))
                .Throws<UserNotAuthorized>();
            mockGroupController = new GroupController(mockGroupService.Object);

            var resultUNA = mockGroupController.AcceptInvite(1).Result;

            Assert.IsInstanceOfType(resultUNA, typeof(UnauthorizedResult));
        }
    }
}