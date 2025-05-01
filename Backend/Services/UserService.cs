
using Backend.Models;
using Backend.Models.DTOs;
using Backend.Persistence;
using Backend.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Backend.Services
{
    public class UserService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly MariaDBContext _context;
        private readonly SignInManager<User> _signInManager;

        public UserService(IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager,
            MariaDBContext context,
            IConfiguration configuration,
            SignInManager<User> signInManager)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._userManager = userManager;
            this._context = context;
            this._configuration = configuration;
            this._signInManager = signInManager;
        }


        public virtual async Task RegisterUser(RegisterModel model)
        {
            if (model.Username.Contains(" "))
            {
                throw new ExceptionWithDescription("A felhasználónévben nem szerepelhet szünet");
            }

            var user = new Models.User { Email = model.Email, UserName = model.Username };
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                throw new ExceptionWithDescription("Az e-mail cím foglalt");
            }
            var userBySameName = await _userManager.FindByNameAsync(model.Username);
            if (userBySameName != null)
            {
                throw new ExceptionWithDescription("A felhasználó név foglalt");

            }
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            else
            {
                throw new ExceptionWithDescription("Nem megfelelő jelszó");
            }
            

        }

        public virtual async Task<User?> GetUserByName(string name)
        {
            var result = await _userManager.FindByNameAsync(name);
            if (result is null)
            {
                return null;
            }
            return result;

        }

        public virtual async Task<LoginDetails?> LoginUser(LoginModel model)
        {
            var email = model.Email;
            var password = model.Password;
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return null;
            }

            var token = GenerateJwt(user);
            return  new LoginDetails(token, user.UserName);
        }

        public  virtual async Task<User?> GetLoggedInUser()
        {
            if (_httpContextAccessor.HttpContext is null)
            {
                return null;
            }
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (String.IsNullOrEmpty(userId))
            {
                return null;
            }
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return null;
            }
            return user;
        }

        private string GenerateJwt(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                 }),
                Expires = DateTime.UtcNow.AddDays(double.Parse(_configuration["Jwt:ExpirationDays"]!)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public virtual async Task<bool> IsCreatorInCompetition(int competitionId, int userId)
        {
            var competition = await _context.Competitions.FirstAsync(comp => comp.Id == competitionId);

            return competition != null && competition.CreatorId == userId;
        }

        public virtual async Task<bool> IsModeratorInCompetition(int competitionId, int userId)
        {
            var competition = await _context.Competitions.Include(comp => comp.Moderators).FirstAsync(comp => comp.Id == competitionId);

            return competition != null && competition.Moderators.Any(mod=>mod.Id == userId);
        }

        public virtual async Task<bool> CanSubmitOnCompetition(int competitionId, int userId)
        {
            var canSubmit =  await _context.Competitions
                .AnyAsync(comp => 
                comp.Submissions.Any(sub => sub.OwnerId == userId 
                    || sub.Creators.Any(crea => crea.Id == userId)) 
                 || comp.Moderators.Any(mod => mod.Id == userId)
                 || comp.CreatorId == userId
                );

            return !canSubmit;
        }

    }
}
