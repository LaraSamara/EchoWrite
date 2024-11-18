namespace Blog.PL.Areas.Admin.ViewModels
{
    public class PostViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool HavePicture { get; set; }
        public string UserProfilePictureUrl { get; set; }
        public int PostId { get; set; }
        public string CategoryName { get; set; }
        public string Content { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
