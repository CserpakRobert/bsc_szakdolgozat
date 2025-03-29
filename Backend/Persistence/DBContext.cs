using Backend.Models;
using Backend.Models.JoinTables;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Persistence
{
    public partial class MariaDBContext : IdentityDbContext<User, UserRole, int>
    {
        public MariaDBContext(DbContextOptions<MariaDBContext> options) : base(options)
        {

        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Competition> Competitions { get; set; }
        public virtual DbSet<Submission> Submissions { get; set; }
        public virtual DbSet<MovieDetails> MoviesDetails { get; set; }
        public virtual DbSet<MusicDetails> MusicDetails { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<ClosedGroupJoinRequests> JoinRequests { get; set; }

        // Join tables

        //public virtual DbSet<CollaboratedSubmissions> CollaboratedSubmissions { get; set; }
        //public virtual DbSet<Moderators> Moderators { get; set; }
        //public virtual DbSet<SubmissionInvites> SubmissionInvites { get; set; }
        //public virtual DbSet<UserParticipateGroups> UserParticipateGroups { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(e => e.Id);
            modelBuilder.Entity<Competition>().HasKey(e => e.Id);
            modelBuilder.Entity<ClosedGroupJoinRequests>().HasKey(e => e.Id);
            modelBuilder.Entity<Group>().HasKey(e => e.Id);
            modelBuilder.Entity<MovieDetails>().HasKey(e => e.Id);
            modelBuilder.Entity<MusicDetails>().HasKey(e => e.Id);
            modelBuilder.Entity<SubmissionInvites>().HasKey(e => e.Id);
            modelBuilder.Entity<Submission>().HasKey(e => e.Id);



            //User

            modelBuilder.Entity<User>().HasMany(e => e.CreatedCompetitions).WithOne(e => e.Creator)/*.HasForeignKey(e=>e.CreatorId)*/;
            modelBuilder.Entity<User>().HasMany(e => e.CreatedSubmissions).WithOne(e => e.Owner);
            modelBuilder.Entity<User>().HasMany(e => e.SentInvites).WithOne(e => e.SenderUser);
            modelBuilder.Entity<User>().HasMany(e => e.RecievedInvites).WithOne(e => e.RecieverUser);
            modelBuilder.Entity<User>().HasMany(e => e.OwnedGroups).WithOne(e => e.GroupOwner);
            modelBuilder.Entity<User>().HasMany(e => e.JoinRequests).WithOne(e => e.RequestOwner);


            modelBuilder.Entity<User>()
                .HasMany(e => e.ModeratedCompetitions)
                .WithMany(e => e.Moderators);
                //.UsingEntity<Moderators>(
                //    l => l.HasOne<Competition>().WithMany().HasForeignKey(e => e.CompetitionId),
                //    r => r.HasOne<User>().WithMany().HasForeignKey(e => e.UserId)
                //    );

            modelBuilder.Entity<User>()
                .HasMany(e => e.CollaboratedSubmissions)
                .WithMany(e => e.Creators);
                //.UsingEntity<CollaboratedSubmissions>(
                //    l => l.HasOne<Submission>().WithMany().HasForeignKey(e => e.SubmissionId),
                //    r => r.HasOne<User>().WithMany().HasForeignKey(e => e.UserId)
                //    );

            modelBuilder.Entity<User>()
                .HasMany(e => e.ParticipatingGroups)
                .WithMany(e => e.Participants);
                //.UsingEntity<UserParticipateGroups>(
                //    l => l.HasOne<Group>().WithMany().HasForeignKey(e => e.GroupId),
                //    r => r.HasOne<User>().WithMany().HasForeignKey(e => e.UserId)
                //    );

            //Group
            modelBuilder.Entity<Group>().HasMany(e => e.Invites).WithOne(e => e.Group);
            modelBuilder.Entity<Group>().HasMany(e => e.JoinRequests).WithOne(e => e.Group);

            // Competition

            modelBuilder.Entity<Competition>().HasMany(e => e.Submissions).WithOne(e => e.Competition);
            modelBuilder.Entity<Competition>().HasMany(e => e.Groups).WithOne(e => e.Competition);


            // Submission
            modelBuilder.Entity<Submission>().HasOne(e => e.MovieDetails)
                .WithOne(e => e.Submission).HasForeignKey<Submission>(e => e.MovieDetailsId).IsRequired(false);
            modelBuilder.Entity<Submission>().HasOne(e => e.MusicDetails)
                .WithOne(e => e.Submission).HasForeignKey<Submission>(e => e.MusicDetailsId).IsRequired(false);
            modelBuilder.Entity<Submission>().HasOne(e => e.ShortStoryDetails)
                .WithOne(e => e.Submission).HasForeignKey<Submission>(e => e.ShortStoryDetailsId).IsRequired(false);

            modelBuilder.Entity<Submission>()
                .HasMany(e => e.VotedUsers)
                .WithMany(e => e.VotedSubmissions);
                //.UsingEntity<SubmissionVotes>(
                //    l => l.HasOne<User>().WithMany().HasForeignKey(e => e.UserId),
                //    r => r.HasOne<Submission>().WithMany().HasForeignKey(e => e.SubmissionId)
                //    );


            //TODO: Indexes

        }
    }
}
