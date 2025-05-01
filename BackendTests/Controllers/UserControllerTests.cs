using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backend.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Models.DTOs;
using Backend.Services;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Backend.Models;

namespace Backend.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        [TestMethod]
        public void Register_Ok_Test()
        {
            var mockUserService = new Mock<UserService>(null!, null!, null!, null!, null!);
            mockUserService.Setup(x => x.RegisterUser(It.IsAny<RegisterModel>()))
                .Returns(Task.CompletedTask);

            var controller = new UserController(mockUserService.Object);
            var result = controller.Register(new RegisterModel()).Result;

            mockUserService.Verify(x => x.RegisterUser(It.IsAny<RegisterModel>()), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void Register_EWD_Test()
        {
            var mockUserService = new Mock<UserService>(null!, null!, null!, null!, null!);
            mockUserService.Setup(x => x.RegisterUser(It.IsAny<RegisterModel>()))
                .Throws<ExceptionWithDescription>();

            var controller = new UserController(mockUserService.Object);
            var result = controller.Register(new RegisterModel()).Result;

            mockUserService.Verify(x => x.RegisterUser(It.IsAny<RegisterModel>()), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void Register_UNA_Test()
        {
            var mockUserService = new Mock<UserService>(null!, null!, null!, null!, null!);
            mockUserService.Setup(x => x.RegisterUser(It.IsAny<RegisterModel>()))
                .Throws<UserNotAuthorized>();

            var controller = new UserController(mockUserService.Object);
            var result = controller.Register(new RegisterModel()).Result;

            mockUserService.Verify(x => x.RegisterUser(It.IsAny<RegisterModel>()), Times.Once);
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public void UserById_Ok_Test()
        {
            var mockUserService = new Mock<UserService>(null!, null!, null!, null!, null!);
            mockUserService.Setup(x => x.GetUserByName(It.IsAny<string>()))
                .ReturnsAsync(new User { Id = 1, UserName = "testuser" });

            var controller = new UserController(mockUserService.Object);
            var result = controller.UserById("testuser").Result;

            mockUserService.Verify(x => x.GetUserByName("testuser"), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }


    }
}