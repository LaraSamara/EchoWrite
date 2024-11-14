using Blog.PL.Areas.User.ViewModels.Comment;
using Blog.PL.Areas.User.ViewModels.Like;

namespace Blog.PL.Areas.User.ViewModels.Post
{
    public class PostViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool HavePicture { get; set; }
        public string UserProfilePictureUrl { get; set; }
        public int PostId { get; set; }
        public string CategoryName { get; set; }
        public string Content { get; set; } // Content of the post to be created
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public bool IsCurrentUser {  get; set; }
        public bool IsLiked { get; set; }
    }
}
