using Blog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BLL.Interfaces
{
    public interface IRepositoryCommentReport
    {
        public int Add(CommentReport report);
        public int Update(CommentReport report);
        public int Delete(CommentReport report);
        public CommentReport Get(int id);
        public IEnumerable<CommentReport> GetAll();  
        public int CommentReportCount();
        public IEnumerable<CommentReport> GetCommentsReportsByUser(string Id);
    }
}
