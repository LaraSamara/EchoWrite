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
    public class RepositoryPostLike : IRepositoryPostLike
    {
        private readonly ApplicationDbContext _context;

        public RepositoryPostLike(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Add(PostLike postLike)
        {
            _context.PostsLike.Add(postLike);
            return _context.SaveChanges();
        }

        public int Delete(PostLike postLike)
        {
            _context.PostsLike.Remove(postLike);
            return _context.SaveChanges();
        }

        public PostLike Get(int id)
        {
            var postsLike = _context.PostsLike.Local.Where(p => p.Id == id).FirstOrDefault();
            if(postsLike == null)
            {
                postsLike = _context.PostsLike.AsNoTracking().Where(p => p.Id == id).FirstOrDefault();
            }
            return postsLike;
        }

        public IEnumerable<PostLike> GetAll()
        {
            return _context.PostsLike.AsNoTracking().ToList();
        }

        public int Update(PostLike postLike)
        {
            _context.PostsLike.Update(postLike);
            return _context.SaveChanges();
        }
        public PostLike GetByUserAndPost(int PostId, string UserId)
        {
            return _context.PostsLike.AsNoTracking().Where(p => p.UserId == UserId && p.PostId == PostId).FirstOrDefault();
        }
        public int PostLikesCount(int postId)
        {
            return _context.PostsLike.Count(pl => pl.PostId == postId);
        }
        public IEnumerable<PostLike> GetLikesForPost(int Id)
        {
            return _context.PostsLike.AsNoTracking().Where(p => p.PostId == Id).Include(l => l.User).Include(l => l.Post).ToList();
        }
    }
}
