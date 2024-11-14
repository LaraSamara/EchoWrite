using Blog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BLL.Interfaces
{
    public interface IRepositoryCommentLike
    {
        public int Add(CommentLike commentLike);
        public int Update(CommentLike commentLike);
        public int Delete(CommentLike commentLike);
        public CommentLike Get(int id);
        public IEnumerable<CommentLike> GetAll();
    }
}
