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
    public class RepositoryLike : IRepositoryLike
    {
        private readonly ApplicationDbContext _context;

        public RepositoryLike(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Add(Like postLike)
        {
            _context.Likes.Add(postLike);
            return _context.SaveChanges();
        }

        public int Delete(Like postLike)
        {
            _context.Likes.Remove(postLike);
            return _context.SaveChanges();
        }

        public Like Get(int id)
        {
            var postsLike = _context.Likes.Local.Where(p => p.Id == id).FirstOrDefault();
            if(postsLike == null)
            {
                postsLike = _context.Likes.AsNoTracking().Where(p => p.Id == id).FirstOrDefault();
            }
            return postsLike;
        }

        public IEnumerable<Like> GetAll()
        {
            return _context.Likes.AsNoTracking().ToList();
        }

        public int Update(Like postLike)
        {
            _context.Likes.Update(postLike);
            return _context.SaveChanges();
        }
        public Like GetByUserAndPost(int PostId, string UserId)
        {
            return _context.Likes.AsNoTracking().Where(p => p.UserId == UserId && p.PostId == PostId).FirstOrDefault();
        }
        public int PostLikesCount(int postId)
        {
            return _context.Likes.Count(pl => pl.PostId == postId);
        }
        public IEnumerable<Like> GetLikesForPost(int Id)
        {
            return _context.Likes.AsNoTracking().Where(p => p.PostId == Id).Include(l => l.User).Include(l => l.Post).ToList();
        }
        public IEnumerable<Like> GetLikesForUser(string Id)
        {
            return _context.Likes.AsNoTracking().Where(p => p.UserId == Id).Include(l => l.User).Include(l => l.Post).ToList();

        }
    }
}
