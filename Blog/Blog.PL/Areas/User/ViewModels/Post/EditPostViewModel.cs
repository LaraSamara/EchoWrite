using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.PL.Areas.User.ViewModels.Post
{
    public class EditPostViewModel
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public int CategoryId { get; set; }
        public SelectList? Category { get; set; }
    }
}
