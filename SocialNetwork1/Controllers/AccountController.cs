using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork1.Data;
using SocialNetwork1.Entities;
using SocialNetwork1.Models;
using SocialNetwork1.Services;

namespace SocialNetwork1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly RoleManager<CustomIdentityRole> _roleManager;
        private readonly SignInManager<CustomIdentityUser> _signInManager;
        private readonly SocialNetworkDbContext _context;
        private readonly IImageService _imageService;

        public AccountController(UserManager<CustomIdentityUser> userManager,
            RoleManager<CustomIdentityRole> roleManager,
            SignInManager<CustomIdentityUser> signInManager,
            SocialNetworkDbContext context,
            IImageService imageService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _imageService = imageService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.File != null)
                {
                    model.ImageUrl = await _imageService.SaveFileAsync(model.File);
                }

                CustomIdentityUser user = new CustomIdentityUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Image = model.ImageUrl
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    if(!await _roleManager.RoleExistsAsync("Admin"))
                    {
                        CustomIdentityRole role = new CustomIdentityRole
                        {
                            Name = "Admin"
                        };

                        IdentityResult roleResult = await _roleManager.CreateAsync(role);
                        if (!roleResult.Succeeded)
                        {
                            return View(model);
                        }
                    }

                    await _userManager.AddToRoleAsync(user, "Admin");
                    return RedirectToAction("Login", "Account");

                }
            }

            return View(model);
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    var user =await _context.Users.SingleOrDefaultAsync(u => u.UserName == model.Username);
                    if (user != null)
                    {
                        user.ConnectTime=DateTime.Now.ToLongDateString()+" "+DateTime.Now.ToShortDateString();
                        user.IsOnline = true;
                        _context.Users.Update(user);
                        await _context.SaveChangesAsync();
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid Login");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if(user != null)
            {
                user.DisconnectTime = DateTime.Now;
                user.IsOnline=false;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                await _signInManager.SignOutAsync();
            }
            return RedirectToAction("Login", "Account");
        }
    }
}
