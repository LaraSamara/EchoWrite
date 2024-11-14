using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Models
{
    public class PostReport:Report
    {
        public int PostId { get; set; }


        // Navigation properties
        [ForeignKey("PostId")]
        public Post Post { get; set; }
    }
}
