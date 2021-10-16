using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO;

namespace UniShareAPI.Models.Relations
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<User> User { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<DegreeCourse> DegreeCourses { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasMany(c => c.RefreshTokens)
                .WithOne(e => e.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasMany(c => c.Projects)
                .WithOne(e => e.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasMany(c => c.Degrees)
                .WithOne(e => e.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasOne(c => c.ActiveDegree)
                .WithOne(e => e.ActiveDegreeUser)
                .HasForeignKey<User>(p => p.ActiveDegreeId)
                .IsRequired(false);

            builder.Entity<DegreeCourse>()
                .HasKey(pc => new { pc.CourseId, pc.DegreeId });

            builder.Entity<UserCourse>()
                .HasKey(pc => new { pc.CourseId, pc.UserId });

            //Reviews, Users and Courses.
            builder.Entity<Review>()
                .HasKey(pc => new { pc.CourseId, pc.UserId });

            builder.Entity<Comment>()
                .HasKey(pc => new { pc.Id });

            builder.Entity<Comment>()
                .HasOne(e => e.Author)
                .WithMany(e => e.Writer)
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>()
                .HasOne(e => e.Profile)
                .WithMany(e => e.Receiver)
                .HasForeignKey(e => e.ProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
