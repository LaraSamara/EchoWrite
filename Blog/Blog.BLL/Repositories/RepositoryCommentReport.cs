﻿using Blog.BLL.Interfaces;
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
    public class RepositoryCommentReport : IRepositoryCommentReport
    {
        private readonly ApplicationDbContext _context;

        public RepositoryCommentReport(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(CommentReport report)
        {
            _context.CommentsReport.Add(report);
            return _context.SaveChanges();
        }

        public int Delete(CommentReport report)
        {
            _context.CommentsReport.Remove(report);
            return _context.SaveChanges();
        }

        public CommentReport Get(int id)
        {
            var report = _context.CommentsReport.Local.Where(c  => c.Id == id).FirstOrDefault();
            if(report == null) {
                report = _context.CommentsReport.AsNoTracking().Where(c => c.Id == id).FirstOrDefault();
            }
            return report;
        }

        public IEnumerable<CommentReport> GetAll()
        {
            return _context.CommentsReport.AsNoTracking()
                .Where(report => _context.Users.Any(user => user.Id == report.ReporterId) &&
                                 _context.Comments.Any(comment => comment.Id == report.CommentId))
                .Where(report => report.IsHandled == false)
                .Include(report => report.Comment)
                .Include(report => report.Reporter)
                .ToList();
        }

        public int Update(CommentReport report)
        {
            _context.CommentsReport.Update(report);
            return _context.SaveChanges();
        }
        public int CommentReportCount()
        {
            return _context.CommentsReport.Count();
        }
        public IEnumerable<CommentReport> GetCommentsReportsByUser(string Id)
        {
            return _context.CommentsReport.AsNoTracking().Where(c => c.ReporterId == Id).ToList();  
        }
    }
}
