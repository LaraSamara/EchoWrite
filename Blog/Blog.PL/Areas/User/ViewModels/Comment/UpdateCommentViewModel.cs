namespace Blog.PL.Areas.User.ViewModels.Comment
{
    public class UpdateCommentViewModel
    {
        public string Content { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public int CommentId { get; set; }
    }
}
