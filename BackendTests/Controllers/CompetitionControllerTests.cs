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

namespace Backend.Controllers.Tests
{
    [TestClass()]
    public class CompetitionControllerTests
    {
        [TestMethod()]
        public void Create_Ok_Test()
        {
            var mockCompetitionService = new Mock<CompetitionService>(null!, null!);
            mockCompetitionService.Setup(x => x.CreateCompetition(It.IsAny<CreateCompetitionDTO>()))
                .ReturnsAsync(1);

            var mockCompetitionController = new CompetitionController(mockCompetitionService.Object);

            var result = mockCompetitionController.Create(new CreateCompetitionDTO()).Result;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod()]
        public void Create_EWD_Test()
        {
            var mockCompetitionService = new Mock<CompetitionService>(null!, null!);
            mockCompetitionService.Setup(x => x.CreateCompetition(It.IsAny<CreateCompetitionDTO>()))
                .Throws<ExceptionWithDescription>();

            var mockCompetitionController = new CompetitionController(mockCompetitionService.Object);

            var result = mockCompetitionController.Create(new CreateCompetitionDTO()).Result;

            mockCompetitionService.Verify(x => x.CreateCompetition(It.IsAny<CreateCompetitionDTO>()), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
        [TestMethod()]
        public async Task Create_UNA_Test()
        {
            var mockCompetitionService = new Mock<CompetitionService>(null!, null!);
            mockCompetitionService.Setup(x => x.CreateCompetition(It.IsAny<CreateCompetitionDTO>()))
                .Throws<UserNotAuthorized>();

            var mockCompetitionController = new CompetitionController(mockCompetitionService.Object);

            var result = mockCompetitionController.Create(new CreateCompetitionDTO()).Result;

            mockCompetitionService.Verify(x => x.CreateCompetition(It.IsAny<CreateCompetitionDTO>()), Times.Once);
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        [TestMethod()]
        public void GetById_Ok_Test()
        {
            var mockCompetitionService = new Mock<CompetitionService>(null!, null!);
            mockCompetitionService.Setup(x => x.CreateCompetition(It.IsAny<CreateCompetitionDTO>()))
                .ReturnsAsync(1);

            var mockCompetitionController = new CompetitionController(mockCompetitionService.Object);

            var result = mockCompetitionController.Create(new CreateCompetitionDTO()).Result;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod()]
        public void GetById_EWD_Test()
        {
            var mockCompetitionService = new Mock<CompetitionService>(null!, null!);
            mockCompetitionService.Setup(x => x.CreateCompetition(It.IsAny<CreateCompetitionDTO>()))
                .Throws<ExceptionWithDescription>();

            var mockCompetitionController = new CompetitionController(mockCompetitionService.Object);

            var result = mockCompetitionController.Create(new CreateCompetitionDTO()).Result;

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
        [TestMethod()]
        public void GetListDetails_OK_Test()
        {
            var mockCompetitionService = new Mock<CompetitionService>(null!, null!);
            mockCompetitionService.Setup(x => x.GetDetailsList(It.IsAny<GetCompetitionListDTO>()))
                .ReturnsAsync(new CompetitionList()
                {
                    Competitions = []
                });


            var mockCompetitionController = new CompetitionController(mockCompetitionService.Object);

            var result = mockCompetitionController.GetListDetails(new GetCompetitionListDTO()).Result;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }
        [TestMethod()]
        public void GetListDetails_EWD_Test()
        {
            var mockCompetitionService = new Mock<CompetitionService>(null!, null!);
            mockCompetitionService.Setup(x => x.GetDetailsList(It.IsAny<GetCompetitionListDTO>()))
                .ReturnsAsync(() => null);


            var mockCompetitionController = new CompetitionController(mockCompetitionService.Object);

            var result = mockCompetitionController.GetListDetails(new GetCompetitionListDTO()).Result;
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

        }
    }
}