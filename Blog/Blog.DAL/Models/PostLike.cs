using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Models
{
    public class PostLike: Like
    {
        public int PostId { get; set; }
        //Navigation property
        [ForeignKey("PostId")]
        public Post Post { get; set; }
    }
}
