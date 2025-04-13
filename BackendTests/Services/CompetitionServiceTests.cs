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

namespace Backend.Services.Tests
{
    [TestClass()]
    public class CompetitionServiceTests
    {
        [TestMethod()]
        public void GetDetailsTest()
        {
            var mockUser = new User { Id = 1, UserName = "TestUser" };

            var mockCompetition = new Competition
            {
                Id = 1,
                Title = "Test Competition",
                Description = "Test Description",
                Creator = mockUser,
                IsInPreparationPhase = true,
                IsInSubmissionPhase = false,
                IsInVotingPhase = false,
            };


            Assert.Fail();
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
                var mockCompetititonService = new CompetitionService(null!, null!, context);
                var result = await mockCompetititonService.IsInPreparationPhase(1);
                Assert.IsTrue(result);
            }

            using (var context = new MariaDBContext(mockOptions))
            {
                var mockCompetititonService = new CompetitionService(null!, null!, context);
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
                var mockCompetititonService = new CompetitionService(null!, null!, context);
                var result = await mockCompetititonService.SubmissionEnabled(1);
                Assert.IsTrue(result);
            }

            using (var context = new MariaDBContext(mockOptions))
            {
                var mockCompetititonService = new CompetitionService(null!, null!, context);
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
                var mockCompetititonService = new CompetitionService(null!, null!, context);
                var result = await mockCompetititonService.VotesEnabled(1);
                Assert.IsTrue(result);
            }

            using (var context = new MariaDBContext(mockOptions))
            {
                var mockCompetititonService = new CompetitionService(null!, null!, context);
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
                var mockCompetititonService = new CompetitionService(null!, null!, context);
                var result = await mockCompetititonService.IsModerator(1, 1);
                Assert.IsTrue(result);
            }
            using (var context = new MariaDBContext(mockOptions))
            {
                var mockCompetititonService = new CompetitionService(null!, null!, context);
                var result = await mockCompetititonService.IsModerator(2, 1);
                Assert.IsFalse(result);
            }
        }
    }
}