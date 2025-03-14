using Backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Persistence
{
    public partial class MariaDBContext: IdentityDbContext<User,UserRole,int>
    {
        public MariaDBContext(DbContextOptions<MariaDBContext> options) : base(options)
        {
           
        }
        public virtual DbSet<testDataModel> Test { get; set; }
        public virtual DbSet<User> Users { get; set; }

    }
}
