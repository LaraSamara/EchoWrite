using Blog.BLL.Interfaces;
using Blog.DAL.Models;
using Blog.PL.Areas.User.ViewModels.Follow;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.PL.Areas.User.Controllers
{
    [Area("User")]
    public class FollowsController : Controller
    {
        private readonly IRepositoryFollow _followRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public FollowsController(IRepositoryFollow followRepo, UserManager<ApplicationUser> userManager)
        {
            _followRepo = followRepo;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult GetFollowers(string Id)
        {
            var followers = _followRepo.GetFollowers(Id);
            var followersVM = followers.Select(follow => new FollowViewModel()
            {
                UserId = follow.FollowerId,
                UserName = $"{follow.Follower.FirstName} {follow.Follower.LastName}",
                HavePicture = (follow.Follower.ProfilePicture != null)? true:false,
                UserProfilePictureUrl = (follow.Follower.ProfilePicture) ?? $"{follow.Follower.FirstName[0]}{follow.Follower.LastName[0]}",
            });
            return PartialView("~/Areas/User/Views/Shared/Follows/_GetFollowers.cshtml", followersVM);
        }
        [HttpGet]
        public IActionResult GetFollowing(string Id)
        {
            var followings = _followRepo.GetFollowing(Id);
            var followingsVM = followings.Select(follow => new FollowViewModel()
            {
                UserId = follow.FollowingId,
                UserName = $"{follow.Following.FirstName} {follow.Following.LastName}",
                HavePicture = (follow.Following.ProfilePicture != null) ? true : false,
                UserProfilePictureUrl = (follow.Following.ProfilePicture) ?? $"{follow.Following.FirstName[0]}{follow.Following.LastName[0]}",
            });
            return PartialView("~/Areas/User/Views/Shared/Follows/_GetFollowing.cshtml", followingsVM);
        }
        [HttpPost]
        public IActionResult ToggleFollow(string UserId)
        {
            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId == null || UserId == null)
            {
                return Json(new { success = false, message = "User not authenticated or target user not found." });
            }
            bool IsFollowing = _followRepo.IsFollowing(currentUserId , UserId);
            if (IsFollowing)
            {
               var follow = _followRepo.GetFollow(currentUserId, UserId);
                try
                {
                    if (follow != null)
                    {
                        _followRepo.Delete(follow);
                        return Json(new { success = true, following = false, followerCount = _followRepo.GetFollowers(UserId).Count() });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Follow relationship not found." });
                    }
                }
                catch (Exception e)
                {
                    // Log the exception message e.Message for better debugging
                    return Json(new { success = false, message = "Failed to unfollow the user." });
                }
            } else{
                var follow = new Follow()
                {
                    FollowerId = currentUserId,
                    FollowingId = UserId,
                };
                try
                {

                    _followRepo.Add(follow);
                    return Json(new { success = true, following = true, followerCount = _followRepo.GetFollowers(UserId).Count() });

                }
                catch (Exception e)
                {
                    return Json(new { success = false, message = "Failed to follow the user."});
                }
               
            }
        }
    }
}
