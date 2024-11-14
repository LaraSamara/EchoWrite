namespace Blog.PL.Areas.User.ViewModels.User
{
    public class UserProfileViewModel
    {
        public string UserId { get; set; }
        public bool IsCurrentUser { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Bio { get; set; }
        public bool HavePicture { get; set; }
        public string UserProfilePictureUrl { get; set; }
        public int PostCount { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public bool IsFollowing { get; set; }

    }
}
