namespace Blog.PL.Areas.Admin.ViewModels
{
    public class CommentViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool HavePicture { get; set; }
        public string UserProfilePictureUrl { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
