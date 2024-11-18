using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.DAL.Models
{
    public class CommentReport:Report
    {
        public int CommentId { get; set; }

        // Navigation properties
        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }
    }
}
