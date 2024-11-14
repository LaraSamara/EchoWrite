using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<PostLike> PostLikes { get; set; }
        public ICollection<PostReport> PostReports { get; set; }

        public Post()
        {
            Comments = new HashSet<Comment>();
            PostLikes = new HashSet<PostLike>();
            PostReports = new HashSet<PostReport>();
        }
    }
}
