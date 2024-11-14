namespace Blog.PL.Areas.User.ViewModels.User
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ImageName { get; set; }
        public IFormFile? Image { get; set; }
        public string? Bio { get; set; }

    }
}
