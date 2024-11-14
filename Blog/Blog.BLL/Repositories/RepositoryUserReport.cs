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
    public class RepositoryUserReport : IRepositoryUserReport
    {
        private readonly ApplicationDbContext _context;

        public RepositoryUserReport(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Add(UserReport report)
        {
           _context.UsersReport.Add(report);
            return _context.SaveChanges();
        }

        public int Delete(UserReport report)
        {
           _context.UsersReport.Remove(report);
            return _context.SaveChanges();
        }

        public IEnumerable<UserReport> GetAll()
        {
           return _context.UsersReport.AsNoTracking().ToList();
        }
        public bool HasReported(string ReporterId, string ReportedId)
        {
            return _context.UsersReport.AsNoTracking().Any(RU => RU.ReporterId == ReporterId && RU.ReportedId == ReportedId);
        }
    }
}
