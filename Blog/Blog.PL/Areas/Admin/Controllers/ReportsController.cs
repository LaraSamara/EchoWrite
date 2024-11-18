using Blog.BLL.Interfaces;
using Blog.PL.Areas.Admin.ViewModels;
using Blog.PL.Areas.User.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blog.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IRepositoryUserReport _userReport;
        private readonly IRepositoryPostReport _postReport;
        private readonly IRepositoryCommentReport _commentReport;
        public ReportsController(IRepositoryUserReport userReport, IRepositoryPostReport postReport,
            IRepositoryCommentReport commentReport) {
            _userReport = userReport;
            _postReport = postReport;
            _commentReport = commentReport;
        }
        [HttpGet]
        public IActionResult GetUserReports()
        {
            try
            {
                var reports = _userReport.GetAll();

                if (reports == null || !reports.Any())
                {
                    return Json(new { success = false, message = "No reports found." });
                }
                
                var ReportsVM = reports.Select(report =>
                    new ReportViewModel
                    {
                        Id = report.Id,
                        Reporter = new User.ViewModels.User.UserViewModel
                        {
                            UserId = report.Reporter.Id,
                            UserName = $"{report.Reporter.FirstName} {report.Reporter.LastName}",
                            HavePicture = report.Reporter.ProfilePicture != null,
                            UserProfilePictureUrl = report.Reporter.ProfilePicture?? $"{report.Reporter.FirstName[0]}{report.Reporter.LastName[0]}",
                        },
                        ReportDate = report.ReportDate,
                        Reason = report.Reason,
                        ReportedId = report.ReportedId,
                    }).ToList();
                ViewBag.type = "user";
                return PartialView("~/Areas/Admin/Views/Reports/_GetReport.cshtml", ReportsVM);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while fetching user reports. Please try again later."});
            }
        }
        [HttpGet]
        public IActionResult GetPostReports()
        {
            try
            {
                var reports = _postReport.GetAll();

                if (reports == null || !reports.Any())
                {
                    return Json(new { success = false, message = "No reports found." });
                }

                var ReportsVM = reports.Select(report =>
                    new ReportViewModel
                    {
                        Id = report.Id,
                        Reporter = new UserViewModel
                        {
                            UserId = report.Reporter.Id,
                            UserName = $"{report.Reporter.FirstName} {report.Reporter.LastName}",
                            HavePicture = report.Reporter.ProfilePicture != null,
                            UserProfilePictureUrl = report.Reporter.ProfilePicture ?? $"{report.Reporter.FirstName[0]}{report.Reporter.LastName[0]}",
                        },
                        ReportDate = report.ReportDate,
                        Reason = report.Reason,
                        PostId = report.PostId,
                    }).ToList();
                ViewBag.type = "post";
                return PartialView("~/Areas/Admin/Views/Reports/_GetReport.cshtml", ReportsVM);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while fetching user reports. Please try again later." });
            }
        }
        [HttpGet]
        public IActionResult GetCommentReports()
        {
            try
            {
                var reports = _commentReport.GetAll();

                if (reports == null || !reports.Any())
                {
                    return Json(new { success = false, message = "No reports found." });
                }

                var ReportsVM = reports.Select(report =>
                    new ReportViewModel
                    {
                        Id = report.Id,
                        Reporter = new UserViewModel
                        {
                            UserId = report.Reporter.Id,
                            UserName = $"{report.Reporter.FirstName} {report.Reporter.LastName}",
                            HavePicture = report.Reporter.ProfilePicture != null,
                            UserProfilePictureUrl = report.Reporter.ProfilePicture ?? $"{report.Reporter.FirstName[0]}{report.Reporter.LastName[0]}",
                        },
                        ReportDate = report.ReportDate,
                        Reason = report.Reason,
                        PostId = report.Comment.PostId,
                        CommentId = report.CommentId,
                    }).ToList();
                ViewBag.type = "comment";
                return PartialView("~/Areas/Admin/Views/Reports/_GetReport.cshtml", ReportsVM);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while fetching user reports. Please try again later." });
            }
        }
        [HttpPost]
        public IActionResult HandleReport(int Id, string Type)
        {
            if (Type == "user")
            {
                var report = _userReport.Get(Id);
                if (report == null)
                {
                    return Json(new { success = false, message = "Report not found." });
                }
                try
                {
                    _userReport.Delete(report);
                    return Json(new { success = true, message = "Report Handled successfully." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "An error occurred while handling the report. Please try again." });
                }
            }
            else if (Type == "post")
            {
                var report = _postReport.Get(Id);
                if (report == null)
                {
                    return Json(new { success = false, message = "Report not found." });
                }
                try
                {
                    _postReport.Delete(report);
                    return Json(new { success = true, message = "Report Handled successfully." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "An error occurred while handling the report. Please try again." });
                }
            }
            else
            {
                var report = _commentReport.Get(Id);
                if (report == null)
                {
                    return Json(new { success = false, message = "Report not found." });
                }
                try
                {
                    _commentReport.Delete(report);
                    return Json(new { success = true, message = "Report Handled successfully." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "An error occurred while handling the report. Please try again." });
                }
            }
        }


    }
}
