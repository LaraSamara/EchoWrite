using AutoMapper;
using Blog.DAL.Models;
using Blog.PL.ViewModel.Accounts;

namespace Blog.PL.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {
            CreateMap<SignupViewModel, ApplicationUser>();
        }
    }
}
