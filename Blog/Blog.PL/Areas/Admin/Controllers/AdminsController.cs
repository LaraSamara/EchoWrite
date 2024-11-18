using Blog.BLL.Interfaces;
using Blog.DAL.Data;
using Blog.DAL.Models;
using Blog.PL.Areas.Admin.ViewModels;
using Blog.PL.Areas.User.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminsController : Controller
    {
        private readonly IRepositoryUserReport _userReport;
        private readonly IRepositoryPostReport _postReport;
        private readonly IRepositoryCommentReport _commentReport;
        private readonly ApplicationDbContext _context;
        private readonly IRepositoryComment _commentRepo;
        private readonly IRepositoryPost _postRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepositoryCategory _categoryRepo;

        public AdminsController(IRepositoryUserReport userReport, IRepositoryPostReport postReport ,
            IRepositoryCommentReport commentReport, ApplicationDbContext context, IRepositoryComment commentRepo,
            IRepositoryPost postRepo, UserManager<ApplicationUser> userManager, IRepositoryCategory categoryRepo)
        {
            _userReport = userReport;
            _postReport = postReport;
            _commentReport = commentReport;
            _context = context;
            _commentRepo = commentRepo;
            _postRepo = postRepo;
            _userManager = userManager;
            _categoryRepo = categoryRepo;
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var allUsers = _context.Users.ToList();

            var users = new List<ApplicationUser>();
            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, "User"))
                {
                    users.Add(user);
                }
            }
            var userViewModels = users.Select(user => new UserViewModel
            {
                UserId = user.Id,
                UserName = $"{user.FirstName} {user.LastName}",
                HavePicture = (user.ProfilePicture == null) ? false : true,
                UserProfilePictureUrl = user.ProfilePicture ?? $"{user.FirstName[0]}{user.LastName[0]}",
            });
            var vm = new AdminViewModel()
            {
                UserCount = users.Count,
                UserReportCount = _userReport.UserReportCount(),
                PostCount = _postRepo.PostCount(),
                PostReportCount = _postReport.PostReportCount(),    
                CommentCount = _commentRepo.CommentCount(),
                CommentReportCount = _commentReport.CommentReportCount(),
                Users = userViewModels.ToList(),
            };
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> GetPost(int Id)
        {
            var post = _postRepo.Get(Id);
            if(post == null)
            {
                return NotFound();
            }
            var category = _categoryRepo.Get(post.CategoryId);
            var user = await _userManager.FindByIdAsync(post.UserId);
            if(user == null)
            {
                return NotFound();
            }
            var vm = new PostViewModel
            {
                UserId = post.UserId,
                PostId = post.Id,
                UserName = $"{user.FirstName} {user.LastName}",
                HavePicture = (user.ProfilePicture == null) ? false : true,
                UserProfilePictureUrl = (user.ProfilePicture == null) ? $"{user.FirstName[0]}{user.LastName[0]}" : user.ProfilePicture,
                CategoryName = category.Name,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
            };
            return PartialView("~/Areas/Admin/Views/Admins/_GetPost.cshtml", vm);
        }
        [HttpGet]
        public async Task<IActionResult> GetComment(int Id)
        {
            var post = _commentRepo.Get(Id);
            if (post == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(post.UserId);
            if (user == null)
            {
                return NotFound();
            }
            var vm = new CommentViewModel
            {
                UserId = post.UserId,
                UserName = $"{user.FirstName} {user.LastName}",
                HavePicture = (user.ProfilePicture == null) ? false : true,
                UserProfilePictureUrl = (user.ProfilePicture == null) ? $"{user.FirstName[0]}{user.LastName[0]}" : user.ProfilePicture,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
            };
            return PartialView("~/Areas/Admin/Views/Admins/_GetComment.cshtml", vm);
        }
    }
}

