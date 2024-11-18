using Blog.BLL.Interfaces;
using Blog.DAL.Models;
using Blog.PL.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly IRepositoryCategory _categoryRepo;

        public CategoriesController(IRepositoryCategory CategoryRepo)
        {
            _categoryRepo = CategoryRepo;
        }
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View("_CreateCategory");
        }
        [HttpPost]
        public IActionResult CreateCategory(CategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var model = new Category()
                {
                    Name = vm.Name,
                };
                try
                {
                    _categoryRepo.Add(model);
                    return Json(new { success = true, message = "Category created successfully!" });
                }
                catch (Exception ex)
                {
                    // Log the exception (optional)
                    Console.WriteLine($"Error creating category: {ex.Message}");
                    return Json(new { success = false, message = "An error occurred while creating the category." });
                }
            }

            return Json(new { success = false, message = "Invalid data provided." });
        }
        [HttpGet]
        public IActionResult GetCategories() {
            var categories = _categoryRepo.GetAll();
            var vm = categories.Select(category => new CategoryViewModel
            {
                Name=category.Name,
            });
            return View("_GetCategories", vm);
        }
    }
}
