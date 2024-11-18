using Blog.BLL.Interfaces;
using Blog.DAL.Data;
using Blog.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public int Update(UserReport report)
        {
            _context.UsersReport.Update(report);
            return _context.SaveChanges();
        }
        public UserReport Get(int id)
        {
            return _context.UsersReport.Where(Report => Report.Id == id && Report.IsHandled == false).FirstOrDefault();
        }
        public IEnumerable<UserReport> GetAll()
        {
            return _context.UsersReport.AsNoTracking()
                 .Where(report => _context.Users.Any(user => user.Id == report.ReporterId) &&
                                  _context.Users.Any(user => user.Id == report.ReportedId))
                 .Where(report => report.IsHandled == false)
                 .Include(report => report.Reporter)
                .ToList();
        }
        public bool HasReported(string ReporterId, string ReportedId)
        {
            return _context.UsersReport.AsNoTracking().Any(RU => RU.ReporterId == ReporterId && RU.ReportedId == ReportedId);
        }
        public int UserReportCount() {
            return _context.UsersReport.Count();
        }
        public IEnumerable<UserReport> GetUserReport(string Id, int reportId)
        {
            return _context.UsersReport.AsNoTracking().Where(u => (u.ReporterId == Id || u.ReportedId == Id) && u.Id != reportId).ToList();
        }
    }
}
