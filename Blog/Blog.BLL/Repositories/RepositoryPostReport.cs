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
    public class RepositoryPostReport:IRepositoryPostReport
    {
        private readonly ApplicationDbContext _context;

        public RepositoryPostReport(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(PostReport report)
        {
            _context.PostsReport.Add(report);
            return _context.SaveChanges();
        }

        public int Delete(PostReport report)
        {
            _context.PostsReport.Remove(report);
            return _context.SaveChanges();
        }

        public PostReport Get(int id)
        {
            var report = _context.PostsReport.Local.Where(p => p.Id == id).FirstOrDefault();
            if(report == null) {
                report = _context.PostsReport.AsNoTracking().Where(p => p.Id == id).FirstOrDefault();
            }
            return report;
        }

        public IEnumerable<PostReport> GetAll()
        {
            return _context.PostsReport.AsNoTracking().ToList();
        }

        public int Update(PostReport report)
        {
            _context.PostsReport.Update(report);
            return _context.SaveChanges();
        }
    }
}
