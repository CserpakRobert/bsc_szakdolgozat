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
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Competition> Competitions { get; set; }
        public virtual DbSet<Submission> Submissions { get; set; }
        public virtual DbSet<MovieDetails> MoviesDetails { get; set; }
        public virtual DbSet<MusicDetails> MusicDetails { get; set; }

    }
}
