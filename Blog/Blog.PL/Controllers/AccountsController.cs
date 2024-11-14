using AutoMapper;
using Blog.DAL.Models;
using Blog.PL.Helpers;
using Blog.PL.ViewModel.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace Blog.PL.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignupViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(vm.Email);

                // Check if the email already exists
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Email already exists.");
                    return View(vm);
                }
                var user = new ApplicationUser { 
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    Email = vm.Email,
                    Address = vm.Address,
                    UserName = vm.Email,
                };
                var result = await _userManager.CreateAsync(user, vm.Password);
                await _userManager.AddToRoleAsync(user, "User");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var confirmEmailURL = Url.Action("ConfirmEmail", "Accounts", new { Email = vm.Email, token = token }, Request.Scheme, Request.Host.ToString());
                    var email = new Email
                    {
                        To = vm.Email,
                        Subject = "Confirm Email",
                        Body = $"Please confirm your account by clicking this link: <a href='{confirmEmailURL}'>link</a>"
                    };
                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(ReciveEmail));
                }
                // If account creation fails, add errors to the model
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            if (email == null || token == null)
            {
                return BadRequest(); 
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest();
            }
            user.EmailConfirmed = true;
            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                return View();
            }
                // Handle update errors
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            
            return BadRequest(); // Redirect to a failure page or view
        }
        [HttpGet]
        public IActionResult Signin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signin(SigninViewModel vm)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(vm.Email);
                if (user is not null)
                {
                    var check = await _userManager.CheckPasswordAsync(user, vm.Password);
                    if(check)
                    {
                        var result = await _signinManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, false);
                        if(result.Succeeded)
                        {
                            if (await _userManager.IsInRoleAsync(user, "User"))
                            {
                                // Redirect to Profile action in Users controller within the User area
                                return RedirectToAction("Profile", "Users", new { Area = "User" });
                            }
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = await  _userManager.FindByEmailAsync(vm.Email);
                if(user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var ResetPasswordURL = Url.Action("NewPassword", "Accounts", new { Email = vm.Email, token = token }, Request.Scheme, Request.Host.ToString());
                    var email = new Email
                    {
                        To = vm.Email,
                        Subject = "Reset Password",
                        Body = $"Please Reset your Password by clicking this link: <a href='{ResetPasswordURL}'>link</a>"
                    };
                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(ReciveEmail));
                }
            }
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> NewPassword(string Email, string Token)
        {
            if (Email == null || Token == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return BadRequest();
            }
            var vm = new NewPasswordViewModel
            {
                Email = Email,
                Token = Token
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewPassword(NewPasswordViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(vm.Email);
                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, vm.Token, vm.NewPassword);
                    if (result.Succeeded)
                    {
                        return View("ChangePasswordSuccesfull");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                 ModelState.AddModelError(string.Empty, "User not found.");
            }
            return View(vm);
        }
        [HttpGet]
        public IActionResult ReciveEmail()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Succesfull()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Signout()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
