using Blog.BLL.Interfaces;
using Blog.DAL.Data;
using Blog.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BLL.Repositories
{
    public class RepositoryComment : IRepositoryComment
    {
        private readonly ApplicationDbContext _context;

        public RepositoryComment(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Add(Comment comment)
        {
            _context.Comments.Add(comment);
            return _context.SaveChanges();
        }

        public int Delete(Comment comment)
        {
            var c = _context.Comments
            .Include(c => c.CommentReports)
                   .FirstOrDefault(c => c.Id == c.Id);

            if (comment != null)
            {
                // Remove dependent entities
                _context.CommentsReport.RemoveRange(c.CommentReports);

                // Remove the comment itself
                _context.Comments.Remove(c);

                // Save changes
                _context.SaveChanges();
            }
            return 0;
        }

        public Comment Get(int id)
        {
           var comment = _context.Comments.Local.Where(c => c.Id == id).FirstOrDefault();
            if(comment == null)
            {
                comment = _context.Comments.AsNoTracking().Where(c => c.Id == id).FirstOrDefault(); 
            }
            return comment;
        }

        public IEnumerable<Comment> GetAll()
        {
            return _context.Comments.AsNoTracking().ToList();
        }

        public int Update(Comment comment)
        {
            _context.Comments.Update(comment);
            return _context.SaveChanges();
        }
        public IEnumerable<Comment> GetPostComments(int PostId)
        {
            return _context.Comments.AsNoTracking().Where(c => c.PostId == PostId).Include(c => c.User).OrderByDescending(c => c.CreatedAt).ToList();
        }
        public int GetPostCommentsCount(int PostId)
        {
            return _context.Comments.Count(c => c.PostId == PostId);
        }
        public int CommentCount()
        {
            return _context.Comments.Count();
        }
        public IEnumerable<Comment> GetUserComments(string Id)
        {
            return _context.Comments.AsNoTracking().Where(c => c.UserId == Id).ToList();
        }
    }
}
