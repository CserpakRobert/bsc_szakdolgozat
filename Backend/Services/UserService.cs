
using System.Security.Claims;

namespace Backend.Services
{
    public class UserService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int? GetId()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext is not null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                int id = -1;
                if (result is not null && int.TryParse(result, out id)){
                    if (id == -1)
                    {
                        return null;
                    }
                    return id;
                } 
            }
            return null;
        }
    }
}
