namespace Blog.PL.Areas.User.ViewModels.Like
{
    public class LikeViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserProfilePictureUrl { get; set; }
        public bool IsCurrentId { get; set; }
        public bool HavePicture { get; set; }
    }
}
