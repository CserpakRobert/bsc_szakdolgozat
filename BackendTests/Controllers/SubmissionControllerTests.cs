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
