using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Models
{
    public class Follow
    {
        public int Id { get; set; }
        public string FollowerId { get; set; }
        public string FollowingId { get; set; }
        // Navigation properties
        [ForeignKey("FollowerId")]
        public ApplicationUser Follower { get; set; }
        [ForeignKey("FollowingId")]
        public ApplicationUser Following { get; set; }
      
    }
}
