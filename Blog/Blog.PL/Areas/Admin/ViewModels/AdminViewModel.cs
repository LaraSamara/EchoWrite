using Blog.PL.Areas.User.ViewModels.User;
namespace Blog.PL.Areas.Admin.ViewModels
{
    public class AdminViewModel
    {
        public int CommentCount { get; set; }
        public int UserCount { get; set; }
        public int PostCount { get; set; }
        public int CommentReportCount { get; set; }
        public int UserReportCount { get; set; }
        public int PostReportCount { get; set; }
        public IEnumerable<UserViewModel> Users { get; set; }
    }
}
