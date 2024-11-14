using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
