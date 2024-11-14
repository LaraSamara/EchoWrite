using Blog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BLL.Interfaces
{
    public interface IRepositoryPost
    {
        public int Add(Post post);
        public int Update(Post post);
        public int Delete(Post post);
        public Post Get(int id);
        //public IEnumerable<Post> GetUserPosts(string UserId);
        public IEnumerable<Post> GetUserPostsFilterByCategory(string UserId, int? CategoryId);
        IEnumerable<Post> GetFollowingPosts(IEnumerable<string> FollowingIds, int? categoryId);
        public IEnumerable<Post> GetAll();
    }
}
