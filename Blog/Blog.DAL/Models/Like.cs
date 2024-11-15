using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Models
{
    public class Like
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }

        // Navigation property
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        //Navigation property
        [ForeignKey("PostId")]
        public Post Post { get; set; }
    }
}
