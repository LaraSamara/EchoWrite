using Blog.PL.Areas.User.ViewModels.Post;

namespace Blog.PL.Areas.User.ViewModels.User
{
    public class ProfileViewModel
    {
        public UserProfileViewModel Profile { get; set; }
        public IEnumerable<PostViewModel> Posts { get; set; }

    }
}
