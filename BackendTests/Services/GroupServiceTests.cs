using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.Utils;

namespace Backend.Services.Tests
{
    [TestClass()]
    public class GroupServiceTests
    {
        [TestMethod()]
        public async Task CanJoinTest()
        {

            var mockOptions = new DbContextOptionsBuilder<MariaDBContext>()
                .UseInMemoryDatabase(databaseName: "testCanJoin")
                .Options;
            var mockUser = new User() { Id = 10, UserName = "testUser", Email = "test@test.com" };

            var mockUserService = new Mock<UserService>(null!, null!, null!, null!, null!);
            mockUserService.Setup(x => x.GetLoggedInUser()).ReturnsAsync(mockUser);

            using (var context = new MariaDBContext(mockOptions))
            {
                context.Competitions.Add(new Competition() { Id = 1, IsInPreparationPhase = true });
                context.Competitions.Add(new Competition() { Id = 2, IsInPreparationPhase = true });
                context.Users.Add(mockUser);
                context.Moderators.Add(new Models.JoinTables.Moderators() {User = mockUser, CompetitionId = 1});
                context.SaveChanges();


                var groupservice = new GroupService(mockUserService.Object, context);
                var result = await groupservice.CanJoin(1,10);
                Assert.IsNotNull(result);
                Assert.IsFalse(result);

                var result2 = await groupservice.CanJoin(2,10);
                Assert.IsNotNull(result2);
                Assert.IsTrue(result2);
            }
        }

        [TestMethod()]
        public async Task GroupSmallDetailsTest()
        {
            var mockOptions = new DbContextOptionsBuilder<MariaDBContext>()
                .UseInMemoryDatabase(databaseName: "testsmallDetails")
                .Options;
            var mockUser = new User() { Id = 10, UserName = "testUser", Email = "test@test.com" };

            var mockUserService = new Mock<UserService>(null!, null!, null!, null!, null!);
            mockUserService.Setup(x => x.GetLoggedInUser()).ReturnsAsync(mockUser);

            using (var context = new MariaDBContext(mockOptions))
            {
                context.Competitions.Add(new Competition() { Id = 1, IsInPreparationPhase = true });
                context.Groups.Add(new Group() { Id = 1, GroupOwner = mockUser, Description = "", CompetitionId = 1 });
                context.SaveChanges();


                var groupservice = new GroupService(mockUserService.Object, context);
                var result = await groupservice.GroupSmallDetails(1, 0);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Groups.ToList()[0].GroupId);
            }
        }

        [TestMethod()]
        public async Task IsOwnerTest()
        {
            var mockOptions = new DbContextOptionsBuilder<MariaDBContext>()
                .UseInMemoryDatabase(databaseName: "testIsOwner")
                .Options;
            var mockUser = new User() { Id = 10, UserName = "testUser", Email = "test@test.com" };
            var mockUser2 = new User() { Id = 11, UserName = "testUser2", Email = "test@test2.com" };

            var mockUserService = new Mock<UserService>(null!, null!, null!, null!, null!);
            mockUserService.Setup(x => x.GetLoggedInUser()).ReturnsAsync(mockUser);

            using (var context = new MariaDBContext(mockOptions))
            {
                context.Competitions.Add(new Competition() { Id = 1, IsInPreparationPhase = true });
                context.Groups.Add(new Group() { Id = 1, GroupOwner = mockUser, Description = "", CompetitionId = 1 });
                context.Groups.Add(new Group() { Id = 2, GroupOwner = mockUser2, Description = "", CompetitionId = 1 });
                context.SaveChanges();


                var groupservice = new GroupService(mockUserService.Object, context);
                var result = await groupservice.IsOwner(1);
                Assert.IsNotNull(result);
                Assert.IsTrue(result);

                var result2 = await groupservice.IsOwner(2);
                Assert.IsFalse(result2);

            }
        }

        [TestMethod()]
        public async Task GetGroupsCompetitionTest()
        {
            var mockOptions = new DbContextOptionsBuilder<MariaDBContext>()
            .UseInMemoryDatabase(databaseName: "testGetGroupsCompetition")
            .Options;
            var mockUser = new User() { Id = 10, UserName = "testUser", Email = "test@test.com" };

            var mockUserService = new Mock<UserService>(null!, null!, null!, null!, null!);
            mockUserService.Setup(x => x.GetLoggedInUser()).ReturnsAsync(mockUser);

            using (var context = new MariaDBContext(mockOptions))
            {
                context.Competitions.Add(new Competition() { Id = 1, IsInPreparationPhase = true });
                context.Groups.Add(new Group() { Id = 1, GroupOwner = mockUser, Description = "", CompetitionId = 1 });
                context.SaveChanges();


                var groupservice = new GroupService(mockUserService.Object, context);
                var result = await groupservice.GetGroupsCompetition(1);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result);
            }
        }

        [TestMethod()]
        public async Task CreateTest()
        {
            var mockOptions = new DbContextOptionsBuilder<MariaDBContext>()
             .UseInMemoryDatabase(databaseName: "testcreategroup")
             .Options;
            var mockUser = new User() { Id = 10, UserName = "testUser", Email = "test@test.com" };

            var mockUserService = new Mock<UserService>(null!, null!, null!, null!, null!);
            mockUserService.Setup(x => x.GetLoggedInUser()).ReturnsAsync(mockUser);

            using (var context = new MariaDBContext(mockOptions))
            {
                context.Competitions.Add(new Competition() { Id = 1, IsInPreparationPhase = true });
                context.Groups.Add(new Group() { Id = 1, GroupOwner = mockUser, Description = "", CompetitionId = 1 });
                context.SaveChanges();


                var groupservice = new GroupService(mockUserService.Object, context);
                var result = await groupservice.Create(1,
                    new Models.DTOs.CreateGroupDTO()
                    {
                        Description = "dsdafsdafsdfas",
                        IsClosed = false
                    });
                Assert.IsNotNull(result);
                Assert.AreEqual(2, result);

                await Assert.ThrowsExceptionAsync<ExceptionWithDescription>(() => groupservice.Create(1,
                    new Models.DTOs.CreateGroupDTO()
                    ));

            }
        }

        [TestMethod()]
        public async Task GetByIdTest()
        {
            var mockOptions = new DbContextOptionsBuilder<MariaDBContext>()
                .UseInMemoryDatabase(databaseName: "testsgetbyid")
                .Options;
            var mockUser = new User() { Id = 10, UserName = "testUser", Email = "test@test.com" };

            var mockUserService = new Mock<UserService>(null!, null!, null!, null!, null!);
            mockUserService.Setup(x => x.GetLoggedInUser()).ReturnsAsync(mockUser);

            using (var context = new MariaDBContext(mockOptions))
            {
                context.Competitions.Add(new Competition() { Id = 1, IsInPreparationPhase = true });
                context.Groups.Add(new Group() { Id = 1, GroupOwner = mockUser, Description = "", CompetitionId = 1 });
                context.SaveChanges();


                var groupservice = new GroupService(mockUserService.Object, context);
                var result = await groupservice.GetById(1);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.GroupId);

               await Assert.ThrowsExceptionAsync<ExceptionWithDescription>(() => groupservice.GetById(2));
            }
        }
    }
}