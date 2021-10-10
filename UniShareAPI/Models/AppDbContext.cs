using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using UniShareAPI.Models.DTO;
using UniShareAPI.Models.Relations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniShareAPI.Models
{
    public class AppDbContext : IdentityDbContext
    {
        public virtual DbSet<ItemData> Items { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Degree> Degrees { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Forum> Forum { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Review> Review { get; set; }
        public virtual DbSet<Request> Request { get; set; }
        public virtual DbSet<Rating> Rating { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
            modelBuilder.Entity<Degree>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
