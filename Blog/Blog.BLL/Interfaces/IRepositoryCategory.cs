using Blog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BLL.Interfaces
{
    public interface IRepositoryCategory
    {
        public int Add(Category category);
        public int Update(Category category);
        public int Delete(Category category);
        public Category Get(int id);
        public IEnumerable<Category> GetAll();
    }
}
