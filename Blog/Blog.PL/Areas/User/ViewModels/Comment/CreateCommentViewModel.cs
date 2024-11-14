using Blog.PL.Areas.User.ViewModels.Like;

namespace Blog.PL.Areas.User.ViewModels.Comment
{
    public class CreateCommentViewModel
    {
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
    }
}
