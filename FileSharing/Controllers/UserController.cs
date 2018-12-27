using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using FileSharing.Models;
using FileSharing.ViewModels;
using FileSharing.Infrastructure;
using Microsoft.EntityFrameworkCore;


namespace FileSharing.Controllers
{
    public class UserController : Controller
    {
        private DataContext dataContext;

        public UserController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await dataContext.Users.ToListAsync());
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(ModelState.IsValid)
            {
                User user = await dataContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if(user != null)
                {
                    await Authenticate(model.Email);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Неверный логин или пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegisterModel model)
        {
            if(ModelState.IsValid)
            {
                User user = await dataContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if(user == null)
                {
                    dataContext.Users.Add(new User { Email = model.Email, Password = model.Password, Login = model.Login });
                    await dataContext.SaveChangesAsync();
                    await Authenticate(model.Email);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Такой пользователь уже существует");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task Authenticate(string email)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, email)
            };
            ClaimsIdentity id = new ClaimsIdentity(
                claims,
                "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
            );
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

    }
}
