using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content {  get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }


        // Navigation properties
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }
        public ICollection<CommentReport> CommentReports { get; set; }

        public Comment()
        {
            CommentReports = new HashSet<CommentReport>();
        }
    }
}
