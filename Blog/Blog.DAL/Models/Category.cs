using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }


        // Navigation property
        public ICollection<Post> Posts { get; set; }

        public Category()
        {
            Posts = new HashSet<Post>();
        }
    }
}
