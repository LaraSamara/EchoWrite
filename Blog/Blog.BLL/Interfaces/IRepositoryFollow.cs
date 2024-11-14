using Blog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BLL.Interfaces
{
    public interface IRepositoryFollow
    {
        public int Add(Follow follow);
        public int Update(Follow follow);
        public int Delete(Follow follow);
        public Follow Get(int id);
        public Follow GetFollow(string CurrentUser, string UserId);
        public IEnumerable<Follow> GetFollowers(string Id);
        public IEnumerable<Follow> GetFollowing(string Id);
        public IEnumerable<Follow> GetAll();
        public bool IsFollowing(string CurrentUser, string UserId);
    }
}
