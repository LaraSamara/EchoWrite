using Blog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BLL.Interfaces
{
    public interface IRepositoryLike
    {
        public int Add(Like postLike);
        public int Update(Like postLike);
        public int Delete(Like postLike);
        public Like Get(int id);
        public IEnumerable<Like> GetAll();
        public IEnumerable<Like> GetLikesForPost(int PostId);
        public Like GetByUserAndPost(int PostId, string UserId);
        public int PostLikesCount(int postId);
        public IEnumerable<Like> GetLikesForUser(string Id);
    }
}
