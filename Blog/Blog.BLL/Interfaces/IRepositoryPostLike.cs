using Blog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BLL.Interfaces
{
    public interface IRepositoryPostLike
    {
        public int Add(PostLike postLike);
        public int Update(PostLike postLike);
        public int Delete(PostLike postLike);
        public PostLike Get(int id);
        public IEnumerable<PostLike> GetAll();
        public IEnumerable<PostLike> GetLikesForPost(int PostId);
        public PostLike GetByUserAndPost(int PostId, string UserId);
        public int PostLikesCount(int postId);
    }
}
