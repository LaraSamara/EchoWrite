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
    public class RepositoryCommentLike : IRepositoryCommentLike
    {
        private readonly ApplicationDbContext _context;

        public RepositoryCommentLike(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Add(CommentLike commentLike)
        {
            _context.CommentsLike.Add(commentLike);
            return _context.SaveChanges();
        }

        public int Delete(CommentLike commentLike)
        {
            _context.CommentsLike.Remove(commentLike);
            return _context.SaveChanges();
        }

        public CommentLike Get(int id)
        {
            var commentLike = _context.CommentsLike.Local.Where(c => c.Id == id).FirstOrDefault();
            if(commentLike == null)
            {
                commentLike = _context.CommentsLike.AsNoTracking().Where(c => c.Id == id).FirstOrDefault();
            }
            return commentLike;
        }

        public IEnumerable<CommentLike> GetAll()
        {
            return _context.CommentsLike.AsNoTracking().ToList();
        }

        public int Update(CommentLike commentLike)
        {
            _context.CommentsLike.Update(commentLike);
            return _context.SaveChanges();
        }
    }
}
