using Blog.BLL.Interfaces;
using Blog.DAL.Data;
using Blog.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BLL.Repositories
{
    public class RepositoryCategory : IRepositoryCategory
    {
        private readonly ApplicationDbContext _context;

        public RepositoryCategory(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Add(Category category)
        {
            _context.Categories.Add(category);
            return _context.SaveChanges();
        }

        public int Delete(Category category)
        {
            _context.Categories.Remove(category);
            return _context.SaveChanges();
        }

        public Category Get(int id)
        {
            var category = _context.Categories.Local.Where(c => c.Id == id).FirstOrDefault();
            if(category == null)
            {
                category = _context.Categories.AsNoTracking().Where(c => c.Id == id).FirstOrDefault();
            }
            return category;
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories.AsNoTracking().ToList();
        }

        public int Update(Category category)
        {
            _context.Categories.Update(category);
            return _context.SaveChanges();
        }
    }
}
