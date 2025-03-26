using Backend.Models;
using Backend.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/competition")]
    [ApiController]
    public class CompetitionController
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly MariaDBContext _context;
        public CompetitionController(UserManager<User> userManager,
            MariaDBContext mariaDbContext,
                         IConfiguration configuration)
        {
            this._userManager = userManager;
            this._configuration = configuration;
            this._context = mariaDbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Competition>> GetAll()
        {
            return _context.Competitions.Where(competition => competition.IsPrivate == false).Skip(0);
        }
    }
}
