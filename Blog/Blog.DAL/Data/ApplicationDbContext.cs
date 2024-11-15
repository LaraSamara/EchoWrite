using Blog.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var RoleAdminId = Guid.NewGuid().ToString();
            var RoleUserId = Guid.NewGuid().ToString();
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = RoleAdminId, Name = "Admin" ,NormalizedName = "ADMIN"},
                new IdentityRole { Id = RoleUserId , Name = "User", NormalizedName = "USER"}
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Phisics"},
                new Category {Id = 2, Name = "Medical" },
                new Category { Id = 3, Name = "Economic"},
                new Category { Id = 4, Name = "IT" }
            );

            // Comment -> Post (One-to-Many)
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            // PostLike -> Post (One-to-Many)
            modelBuilder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.PostLikes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            // CommentReport -> Comment (One-to-Many)
            modelBuilder.Entity<CommentReport>()
                .HasOne(cr => cr.Comment)
                .WithMany(c => c.CommentReports)
                .HasForeignKey(rc => rc.CommentId)
                .OnDelete(DeleteBehavior.Restrict);

            // PostReport -> Post (One-to-Many)
            modelBuilder.Entity<PostReport>()
                .HasOne(pr => pr.Post)
                .WithMany(p => p.PostReports)
                .HasForeignKey(rp => rp.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            // Follow -> Follower (Self-referencing many-to-many)
            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Follow -> Followed (Self-referencing many-to-many)
            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Following)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Post -> ApplicationUser (One-to-Many)
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Comment -> ApplicationUser (One-to-Many)
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // PostLike -> ApplicationUser (One-to-Many)
            modelBuilder.Entity<Like>()
                .HasOne(pl => pl.User)
                .WithMany(u => u.PostLikes)
                .HasForeignKey(pl => pl.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // PostReport -> ApplicationUser (One-to-Many)
            modelBuilder.Entity<PostReport>()
                .HasOne(pr => pr.Reporter)
                .WithMany(u => u.PostReports)
                .HasForeignKey(pr => pr.ReporterId)
                .OnDelete(DeleteBehavior.Restrict);

            // CommentReport -> ApplicationUser (One-to-Many)
            modelBuilder.Entity<CommentReport>()
                .HasOne(cr => cr.Reporter)
                .WithMany(u => u.CommentReports)
                .HasForeignKey(cr => cr.ReporterId)
                .OnDelete(DeleteBehavior.Restrict);

            // Category -> Post (One-to-Many)
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserReport>()
            .HasOne(r => r.Reporter)
            .WithMany(u => u.ReportsMade)
            .HasForeignKey(r => r.ReporterId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserReport>()
                .HasOne(r => r.Reported)
                .WithMany(u => u.ReportsReceived)
                .HasForeignKey(r => r.ReportedId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<ApplicationUser> Users {  get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet <CommentReport> CommentsReport { get; set; }
        public DbSet<PostReport> PostsReport { get; set; }
        public DbSet<UserReport> UsersReport { get; set; }
    }
}
