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
    public class RepositoryFollow : IRepositoryFollow
    {
        private readonly ApplicationDbContext _context;

        public RepositoryFollow(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Add(Follow follow)
        {
            _context.Follows.Add(follow);
            return _context.SaveChanges();
        }

        public int Delete(Follow follow)
        {
            _context.Follows.Remove(follow);
            return _context.SaveChanges();
        }

        public Follow Get(int id)
        {
            var follow = _context.Follows.Local.Where(f => f.Id == id).FirstOrDefault();
            if(follow == null)
            {
                follow = _context.Follows.AsNoTracking().Where(f => f.Id == id).FirstOrDefault();
            }
            return follow;
        }
        public Follow GetFollow(string CurrentUser, string UserId)
        {
            return _context.Follows.AsNoTracking().SingleOrDefault(f => f.FollowingId == UserId && f.FollowerId == CurrentUser);
        }
        public IEnumerable<Follow> GetAll()
        {
            return _context.Follows.AsNoTracking().ToList();
        }
        public int Update(Follow follow)
        {
            _context.Follows.Update(follow);
            return _context.SaveChanges();
        }
        public IEnumerable<Follow> GetFollowers(string Id)
        {
            return _context.Follows.AsNoTracking().Where(f => f.FollowingId == Id).Include(f => f.Follower).ToList();
        }
        public IEnumerable<Follow> GetFollowing(string Id)
        {
            return _context.Follows.AsNoTracking().Where(f => f.FollowerId == Id).Include(f => f.Following).ToList();
        }
        public bool IsFollowing(string CurrentUser, string UserId)
        {
           return _context.Follows.AsNoTracking().Any(f => f.FollowingId == UserId && f.FollowerId == CurrentUser);
        }
    }
}
