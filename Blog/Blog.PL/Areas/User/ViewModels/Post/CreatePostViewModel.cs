using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.PL.Areas.User.ViewModels.Post
{
    public class CreatePostViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool HavePicture { get; set; }
        public string UserProfilePictureUrl { get; set; }
        public string Content { get; set; }
        public int? CategoryId { get; set; }
        public SelectList? Category {  get; set; }
    }
}
