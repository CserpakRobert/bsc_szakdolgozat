using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTests
{
    public static class  MockClasses
    {

        public static User TestUser1 => new() {Id =1, UserName="testUser1", Email = "testEmail1", PasswordHash="testpwd" };
        public static User TestUser2 => new() {Id =2, UserName="testUser2", Email = "testEmail2", PasswordHash="testpwd" };

        public static Competition TestCompetitionprepare(int id) => new() { Id = id, Title = "testTitle", Description = "testDescription", IsInPreparationPhase = true };
        public static Competition TestCompetitionsubmit(int id) => new() { Id = id, Title = "testTitle", Description = "testDescription", IsInSubmissionPhase = true };
        public static Competition TestCompetitionvoting(int id) => new() { Id = id, Title = "testTitle", Description = "testDescription", IsInVotingPhase = true };
        public static Competition TestCompetitionFinished(int id) => new() { Id = id, Title = "testTitle", Description = "testDescription" };
    }
}
