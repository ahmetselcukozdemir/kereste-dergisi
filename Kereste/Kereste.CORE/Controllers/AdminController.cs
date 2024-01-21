using Kereste.BLL.Services.Abstract;
using Kereste.BLL.Services.Concrete;
using Kereste.DATA.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Kereste.CORE.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        public AdminController(IUserService userService)
        {
           _userService = userService;
        }

		[AllowAnonymous]
		public IActionResult Login()
        {
            return View();  
        }

		[HttpPost]
		public IActionResult LoginPost(string username, string pass)
		{
			if (username != null && pass != null)
			{
				User user = _userService.GetUserByUser(username, pass);

				if (user != null)
				{
					var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.NameIdentifier, user.ID.ToString())
               
            };

					var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

					var authProperties = new AuthenticationProperties
					{
						ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
						IsPersistent = true
					};

					HttpContext.SignInAsync(
						CookieAuthenticationDefaults.AuthenticationScheme,
						new ClaimsPrincipal(claimsIdentity),
						authProperties);
					
					return RedirectToAction("Index","Admin");
				}
			}
			ViewData["ErrorMessage"] = "Kullanıcı adınız ve şifreniz hatalı..";
			return RedirectToAction("Login", "Admin");
		}

		[Authorize]
		public IActionResult Index()
        {
            return View();
        }
    }
}
