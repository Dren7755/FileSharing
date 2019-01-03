using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using FileSharing.Models.UserModel;
using FileSharing.Models.FileModel;
using FileSharing.ViewModels;
using FileSharing.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace FileSharing.Controllers
{
    public class UserController : Controller
    {
        private DataContext dataContext;

        public UserController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            User currentUser = await dataContext.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
            List<File> files = await dataContext.Files.Where(f => f.User.UserId == currentUser.UserId).ToListAsync();
            foreach(var file in files)
                await dataContext.Entry(file).Reference(f => f.Link).LoadAsync();
            ViewBag.BaseUrl = Request.PathBase;
            return View(files);
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
                    await Authenticate(user);
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
            if (ModelState.IsValid)
            {
                User user = await dataContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if(user == null)
                {
                    user = new User { Email = model.Email, Password = model.Password, Login = model.Login };
                    dataContext.Users.Add(user);
                    await dataContext.SaveChangesAsync();
                    await Authenticate(user);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Такой пользователь уже существует");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email)
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
