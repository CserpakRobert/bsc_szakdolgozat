using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using Backend.Utils;

namespace Backend.Services.Tests
{
    [TestClass()]
    public class SubmissionServiceTests
    {
        [TestMethod()]
        public async Task GetSubmissionListTest()
        {
            var mockOptions = new DbContextOptionsBuilder<MariaDBContext>()
                .UseInMemoryDatabase(databaseName: "testGetSubmissionList")
                .Options;
            var mockUser = new User() { Id = 10, UserName = "testUser", Email = "test@test.com" };

            var mockUserService = new Mock<UserService>(null!, null!, null!, null!, null!);
            mockUserService.Setup(x => x.GetLoggedInUser()).ReturnsAsync(mockUser);

            using (var context = new MariaDBContext(mockOptions))
            {
                context.Competitions.Add(new Competition() { Id = 1, IsInPreparationPhase = true });
                context.Submissions.Add(new Submission() { Id = 1, Owner = mockUser, Title="", Description = "", CompetitionId = 1 });
                context.SaveChanges();


                var submissionService = new SubmissionService(mockUserService.Object, context);
                var result = await submissionService.GetSubmissionList(new Models.DTOs.SubmissionListGetModel()
                {

                });
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Submissions.ToList()[0].Id);
            }
        }

        [TestMethod()]
        public async Task GetSubmissionTest()
        {
            var mockOptions = new DbContextOptionsBuilder<MariaDBContext>()
                .UseInMemoryDatabase(databaseName: "testGetSubmission")
                .Options;
            var mockUser = new User() { Id = 10, UserName = "testUser", Email = "test@test.com" };

            var mockUserService = new Mock<UserService>(null!, null!, null!, null!, null!);
            mockUserService.Setup(x => x.GetLoggedInUser()).ReturnsAsync(mockUser);

            using (var context = new MariaDBContext(mockOptions))
            {
                context.Competitions.Add(new Competition() { Id = 1, IsInPreparationPhase = true });
                context.Submissions.Add(new Submission() { Id = 1, Owner = mockUser, Title = "", Description = "", CompetitionId = 1 });
                context.SaveChanges();


                var submissionService = new SubmissionService(mockUserService.Object, context);
                var result = await submissionService.GetSubmission(1);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Id);

                await Assert.ThrowsExceptionAsync<ExceptionWithDescription>(() => submissionService.GetSubmission(2));
            }
        }

        [TestMethod()]
        public async Task VoteForSubmissionTest()
        {
            var mockOptions = new DbContextOptionsBuilder<MariaDBContext>()
            .UseInMemoryDatabase(databaseName: "testVoteForSubmission")
            .Options;
            var mockUser = new User() { Id = 10, UserName = "testUser", Email = "test@test.com" };
            var mockUser2 = new User() { Id = 11, UserName = "testUser", Email = "test@test.com" };

            var mockUserService = new Mock<UserService>(null!, null!, null!, null!, null!);
            mockUserService.Setup(x => x.GetLoggedInUser()).ReturnsAsync(mockUser);

            using (var context = new MariaDBContext(mockOptions))
            {
                context.Competitions.Add(new Competition() { Id = 1, IsInVotingPhase = true });
                context.Users.Add(mockUser);
                context.Users.Add(mockUser2);
                context.Submissions.Add(new Submission() { Id = 1, Owner = mockUser2, Title = "", Description = "", CompetitionId = 1 });
                context.Submissions.Add(new Submission() { Id = 2, Owner = mockUser2, Title = "", Description = "", CompetitionId = 1 });
                context.SubmissionVotes.Add(new Models.JoinTables.SubmissionVotes() { SubmissionId = 2, UserId = 10 });
                context.SaveChanges();


                var submissionService = new SubmissionService(mockUserService.Object, context);
                var result = await submissionService.VoteForSubmission(1);
                Assert.IsNotNull(result);
                Assert.IsTrue(result);

                await Assert.ThrowsExceptionAsync<ExceptionWithDescription>(() => submissionService.VoteForSubmission(2));
            }
        }

        [TestMethod()]
        public async Task HasVoteTest()
        {
            var mockOptions = new DbContextOptionsBuilder<MariaDBContext>()
                .UseInMemoryDatabase(databaseName: "testHasVote")
                .Options;
            var mockUser = new User() { Id = 10, UserName = "testUser", Email = "test@test.com" };
            var mockUser2 = new User() { Id = 11, UserName = "testUser", Email = "test@test.com" };

            var mockUserService = new Mock<UserService>(null!, null!, null!, null!, null!);
            mockUserService.Setup(x => x.GetLoggedInUser()).ReturnsAsync(mockUser);

            using (var context = new MariaDBContext(mockOptions))
            {
                context.Competitions.Add(new Competition() { Id = 1, IsInVotingPhase = true });
                context.Users.Add(mockUser);
                context.Users.Add(mockUser2);
                context.Submissions.Add(new Submission() { Id = 1, Owner = mockUser2, Title = "", Description = "", CompetitionId = 1 });
                context.Submissions.Add(new Submission() { Id = 2, Owner = mockUser2, Title = "", Description = "", CompetitionId = 1 });
                context.SubmissionVotes.Add(new Models.JoinTables.SubmissionVotes() { SubmissionId = 2, UserId = 10 });
                context.SaveChanges();


                var submissionService = new SubmissionService(mockUserService.Object, context);
                var result = await submissionService.HasVote(2);
                Assert.IsNotNull(result);
                Assert.IsTrue(result);

                var result2 = await submissionService.HasVote(1);
                Assert.IsNotNull(result2);
                Assert.IsFalse(result2);
            }
        }

        [TestMethod()]
        public async Task ModifyMovieSubmissionTest()
        {
            var mockOptions = new DbContextOptionsBuilder<MariaDBContext>()
                .UseInMemoryDatabase(databaseName: "testModifyMovieSubmission")
                .Options;
            var mockUser = new User() { Id = 10, UserName = "testUser", Email = "test@test.com" };
            var mockUser2 = new User() { Id = 11, UserName = "testUser", Email = "test@test.com" };

            var mockUserService = new Mock<UserService>(null!, null!, null!, null!, null!);
            mockUserService.Setup(x => x.GetLoggedInUser()).ReturnsAsync(mockUser);

            using (var context = new MariaDBContext(mockOptions))
            {
                context.Competitions.Add(new Competition() { Id = 1, IsInSubmissionPhase = true });
                context.Users.Add(mockUser);
                context.Users.Add(mockUser2);
                context.Submissions.Add(new Submission() { Id = 1, Owner = mockUser, Title = "", Description = "", CompetitionId = 1, IsSubmitted = false });
                context.Submissions.Add(new Submission() { Id = 2, Owner = mockUser, Title = "", Description = "", CompetitionId = 1, IsSubmitted = false });
                context.SubmissionVotes.Add(new Models.JoinTables.SubmissionVotes() { SubmissionId = 2, UserId = 10 });
                context.CollaboratedSubmissions.Add(new Models.JoinTables.CollaboratedSubmissions() { SubmissionId = 1, UserId = 11 });
                context.SaveChanges();


                var submissionService = new SubmissionService(mockUserService.Object, context);
                var insert = new Models.DTOs.ModifyMovieDetails()
                {
                    Id = 1,
                    Title = "",
                    Description = "testtestestestesstetststestestestes",
                    MovieLink = "",
                    TrailerLink = "nsl7pv3Ndxw"
                };
                await Assert.ThrowsExceptionAsync<ExceptionWithDescription>(() => submissionService.ModifyMovieSubmission(insert));

                insert.Title = "testtesttesttest";
                await Assert.ThrowsExceptionAsync<ExceptionWithDescription>(() => submissionService.ModifyMovieSubmission(insert));

                insert.MovieLink = "nsl7pv3Ndxw";
                var result = await submissionService.ModifyMovieSubmission(insert);
                Assert.AreEqual(1, result);
                
            }
        }

        [TestMethod()]
        public async Task ModifyMusicSubmissionTest()
        {
            var mockOptions = new DbContextOptionsBuilder<MariaDBContext>()
                .UseInMemoryDatabase(databaseName: "testModifyMusicSubmission")
                .Options;
            var mockUser = new User() { Id = 10, UserName = "testUser", Email = "test@test.com" };
            var mockUser2 = new User() { Id = 11, UserName = "testUser", Email = "test@test.com" };

            var mockUserService = new Mock<UserService>(null!, null!, null!, null!, null!);
            mockUserService.Setup(x => x.GetLoggedInUser()).ReturnsAsync(mockUser);

            using (var context = new MariaDBContext(mockOptions))
            {
                context.Competitions.Add(new Competition() { Id = 1, IsInSubmissionPhase = true });
                context.Users.Add(mockUser);
                context.Users.Add(mockUser2);
                context.Submissions.Add(new Submission() { Id = 1, Owner = mockUser, Title = "", Description = "", CompetitionId = 1, IsSubmitted = false });
                context.Submissions.Add(new Submission() { Id = 2, Owner = mockUser, Title = "", Description = "", CompetitionId = 1, IsSubmitted = false });
                context.SubmissionVotes.Add(new Models.JoinTables.SubmissionVotes() { SubmissionId = 2, UserId = 10 });
                context.CollaboratedSubmissions.Add(new Models.JoinTables.CollaboratedSubmissions() { SubmissionId = 1, UserId = 11 });
                context.SaveChanges();


                var submissionService = new SubmissionService(mockUserService.Object, context);
                var insert = new Models.DTOs.ModifyMusicDetails()
                {
                    Id = 1,
                    Title = "",
                    Description = "testtestestestesstetststestestestes",
                    Link = "",
                };
                await Assert.ThrowsExceptionAsync<ExceptionWithDescription>(() => submissionService.ModifyMusicSubmission(insert));

                insert.Title = "testtesttesttest";
                await Assert.ThrowsExceptionAsync<ExceptionWithDescription>(() => submissionService.ModifyMusicSubmission(insert));

                insert.Link = "nsl7pv3Ndxw";

                var result = await submissionService.ModifyMusicSubmission(insert);
                Assert.AreEqual(1, result);

            }
        }

        [TestMethod()]
        public async Task ModifyShortStorySubmissionTest()
        {
            var mockOptions = new DbContextOptionsBuilder<MariaDBContext>()
                .UseInMemoryDatabase(databaseName: "testModifyShortStorySubmission")
                .Options;
            var mockUser = new User() { Id = 10, UserName = "testUser", Email = "test@test.com" };
            var mockUser2 = new User() { Id = 11, UserName = "testUser", Email = "test@test.com" };

            var mockUserService = new Mock<UserService>(null!, null!, null!, null!, null!);
            mockUserService.Setup(x => x.GetLoggedInUser()).ReturnsAsync(mockUser);

            using (var context = new MariaDBContext(mockOptions))
            {
                context.Competitions.Add(new Competition() { Id = 1, IsInSubmissionPhase = true });
                context.Users.Add(mockUser);
                context.Users.Add(mockUser2);
                context.Submissions.Add(new Submission() { Id = 1, Owner = mockUser, Title = "", Description = "", CompetitionId = 1, IsSubmitted = false });
                context.Submissions.Add(new Submission() { Id = 2, Owner = mockUser, Title = "", Description = "", CompetitionId = 1, IsSubmitted = false });
                context.SubmissionVotes.Add(new Models.JoinTables.SubmissionVotes() { SubmissionId = 2, UserId = 10 });
                context.CollaboratedSubmissions.Add(new Models.JoinTables.CollaboratedSubmissions() { SubmissionId = 1, UserId = 11 });
                context.SaveChanges();


                var submissionService = new SubmissionService(mockUserService.Object, context);
                var insert = new Models.DTOs.ModifyShortStoryDetails()
                {
                    Id = 1,
                    Title = "",
                    Description = "testtestestestesstetststestestestes",
                    Story = "",
                };
                await Assert.ThrowsExceptionAsync<ExceptionWithDescription>(() => submissionService.ModifyShortStorySubmission(insert));

                insert.Title = "testtesttesttest";
                await Assert.ThrowsExceptionAsync<ExceptionWithDescription>(() => submissionService.ModifyShortStorySubmission(insert));

                insert.Story = "nsl7pv3Ndxwsdkjlfsdklfjsdaklfasdjklésdajfsdklaéfjsdfklsdjfé";

                var result = await submissionService.ModifyShortStorySubmission(insert);
                Assert.AreEqual(1, result);

            }
        }
    }
}