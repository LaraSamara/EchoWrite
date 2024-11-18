using Blog.PL.Areas.User.ViewModels.User;

namespace Blog.PL.Areas.Admin.ViewModels
{
    public class ReportViewModel
    {
        public int Id { get; set; }
        public UserViewModel Reporter { get; set; }
        public string Reason { get; set; }
        public DateTime ReportDate { get; set; }
        public string? ReportedId { get; set; }
        public int? CommentId { get; set; }
        public int? PostId { get; set; }
    }
}
