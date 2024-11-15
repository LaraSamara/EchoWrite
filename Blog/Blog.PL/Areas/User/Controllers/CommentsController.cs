using Blog.BLL.Interfaces;
using Blog.DAL.Models;
using Blog.PL.Areas.User.ViewModels.Comment;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.PL.Areas.User.Controllers
{
    [Area("User")]
    public class CommentsController : Controller
    {
        private readonly IRepositoryComment _commentRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepositoryPost _postRepo;
        private readonly IRepositoryCommentReport _commentReportRepo;

        public CommentsController(IRepositoryComment CommentRepo, UserManager<ApplicationUser> userManager, IRepositoryPost PostRepo,
            IRepositoryCommentReport CommentReportRepo)
        {
            _commentRepo = CommentRepo;
            _userManager = userManager;
            _postRepo = PostRepo;
            _commentReportRepo = CommentReportRepo;
        }
        [HttpGet]
        public IActionResult CreateComment(int Id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return NotFound();
            }
            var post = _postRepo.Get(Id);
            if (post == null)
            {
                return NotFound();
            }
            var vm = new CreateCommentViewModel
            {
                PostId = Id,
                UserId = userId,
            };
            return PartialView("~/Areas/User/Views/Comments/_CreateComment.cshtml", vm);
        }
        [HttpPost]
        public IActionResult CreateComment(CreateCommentViewModel vm)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return NotFound();
            }
            var post = _postRepo.Get(vm.PostId);
            if (post == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {

                return Json(new { success = false, message = "Invalid data.", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            // Create a new Comment entity from the ViewModel
            var comment = new Comment
            {
                UserId = userId,
                PostId = vm.PostId,
                Content = vm.Content,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            // Add the comment to the repository
            _commentRepo.Add(comment);
            // Return a success response with the created comment data
            return Json(new { success = true, message = "Post Comments", CommentsCount = _commentRepo.GetPostCommentsCount(vm.PostId) });

        }
        [HttpGet]
        public IActionResult GetPostComments(int Id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return NotFound();
            }
            var models = _commentRepo.GetPostComments(Id);
            var vm = models.Select(comment => new GetCommentViewModel
            {
                Id = comment.Id,
                IsCurrentUser = userId == comment.UserId,
                UserId = comment.UserId,
                UserName = $"{comment.User.FirstName} {comment.User.LastName}",
                HavePicture = comment.User.ProfilePicture == null ? false : true,
                UserProfilePictureUrl = comment.User.ProfilePicture ?? $"{comment.User.FirstName[0]}{comment.User.LastName[0]}",
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,

            });
            return PartialView("~/Areas/User/Views/Comments/_GetPostComments.cshtml", vm);
        }
        [HttpPost]
        public IActionResult Delete(int Id)
        {
            var comment = _commentRepo.Get(Id);
            if(comment == null)
            {
                return NotFound();  
            }
            try
            {
                _commentRepo.Delete(comment);
                return Json(new { success = true, message = "Comment successfully deleted.", CommentsCount = _commentRepo.GetPostCommentsCount(comment.PostId) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while deleting the comment." });
            }
        }
        [HttpGet]
        public IActionResult UpdateComment(int Id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return NotFound();
            }
            var comment = _commentRepo.Get(Id);
            if (comment == null)
            {
                return NotFound();
            }
            var post = _postRepo.Get(comment.PostId);
            if (post == null)
            {
                return NotFound();
            }
            var vm = new UpdateCommentViewModel()
            {
                Content = comment.Content,
                PostId = comment.PostId,
                CommentId = comment.Id,
                UserId = comment.UserId
            };
            return View("~/Areas/User/Views/Comments/_UpdateComment.cshtml", vm);
        }
        [HttpPost]
        public IActionResult UpdateComment(UpdateCommentViewModel vm)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Json(new { success = false, message = "User not found." });
            }
            var comment = _commentRepo.Get(vm.CommentId);
            if (comment == null)
            {
                return Json(new { success = false, message = "Comment not found." });
            }
            var post = _postRepo.Get(vm.PostId);
            if (post == null)
            {
                return Json(new { success = false, message = "Post not found." });
            }
            var model = new Comment() {
                Id = vm.CommentId,
                PostId =vm.PostId,
                UserId=vm.UserId,
                Content = vm.Content,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = DateTime.Now,
                IsDeleted = false,
            };
            try
            {
                _commentRepo.Update(model);
                return Json(new { success = true, message = "Comment updated successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception here if necessary
                return Json(new { success = false, message = "An error occurred while updating the comment." });
            }
        }
        [HttpGet]
        public IActionResult ReportComment(int Id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Json(new { success = false, message = "User not found." });
            }
            var comment = _commentRepo.Get(Id);
            if (comment == null)
            {
                return Json(new { success = false, message = "Comment not found." });
            }
            var post = _postRepo.Get(comment.PostId);
            if (post == null)
            {
                return Json(new { success = false, message = "Post not found." });
            }
            var vm = new ReportCommentViewModel()
            {
                UserId = comment.UserId,
                CommentId = comment.Id,
            };
            return View("~/Areas/User/Views/Comments/_ReportComment.cshtml", vm);
        }
        [HttpPost]
        public IActionResult ReportComment(ReportCommentViewModel vm)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Json(new { success = false, message = "User not found." });
            }
            var comment = _commentRepo.Get(vm.CommentId);
            if (comment == null)
            {
                return Json(new { success = false, message = "Comment not found." });
            }
            var post = _postRepo.Get(comment.PostId);
            if (post == null)
            {
                return Json(new { success = false, message = "Post not found." });
            }
            var model = new CommentReport()
            {
                CommentId = comment.Id,
                Reason = vm.Reason,
                IsHandled = false,
                ReportDate = DateTime.Now,
                ReporterId = userId,
            };
            try
            {
                _commentReportRepo.Add(model);
                return Json(new { success = true, message = "Comment reported successfully." });
            }
            catch (Exception ex) {
                return Json(new { success = false, message = "An error occurred while reporting the comment." });
            }
        }

    }
}
