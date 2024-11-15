using Blog.BLL.Interfaces;
using Blog.DAL.Data;
using Blog.DAL.Models;
using Blog.PL.Areas.User.ViewModels.Follow;

//using Blog.PL.Areas.User.ViewModels.Like;
using Blog.PL.Areas.User.ViewModels.Post;
using Blog.PL.Areas.User.ViewModels.User;
using Blog.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.PL.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IRepositoryPost _postRepo;
        private readonly IRepositoryFollow _followRepo;
        private readonly IRepositoryLike _postLikeRepo;
        private readonly IRepositoryUserReport _userReportRepo;
        private readonly IRepositoryComment _commentRepo;

        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,ApplicationDbContext context, 
            IRepositoryPost _PostRepo, IRepositoryFollow FollowRepo, IRepositoryLike PostLikeRepo, IRepositoryUserReport userReportRepo, IRepositoryComment commentRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _postRepo = _PostRepo;
            _followRepo = FollowRepo;
            _postLikeRepo = PostLikeRepo;
            _userReportRepo = userReportRepo;
            _commentRepo = commentRepo;
        }
        [HttpGet]
        public IActionResult SearchUsers(string query)
        {
            var userId = _userManager.GetUserId(User);
            // Query database for users matching the search query
            var users = _context.Users
                               .Where(user => user.Id != userId &&
                                   (user.FirstName.Contains(query) ||
                                    user.LastName.Contains(query)))
                               .ToList();

            var userViewModels = users.Select(user => new UserViewModel
            {
                UserId = user.Id,
                UserName = $"{user.FirstName} {user.LastName}",
                HavePicture = (user.ProfilePicture == null) ? false : true,
                UserProfilePictureUrl = user.ProfilePicture ?? $"{user.FirstName[0]}{user.LastName[0]}",
            });

            return PartialView("~/Areas/User/Views/Users/Profile/_UserSearchResults.cshtml", userViewModels);
        }
        [HttpGet]
        public async Task<IActionResult> Profile(string UserId = null)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);
            if (string.IsNullOrEmpty(UserId)) {
                UserId = currentUser.Id;
            }
            var userProfile = _context.Users
                .Where(u => u.Id == UserId)
                .Include(u => u.Posts) // Include user's posts
                .Include(u => u.Comments)
                .Include(u => u.Followers) // Assuming Followers is a collection of Follow entities
                .Include(u => u.Following) // Assuming Following is a collection of Follow entities
                .FirstOrDefault();
            if (userProfile == null)
            {
                return NotFound(); 
            }
            var ProfileSectionVM = new UserProfileViewModel { 
                UserId = UserId,
                IsCurrentUser = UserId == currentUser.Id,
                IsFollowing = UserId != currentUser.Id && _followRepo.IsFollowing(currentUser.Id, UserId),
                Email = userProfile.Email,
                UserName = $"{userProfile.FirstName} {userProfile.LastName}",
                Bio = (userProfile.Bio)??"Please Edit Your Profile and add a Bio",
                HavePicture = (userProfile.ProfilePicture == null)?false:true,
                UserProfilePictureUrl = (userProfile.ProfilePicture == null) ? $"{userProfile.FirstName[0]}{userProfile.LastName[0]}": userProfile.ProfilePicture,
                PostCount = userProfile.Posts.Count(),
                FollowersCount = userProfile.Followers.Count(),
                FollowingCount = userProfile.Following.Count(),
            };
            var Posts = _postRepo.GetUserPostsFilterByCategory(UserId, null);
            var PostsVM = Posts.Select(post => new PostViewModel
            {
                UserId = post.UserId,
                PostId = post.Id,
                UserName = $"{userProfile.FirstName} {userProfile.LastName}",
                HavePicture = (userProfile.ProfilePicture == null) ? false : true,
                UserProfilePictureUrl = (userProfile.ProfilePicture == null) ? $"{userProfile.FirstName[0]}{userProfile.LastName[0]}" : userProfile.ProfilePicture,
                CategoryName = post.Category.Name,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                LikeCount = _postLikeRepo.PostLikesCount(post.Id),
                CommentCount = _commentRepo.GetPostCommentsCount(post.Id),
                IsCurrentUser = UserId == currentUser.Id,
                IsLiked = (_postLikeRepo.GetByUserAndPost(post.Id, currentUser.Id) == null) ? false : true,
            });
            var ProfileVM = new ProfileViewModel
            {
                Profile = ProfileSectionVM,
                Posts = PostsVM
            };
            return View("~/Areas/User/Views/Users/Profile/Profile.cshtml", ProfileVM);
        }
        [HttpGet]
        public IActionResult GetFollowers(string Id)
        {
            var followers = _followRepo.GetFollowers(Id);
            var followersVM = followers.Select(follow => new FollowViewModel()
            {
                UserId = follow.FollowerId,
                UserName = $"{follow.Follower.FirstName} {follow.Follower.LastName}",
                HavePicture = (follow.Follower.ProfilePicture != null) ? true : false,
                UserProfilePictureUrl = (follow.Follower.ProfilePicture) ?? $"{follow.Follower.FirstName[0]}{follow.Follower.LastName[0]}",
            });
            return PartialView("~/Areas/User/Views/Users/Follows/_GetFollowers.cshtml", followersVM);
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
            return PartialView("~/Areas/User/Views/Users/Follows/_GetFollowing.cshtml", followingsVM);
        }
        [HttpPost]
        public IActionResult ToggleFollow(string UserId)
        {
            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId == null || UserId == null)
            {
                return Json(new { success = false, message = "User not authenticated or target user not found." });
            }
            bool IsFollowing = _followRepo.IsFollowing(currentUserId, UserId);
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
            }
            else
            {
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
                    return Json(new { success = false, message = "Failed to follow the user." });
                }

            }
        }
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }
            var model = new EditUserViewModel
            {
                Id = currentUser.Id,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Bio = currentUser.Bio
            };
            return View("~/Areas/User/Views/Users/Btns/_EditProfile.cshtml", model);
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditUserViewModel vm)
        {
            var user = await _userManager.FindByIdAsync(vm.Id);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }
            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;
            user.Bio = vm.Bio;

            if (vm.Image != null) 
            {
                if (user.ProfilePicture != null)
                {
                    FilesSettings.Delete("Images", user.ProfilePicture);
                }
                user.ProfilePicture = FilesSettings.UploadFile(vm.Image, "Images");
            }
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Json(new { success = true, message = "Profile updated successfully!" });
            }
            return Json(new { success = false, errors = result.Errors.Select(e => e.Description) });

        }
        [HttpGet]
        public async Task <IActionResult> ChangePassword()
        {
            return View("~/Areas/User/Views/Users/Btns/_ChangePassword.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            var result = await _userManager.ChangePasswordAsync(user, vm.OldPassword, vm.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return Json(new { success = true, message = "Password changed successfully!" });
            }

            var errors = result.Errors.Select(e => e.Description).ToArray();
            return Json(new { success = false, errors });
        }
        [HttpGet]
        public IActionResult ReportUser(string Id)
        {
            var UserId = _userManager.GetUserId(User);
            if (UserId == null)
            {
                return Json(new { success = false, message = "User not authenticated." });
            }
            if (string.IsNullOrEmpty(Id))
            {
                return Json(new { success = false, message = "Target user not found." });
            }
            UserReportViewModel vm = new UserReportViewModel()
            {
                ReportedId = Id,    
                ReporterId = UserId,
            };
            return PartialView("~/Areas/User/Views/Users/Btns/_ReportUser.cshtml", vm);
        }
        [HttpPost]
        public IActionResult ReportUser(UserReportViewModel vm)
        {
            if (vm.ReporterId == null)
            {
                return Json(new { success = false, message = "User not authenticated." });
            }
            if (string.IsNullOrEmpty(vm.ReportedId))
            {
                return Json(new { success = false, message = "Target user not found." });
            }
            bool hasAlreadyReported = _userReportRepo.HasReported(vm.ReporterId, vm.ReportedId);  // Ensure you have a method in your repository to check this
            if (hasAlreadyReported)
            {
                return Json(new { success = false, message = "You have already reported this user." });
            }
            var userReport = new UserReport
            {
                ReporterId = vm.ReporterId,
                ReportedId = vm.ReportedId,
                ReportDate = DateTime.Now,
                Reason = vm.Reason,
                IsHandled = false,
            };

            try
            {
                _userReportRepo.Add(userReport); 
                return Json(new { success = true, message = "User reported successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return Json(new { success = false, message = "Failed to report user." });
            }
        }
        [HttpGet]
        public IActionResult Home()
        {
            var currentUser =  _userManager.GetUserId(User);
            if (currentUser == null)
            {
                return PartialView("_NotFound");
            }
            var FollowingsId = _followRepo.GetFollowing(currentUser).Select(F => F.FollowingId).ToList();
            if (FollowingsId == null || !FollowingsId.Any())
            {
                //return NotFound();
                return PartialView("_NotFound");
            }
            var Posts = _postRepo.GetFollowingPosts(FollowingsId, null);
            var PostsVm = Posts.Select(post => new PostViewModel
            {
                UserId = post.UserId,
                PostId = post.Id,
                UserName = $"{post.User.FirstName} {post.User.LastName}",
                HavePicture = (post.User.ProfilePicture == null) ? false : true,
                UserProfilePictureUrl = (post.User.ProfilePicture == null) ? $"{post.User.FirstName[0]}{post.User.LastName[0]}" : post.User.ProfilePicture,
                CategoryName = post.Category.Name,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                LikeCount = _postLikeRepo.PostLikesCount(post.Id),
                CommentCount = _commentRepo.GetPostCommentsCount(post.Id),
                IsCurrentUser = currentUser == post.UserId,
                IsLiked = _postLikeRepo.GetByUserAndPost(post.Id, currentUser) != null,
            });
            return PartialView("~/Areas/User/Views/Users/Home/Home.cshtml",PostsVm);
        }

    }
    
}