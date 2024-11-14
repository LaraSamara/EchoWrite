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
            _context.Comments.Remove(comment);
            return _context.SaveChanges();
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
    }
}
