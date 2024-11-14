using Blog.BLL.Interfaces;
using Blog.DAL.Data;
using Blog.DAL.Models;
using Blog.PL.Areas.User.ViewModels.Comment;
using Blog.PL.Areas.User.ViewModels.Like;
using Blog.PL.Areas.User.ViewModels.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Blog.PL.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepositoryPost _postRepo;
        private readonly IRepositoryCategory _categoryRepo;
        private readonly IRepositoryPostReport _postReportRepo;
        private readonly IRepositoryPostLike _postLikeRepo;
        private readonly IRepositoryComment _commentRepo;

        public PostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,IRepositoryPost PostRepo,
            IRepositoryCategory CategoryRepo, IRepositoryPostReport PostReportRepo, IRepositoryPostLike PostLikeRepo, IRepositoryComment commentRepo)
        {
            _context = context;
            _userManager = userManager;
            _postRepo = PostRepo;
            _categoryRepo = CategoryRepo;
            _postReportRepo = PostReportRepo;
            _postLikeRepo = PostLikeRepo;
            _commentRepo = commentRepo;
        }

        [HttpGet]
        public async Task<IActionResult> CreatePostForm()
        {
            var UserId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(UserId);
            var vm = new CreatePostViewModel {
                UserId = UserId,
                UserName = $"{user.FirstName} {user.LastName}",
                HavePicture = (user.ProfilePicture == null) ? false : true,
                UserProfilePictureUrl = (user.ProfilePicture == null) ? $"{user.FirstName[0]}{user.LastName[0]}" : user.ProfilePicture,
                Category = new SelectList(_categoryRepo.GetAll(), "Id", "Name")
            };
            return PartialView("_AddPostForm", vm);
        }
        [HttpPost]
        public IActionResult CreatePost(CreatePostViewModel vm)
        {
            if (!vm.CategoryId.HasValue)
            {
                ModelState.AddModelError("CategoryId", "Category is required.");
            }
            if (ModelState.IsValid)
            {
                var model = new Post
                {
                    Content = vm.Content,
                    CreatedAt = DateTime.Now,
                    UpdatedAt= DateTime.Now,
                    IsDeleted = false,
                    UserId = vm.UserId,
                    CategoryId = (vm.CategoryId)??1,
                };

                _postRepo.Add(model);
                return Json(new { success = true, message = "Post added successfully!" });
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }
        [HttpPost]
        public IActionResult DeletePost (int Id)
        {
            var post = _postRepo.Get(Id);
            if(post == null)
            {
                return Json(new { success = false, message = "Post not found." });
            }
            _postRepo.Delete(post);
            return Json(new { success = true, message = "Post deleted successfully!" });
        }
        [HttpGet]
        public IActionResult UpdatePost(int Id)
        {
            var Category = new SelectList(_categoryRepo.GetAll(), "Id", "Name");
            var Post = _postRepo.Get(Id);
            if(Post == null)
            {
                return NotFound();
            }
            var vm = new EditPostViewModel
            {
                Id = Post.Id,    
                Category = Category,
                CategoryId = Post.CategoryId,
                Content = Post.Content,
            };

            return PartialView("_UpdatePost", vm);
        }
        [HttpPost]
        public IActionResult UpdatePost(EditPostViewModel vm)
        {
            var post = _postRepo.Get(vm.Id);
            if (post == null)
            {
                return Json(new { success = false, errors = new List<string> { "Post not found." } });
            }
            var model = new Post { 
                Id = vm.Id,
                CreatedAt = post.CreatedAt,
                UpdatedAt = DateTime.Now,
                UserId = post.UserId,
                IsDeleted = post.IsDeleted,
            };
            model.Content = (vm.Content != post.Content) ?  vm.Content : post.Content;
            model.CategoryId = (vm.CategoryId != post.CategoryId)? vm.CategoryId: post.CategoryId; // Update category if changed

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });
            }
            _postRepo.Update(model);
            // Update only the fields that may have changed
            return Json(new { success = true, message = "Post updated successfully!", updatedContent = post.Content });
        }
        [HttpGet]
        public IActionResult ReportPost (int Id)
        {
            var post = _postRepo.Get(Id);
            if (post == null)
            {
                return NotFound();
            }
            var UserId = _userManager.GetUserId(User);
            if(UserId == null)
            {
                return NotFound();
            }
            var vm = new ReportPostViewModel {
                PostId = post.Id,
                UserId = UserId,

            };
            return PartialView("_ReportPost", vm);
        }

        [HttpPost]
        public IActionResult ReportPost(ReportPostViewModel vm)
        {
            var post = _postRepo.Get(vm.PostId);
            if (post == null)
            {
                return NotFound();
            }
            if (vm.UserId == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });
            }
            var model = new PostReport
            {
                ReporterId = vm.UserId,
                PostId = post.Id,
                Reason = vm.Reason,
                IsHandled = false,
                ReportDate = DateTime.Now,
            };
            _postReportRepo.Add(model);
            return Json(new { success = true, message = "Post Reported successfully!", updatedContent = post.Content });
        }
        [HttpPost]
        public IActionResult LikePost(int Id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Json(new { success = false, message = "User not authenticated" });
            }

            var post = _postRepo.Get(Id);
            if (post == null)
            {
                return Json(new { success = false, message = "Post not found" });
            }

            // Check if the post has already been liked by the user
            var existingLike = _postLikeRepo.GetByUserAndPost(Id, userId);
            if (existingLike == null)
            {
                // Add new like
                var postLike = new PostLike { 
                    PostId = Id,
                    UserId = userId 
                };
              _postLikeRepo.Add(postLike);

                return Json(new { success = true, message = "Post liked", likeCount = _postLikeRepo.PostLikesCount(Id)});
            }
            else
            {
                // Optionally handle if you want to allow unliking
                return Json(new { success = false, message = "Already liked this post" });
            }
        }
        [HttpPost]
        public IActionResult UnlikePost(int Id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Json(new { success = false, message = "User not authenticated" });
            }

            var post = _postRepo.Get(Id);
            if (post == null)
            {
                return Json(new { success = false, message = "Post not found" });
            }

            // Check if the post has already been liked by the user
            var existingLike = _postLikeRepo.GetByUserAndPost(Id, userId);
            if (existingLike != null)
            {
                // Remove the like
                _postLikeRepo.Delete(existingLike); // Ensure you have a method to remove the like
                return Json(new { success = true, message = "Post unliked", likeCount = _postLikeRepo.PostLikesCount(Id) });
            }
            else
            {
                return Json(new { success = false, message = "Post was not liked" });
            }
        }
        [HttpGet]
        public IActionResult GetPostLikes(int Id)
        {
            var userId = _userManager.GetUserId(User);
            if(userId == null)
            {
                return NotFound();
            }
            var post = _postRepo.Get(Id);
            if(post == null)
            {
                return NotFound();
            }
            var likes = _postLikeRepo.GetLikesForPost(Id);
            var vm = likes.Select(like => new LikeViewModel
            {
               UserId = like.User.Id,
               UserName = $"{like.User.FirstName} {like.User.LastName}",
               UserProfilePictureUrl = (like.User.ProfilePicture ) ?? $"{like.User.FirstName[0]}{like.User.LastName[0]}",
               IsCurrentId = ( userId == like.Post.UserId),
               HavePicture = like.User?.ProfilePicture != null,
            }); 
            return PartialView("_GetPostLikes", vm);

        }
    }
}
