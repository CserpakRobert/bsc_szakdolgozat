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
        public virtual DbSet<InvitedSubmissionUsers> InvitedSubmissionUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(e => e.Id);
            modelBuilder.Entity<User>().HasMany(e => e.CreatedCompetitions).WithOne(e => e.Creator).HasForeignKey(e=>e.CreatorId);
            modelBuilder.Entity<User>().HasMany(e => e.ModeratedCompetitions).WithMany(e => e.Moderators);

            modelBuilder.Entity<User>().HasMany(e => e.CreatedSubmissions).WithOne(e => e.Owner);
            modelBuilder.Entity<User>().HasMany(e => e.CollaboratedSubmissions).WithMany(e => e.Creators);

            modelBuilder.Entity<Competition>().HasMany(e => e.Submissions).WithOne(e => e.Competition).HasForeignKey(e=>e.CompetitionId);

            modelBuilder.Entity<Submission>().HasOne(e => e.MovieDetails)
                .WithOne(e => e.Submission).HasForeignKey<Submission>(e=>e.MovieDetailsId).IsRequired(false);
            modelBuilder.Entity<Submission>().HasOne(e => e.MusicDetails)
                .WithOne(e => e.Submission).HasForeignKey<Submission>(e => e.MusicDetailsId).IsRequired(false);

            modelBuilder.Entity<Submission>().HasMany(e => e.VotedUsers).WithMany(e => e.VotedSubmissions);

            modelBuilder.Entity<InvitedSubmissionUsers>().HasOne(e => e.Submission).WithMany(e => e.Invites);

            modelBuilder.Entity<InvitedSubmissionUsers>().HasOne(e => e.Creator).WithMany(e => e.Invites);

        }
    }
}
