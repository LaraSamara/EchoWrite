using Blog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BLL.Interfaces
{
    public interface IRepositoryPostReport
    {
        public int Add(PostReport report);
        public int Update(PostReport report);
        public int Delete(PostReport report);
        public PostReport Get(int id);
        public IEnumerable<PostReport> GetAll();
    }
}
