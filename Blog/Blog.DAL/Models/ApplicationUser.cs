using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Bio {  get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> PostLikes { get; set; }
        public ICollection<PostReport> PostReports { get; set; }
        public ICollection<CommentReport> CommentReports { get; set; }
        public ICollection<Follow> Followers { get; set; }
        public ICollection<Follow> Following { get; set; }
        public ICollection<UserReport> ReportsMade { get; set; }  // Reports made by this user
        public ICollection<UserReport> ReportsReceived { get; set; }  // Reports against this user

        public ApplicationUser()
        {
            Posts = new HashSet<Post>();
            Comments = new HashSet<Comment>();
            PostLikes = new HashSet<Like>();
            PostReports = new HashSet<PostReport>();
            CommentReports = new HashSet<CommentReport>();
            Followers = new HashSet<Follow>();
            Following = new HashSet<Follow>();
            ReportsMade = new HashSet<UserReport>();
            ReportsReceived = new HashSet<UserReport>();
        }

    }
}
