using Kereste.BLL.Services.Abstract;
using Kereste.BLL.Services.Concrete;
using Kereste.DATA.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Kereste.BLL.DTO;

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
				new Claim(ClaimTypes.Name, user.NameSurname),
				new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
				new Claim(ClaimTypes.Role, user.isAdmin == true ? "1" : "0"),
                new Claim(ClaimTypes.Uri, user.Image != null ? user.Image : "")
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

		[Authorize]
		public IActionResult Profile()
        {
            ClaimsPrincipal user = HttpContext.User;

            string userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			User userModel = _userService.GetUserById(Convert.ToInt32(userId));

            return View(userModel);
		}

        [HttpPost]
        public IActionResult UpdateProfile(IFormFile formFile)
		{
			UserDTO dto = new UserDTO();
            dto.nameSurname = HttpContext.Request.Form["namesurname"].ToString();
            dto.userName = HttpContext.Request.Form["username"].ToString();
            dto.password = HttpContext.Request.Form["password"].ToString();
            dto.email = HttpContext.Request.Form["email"].ToString();
            dto.userID = Convert.ToInt32(HttpContext.Request.Form["userID"]);
          
            if (formFile.Length > 0)
			{
                //var imagePath = "/uploads/" + Guid.NewGuid().ToString() + "_" + formFile.FileName;
                var fileName = $"{dto.userID}{Path.GetExtension(formFile.FileName)}";
                var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "cdn/users", fileName);

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    formFile.CopyTo(stream);
                }

                dto.image = fileName;
            }



		    bool update = _userService.UpdateUser(dto);
			if (update == true)
			{
				HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, dto.nameSurname),
                new Claim(ClaimTypes.NameIdentifier, dto.userID.ToString()),
                new Claim(ClaimTypes.Role, dto.isAdmin == true ? "1" : "0"),
                new Claim(ClaimTypes.Uri, dto.image != null ? dto.image : "")
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

                return RedirectToAction("Profile","Admin");
			}


            return View();
		}

		public IActionResult Logout()
		{
            HttpContext.SignOutAsync();

            return RedirectToAction("Login", "Admin");
        }
    }
}
