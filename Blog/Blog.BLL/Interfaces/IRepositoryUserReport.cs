using Blog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BLL.Interfaces
{
    public interface IRepositoryUserReport
    {
        public int Add(UserReport report);
        public int Delete (UserReport report);
        public int Update (UserReport report);
        public IEnumerable<UserReport> GetAll ();
        public UserReport Get(int id);
        public bool HasReported(string userId, string reportedUserId);
        public int UserReportCount();
        public IEnumerable<UserReport> GetUserReport(string Id, int reportId);
    }
}
