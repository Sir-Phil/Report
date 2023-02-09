using Articles.Models;
using Microsoft.EntityFrameworkCore;

namespace Articles.Infrastructure
{
    public class ReportDbContext : DbContext
    {
        public ReportDbContext(DbContextOptions<ReportDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Report> Reports { get; set; } = null;
        public DbSet<Person> Persons { get; set; } = null;
        public DbSet<Comment> Comments { get; set; } = null ;
        public DbSet<Tag> Tags { get; set; } = null;
        public DbSet<FollowedPeople> FollowedPeoples { get; set; } = null;
        public DbSet<ReportFavorite> ReportFavorites { get; set; } = null;
        public DbSet<ReportTag> ReportTags { get; set; } = null;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReportTag>(b =>
            {
                b.HasKey(t => new { t.ReportId, t.TagId });

                b.HasOne(pt => pt.Report).WithMany(p => p!.ReportTags)
                .HasForeignKey(pt => pt.ReportId);

                b.HasOne(pt => pt.Tag).WithMany(t => t!.ReportTags)
                .HasForeignKey(pt => pt.TagId);
            });

            modelBuilder.Entity<ReportFavorite>(b =>
            {
                b.HasKey(t => new { t.ReportId, t.PersonId });

                b.HasOne(pt => pt.Report).WithMany(p => p!.ReportFavorites)
                .HasForeignKey(pt => pt.ReportId);

                b.HasOne(pt => pt.Person).WithMany(t => t!.ReportFavorites)
                .HasForeignKey(pt => pt.PersonId);
            });


            modelBuilder.Entity<FollowedPeople>(b =>
            {
                b.HasKey(t => new { t.ObserveId, t.TargetId });

                //ondelete retriction should be added otherwise sqlserver will throw error

                b.HasOne(pt => pt.Observer).WithMany(p => p!.Followers)
                .HasForeignKey(pt => pt.ObserveId).OnDelete(DeleteBehavior.Restrict);

                b.HasOne(pt => pt.Target).WithMany(t => t!.Following)
                .HasForeignKey(pt => pt.TargetId).OnDelete(DeleteBehavior.Restrict);
            });

        }

    }
}
