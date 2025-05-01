using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Backend.Models;
using Backend.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Backend.Models.JoinTables;
using Microsoft.EntityFrameworkCore.InMemory;
using Backend.Utils;
using Backend.Models.DTOs;
using BackendTests;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services.Tests
{
    [TestClass()]
    public class CompetitionServiceTests
    {

        [TestMethod]
        public async Task CreateTest()
        {
            var mockOptions = new DbContextOptionsBuilder<MariaDBContext>()
                .UseInMemoryDatabase(databaseName: "testCreateCompetition")
                .Options;
            var mockUser = new User() { Id = 10, UserName = "testUser", Email = "test@test.com" };

            var mockUserService = new Mock<UserService>(null!, null!, null!, null!, null!);
            mockUserService.Setup(x => x.GetLoggedInUser()).ReturnsAsync(mockUser);


            using (var context = new MariaDBContext(mockOptions))
            {
                CompetitionService mockCompetititonService = new CompetitionService(mockUserService.Object, context);

                Assert.IsTrue(mockCompetititonService.CreateCompetition(new CreateCompetitionDTO()
                {
                    Title = "testtesttest",
                    Description = "Test123456789101112131415",
                    Category = "movie",
                    SubmissionStartDate = DateOnly.FromDateTime(DateTime.Now).AddDays(10),
                    VotingStartDate = DateOnly.FromDateTime(DateTime.Now).AddDays(2),
                    FinishDate = DateOnly.FromDateTime(DateTime.Now).AddDays(3)

                }).IsFaulted);

                var result = await mockCompetititonService.CreateCompetition(new CreateCompetitionDTO()
                {
                    Title = "testtesttest",
                    Description = "Test123456789101112131415",
                    Category = "movie",
                    SubmissionStartDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1),
                    VotingStartDate = DateOnly.FromDateTime(DateTime.Now).AddDays(2),
                    FinishDate = DateOnly.FromDateTime(DateTime.Now).AddDays(3)

                });
                Assert.AreEqual(1, result);
            }
        }

        [TestMethod()]
        public async Task GetDetails_Test()
        {
            var mockOptions = new DbContextOptionsBuilder<MariaDBContext>()
                    .UseInMemoryDatabase(databaseName: "TestGetDetails")
                    .Options;
            var mockSubmission = new Submission() { Id = 1, CompetitionId = 1, Title = "test", Description = "test" };
            var mockCollabs = new CollaboratedSubmissions() { SubmissionId = 1, UserId = 1, Submission = mockSubmission };
            var mockUser = new User() { Id = 1, UserName = "testUser", Email = "test@test.com" };

            var mockUserService = new Mock<UserService>(null!, null!, null!, null!, null!);
            mockUserService.Setup(x => x.GetLoggedInUser()).ReturnsAsync(mockUser);

            using (var context = new MariaDBContext(mockOptions))
            {
                context.Competitions.Add(new Competition() { Id = 1, CreatorId = 1, Title = "test", IsInPreparationPhase = true });
                context.Moderators.Add(new Moderators() { UserId = 1, CompetitionId = 1 });
                context.Submissions.Add(mockSubmission);
                context.CollaboratedSubmissions.Add(mockCollabs);
                context.Users.Add(mockUser);
                context.SaveChanges();
            }

            using (var context = new MariaDBContext(mockOptions))
            {
                CompetitionService mockCompetititonService = new CompetitionService(mockUserService.Object, context);
                await Assert.ThrowsExceptionAsync<ExceptionWithDescription>(() => mockCompetititonService.GetDetails(0));
                var result = await mockCompetititonService.GetDetails(1);
                Assert.IsNotNull(result);
                Assert.AreEqual(result.Id, 1);
                Assert.AreEqual(result.Title, "test");
                Assert.AreEqual(result.OwnSubmission, 1);
            }

        }


        [TestMethod()]
        public async Task IsInPreparationPhase_Test()
        {
            var mockOptions = new DbContextOptionsBuilder<MariaDBContext>()
                    .UseInMemoryDatabase(databaseName: "TestIsInPreparationPhase")
                    .Options;

            using (var context = new MariaDBContext(mockOptions))
            {
                context.Competitions.Add(new Competition() { Id = 1, IsInPreparationPhase = true });
                context.Competitions.Add(new Competition() { Id = 2, IsInPreparationPhase = false });
                context.SaveChanges();
            }

            using (var context = new MariaDBContext(mockOptions))
            {
                var mockCompetititonService = new CompetitionService(null!, context);
                var result = await mockCompetititonService.IsInPreparationPhase(1);
                Assert.IsTrue(result);
            }

            using (var context = new MariaDBContext(mockOptions))
            {
                var mockCompetititonService = new CompetitionService(null!, context);
                var result = await mockCompetititonService.IsInPreparationPhase(2);
                Assert.IsFalse(result);
            }
        }

        [TestMethod()]
        public async Task SubmissionEnabled_Test()
        {
            var mockOptions = new DbContextOptionsBuilder<MariaDBContext>()
                    .UseInMemoryDatabase(databaseName: "TestSubmissionEnabled")
                    .Options;

            using (var context = new MariaDBContext(mockOptions))
            {
                context.Competitions.Add(new Competition() { Id = 1, IsInSubmissionPhase = true });
                context.Competitions.Add(new Competition() { Id = 2, IsInSubmissionPhase = false });
                context.SaveChanges();
            }

            using (var context = new MariaDBContext(mockOptions))
            {
                var mockCompetititonService = new CompetitionService(null!, context);
                var result = await mockCompetititonService.SubmissionEnabled(1);
                Assert.IsTrue(result);
            }

            using (var context = new MariaDBContext(mockOptions))
            {
                var mockCompetititonService = new CompetitionService(null!, context);
                var result = await mockCompetititonService.SubmissionEnabled(2);
                Assert.IsFalse(result);
            }
        }

        [TestMethod()]
        public async Task VotesEnabled_Test()
        {
            var mockOptions = new DbContextOptionsBuilder<MariaDBContext>()
                .UseInMemoryDatabase(databaseName: "TestVotingEnabled")
                .Options;

            using (var context = new MariaDBContext(mockOptions))
            {
                context.Competitions.Add(new Competition() { Id = 1, IsInVotingPhase = true });
                context.Competitions.Add(new Competition() { Id = 2, IsInVotingPhase = false });
                context.SaveChanges();
            }

            using (var context = new MariaDBContext(mockOptions))
            {
                var mockCompetititonService = new CompetitionService(null!, context);
                var result = await mockCompetititonService.VotesEnabled(1);
                Assert.IsTrue(result);
            }

            using (var context = new MariaDBContext(mockOptions))
            {
                var mockCompetititonService = new CompetitionService(null!, context);
                var result = await mockCompetititonService.VotesEnabled(2);
                Assert.IsFalse(result);
            }
        }

        [TestMethod()]
        public async Task IsModerator_Test()
        {
            var mockModerator = new Moderators
            {
                UserId = 1,
                CompetitionId = 1,
            };
            var mockOptions = new DbContextOptionsBuilder<MariaDBContext>()
                .UseInMemoryDatabase(databaseName: "TestIsModerator")
                .Options;

            using (var context = new MariaDBContext(mockOptions))
            {
                context.Moderators.Add(mockModerator);
                context.SaveChanges();
            }

            using (var context = new MariaDBContext(mockOptions))
            {
                var mockCompetititonService = new CompetitionService(null!, context);
                var result = await mockCompetititonService.IsModerator(1, 1);
                Assert.IsTrue(result);
            }
            using (var context = new MariaDBContext(mockOptions))
            {
                var mockCompetititonService = new CompetitionService(null!, context);
                var result = await mockCompetititonService.IsModerator(2, 1);
                Assert.IsFalse(result);
            }
        }
    }
}