using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backend.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Models.DTOs;
using Backend.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Backend.Utils;
using Backend.Models;
using Backend.Controllers.Backend.Controllers;

namespace Backend.Controllers.Tests
{
    [TestClass()]
    public class SubmissionControllerTests
    {
        [TestMethod()]
        public void GetSubmissionListTest()
        {
            var mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.GetSubmissionList(It.IsAny<SubmissionListGetModel>()))
                .ReturnsAsync(new SubmissionListModel());

            var mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var result = mockSubmissionController.GetSubmissionList(new SubmissionListGetModel()).Result;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.GetSubmissionList(It.IsAny<SubmissionListGetModel>()))
                .Throws<ExceptionWithDescription>();
            mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var resultEWD = mockSubmissionController.GetSubmissionList(new SubmissionListGetModel()).Result;

            Assert.IsInstanceOfType(resultEWD, typeof(BadRequestObjectResult));

            mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.GetSubmissionList(It.IsAny<SubmissionListGetModel>()))
                .Throws<UserNotAuthorized>();
            mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var resultUNA = mockSubmissionController.GetSubmissionList(new SubmissionListGetModel()).Result;

            Assert.IsInstanceOfType(resultUNA, typeof(UnauthorizedResult));
        }

        [TestMethod()]
        public void GetSubmissionTest()
        {
            var mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.GetSubmission(It.IsAny<int>()))
                .ReturnsAsync(new GetSusbmission());

            var mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var result = mockSubmissionController.GetSubmission(1).Result;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.GetSubmission(It.IsAny<int>()))
                .Throws<ExceptionWithDescription>();
            mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var resultEWD = mockSubmissionController.GetSubmission(1).Result;

            Assert.IsInstanceOfType(resultEWD, typeof(BadRequestObjectResult));

            mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.GetSubmission(It.IsAny<int>()))
                .Throws<UserNotAuthorized>();
            mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var resultUNA = mockSubmissionController.GetSubmission(1).Result;

            Assert.IsInstanceOfType(resultUNA, typeof(UnauthorizedResult));
        }

        [TestMethod()]
        public void GetOwnSubmissionTest()
        {
            var mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.GetOwnSubmission(It.IsAny<int>()))
                .ReturnsAsync(1);

            var mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var result = mockSubmissionController.GetOwnSubmission(1).Result;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.GetOwnSubmission(It.IsAny<int>()))
                .Throws<ExceptionWithDescription>();
            mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var resultEWD = mockSubmissionController.GetOwnSubmission(1).Result;

            Assert.IsInstanceOfType(resultEWD, typeof(BadRequestObjectResult));

            mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.GetOwnSubmission(It.IsAny<int>()))
                .Throws<UserNotAuthorized>();
            mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var resultUNA = mockSubmissionController.GetOwnSubmission(1).Result;

            Assert.IsInstanceOfType(resultUNA, typeof(UnauthorizedResult));
        }

        [TestMethod()]
        public void GetSubmissionTypeTest()
        {
            var mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.GetType(It.IsAny<int>()))
                .ReturnsAsync("");

            var mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var result = mockSubmissionController.GetSubmissionType(1).Result;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.GetType(It.IsAny<int>()))
                .Throws<ExceptionWithDescription>();
            mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var resultEWD = mockSubmissionController.GetSubmissionType(1).Result;

            Assert.IsInstanceOfType(resultEWD, typeof(BadRequestObjectResult));

            mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.GetType(It.IsAny<int>()))
                .Throws<UserNotAuthorized>();
            mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var resultUNA = mockSubmissionController.GetSubmissionType(1).Result;

            Assert.IsInstanceOfType(resultUNA, typeof(UnauthorizedResult));
        }

        [TestMethod()]
        public void VoteForSubmissionTest()
        {
            var mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.VoteForSubmission(It.IsAny<int>()))
                .ReturnsAsync(true);

            var mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var result = mockSubmissionController.VoteForSubmission(1).Result;

            Assert.IsInstanceOfType(result, typeof(OkResult));

            mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.VoteForSubmission(It.IsAny<int>()))
                .Throws<ExceptionWithDescription>();
            mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var resultEWD = mockSubmissionController.VoteForSubmission(1).Result;

            Assert.IsInstanceOfType(resultEWD, typeof(BadRequestObjectResult));

            mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.VoteForSubmission(It.IsAny<int>()))
                .Throws<UserNotAuthorized>();
            mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var resultUNA = mockSubmissionController.VoteForSubmission(1).Result;

            Assert.IsInstanceOfType(resultUNA, typeof(UnauthorizedResult));
        }

        [TestMethod()]
        public void UnVoteForSubmissionTest()
        {
            var mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.UnVoteForSubmission(It.IsAny<int>()))
                .ReturnsAsync(true);

            var mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var result = mockSubmissionController.UnVoteForSubmission(1).Result;

            Assert.IsInstanceOfType(result, typeof(OkResult));

            mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.UnVoteForSubmission(It.IsAny<int>()))
                .Throws<ExceptionWithDescription>();
            mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var resultEWD = mockSubmissionController.UnVoteForSubmission(1).Result;

            Assert.IsInstanceOfType(resultEWD, typeof(BadRequestObjectResult));

            mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.UnVoteForSubmission(It.IsAny<int>()))
                .Throws<UserNotAuthorized>();
            mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var resultUNA = mockSubmissionController.UnVoteForSubmission(1).Result;

            Assert.IsInstanceOfType(resultUNA, typeof(UnauthorizedResult));
        }

        [TestMethod()]
        public void CreateMovie_Test()
        {
            var mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.ModifyMovieSubmission(It.IsAny<ModifyMovieDetails>()))
                .ReturnsAsync(1);

            var mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var result = mockSubmissionController.CreateSubmission(new ModifyMovieDetails()).Result;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.ModifyMovieSubmission(It.IsAny<ModifyMovieDetails>()))
                .Throws<ExceptionWithDescription>();
            mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var resultEWD = mockSubmissionController.CreateSubmission(new ModifyMovieDetails()).Result;

            Assert.IsInstanceOfType(resultEWD, typeof(BadRequestObjectResult));

            mockSubmissionService = new Mock<SubmissionService>(null!, null!);
            mockSubmissionService.Setup(x => x.ModifyMovieSubmission(It.IsAny<ModifyMovieDetails>()))
                .Throws<UserNotAuthorized>();
            mockSubmissionController = new SubmissionController(mockSubmissionService.Object);

            var resultUNA = mockSubmissionController.CreateSubmission(new ModifyMovieDetails()).Result;

            Assert.IsInstanceOfType(resultUNA, typeof(UnauthorizedResult));
        }

    }

}

