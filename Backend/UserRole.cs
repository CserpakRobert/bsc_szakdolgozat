using Microsoft.AspNetCore.Identity;

namespace Backend
{
    public class UserRole: IdentityRole<int>
    {
        public UserRole() : base() { }
        public UserRole(string roleName) : base(roleName) { }
    }
}
