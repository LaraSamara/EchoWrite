using Blog.BLL.Interfaces;
using Blog.PL.Areas.User.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

namespace Blog.PL.Areas.ViewComponents
{
    public class CategoryListViewComponent : ViewComponent
    {
        private readonly IRepositoryCategory _categoryRepo;

        public CategoryListViewComponent(IRepositoryCategory CategoryRepo)
        {
            _categoryRepo = CategoryRepo;
        }
        public async Task<IViewComponentResult> InvokeAsync(string UserId = null)
        {
            var CategoriesVM = _categoryRepo.GetAll().Select(category => new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
            }).ToList();
            ViewBag.UserId = UserId;
            return View(CategoriesVM);
        }
    }
}
