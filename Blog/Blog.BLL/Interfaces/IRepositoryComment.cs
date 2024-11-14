using Blog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BLL.Interfaces
{
    public interface IRepositoryComment
    {
        public int Add(Comment comment);
        public int Update(Comment comment);
        public int Delete(Comment comment);
        public Comment Get(int id);
        public IEnumerable<Comment> GetAll();
        public IEnumerable<Comment> GetPostComments(int PostId);
        public int GetPostCommentsCount(int PostId);
    }
}
