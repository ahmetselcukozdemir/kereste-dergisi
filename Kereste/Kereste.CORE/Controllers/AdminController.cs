using Kereste.BLL.Services.Abstract;
using Kereste.BLL.Services.Concrete;
using Kereste.DATA.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Kereste.BLL.DTO;
using Kereste.CORE.Models.Admin;

namespace Kereste.CORE.Controllers
{
    public class AdminController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;
        public AdminController(IConfiguration config,IUserService userService,ICategoryService categoryService)
        {
            _config = config;
           _userService = userService;
            _categoryService = categoryService;
        }


		[Authorize]
		public IActionResult Index()
        {

            return View();
        }

        #region Profile
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

                return RedirectToAction("Profile", "Admin");
            }


            return View();
        }
        #endregion


        #region Session
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

                    return RedirectToAction("Index", "Admin");
                }
            }
            ViewData["ErrorMessage"] = "Kullanıcı adınız ve şifreniz hatalı..";
            return RedirectToAction("Login", "Admin");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();

            return RedirectToAction("Login", "Admin");
        }
        #endregion


        #region Category
        public IActionResult Category()
        {
            CategoryModel model = new CategoryModel();

            model.CategoryList = _categoryService.GetAllCategories();

            return View(model);
        }

        public IActionResult AddCategory(CategoryDTO model)
        {
            if (model != null)
            {
                bool check = _categoryService.AddCategory(model);

                if (check == true)
                {
                    return RedirectToAction("Category", "Admin");
                }

            }
            return RedirectToAction("Category", "Admin");
        }

        public IActionResult EditCategory(int categoryID)
        {
            CategoryModel model = new CategoryModel();

            model.Category = _categoryService.GetCategoryById(categoryID);

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateCategory(CategoryDTO model)
        {
            if (model != null)
            {
                bool update = _categoryService.UpdateCategory(model);
                if (update == true)
                {
                    return RedirectToAction("Category", "Admin");
                }
            }
            return RedirectToAction("Category", "Admin");
        }

        public IActionResult DeleteCategory(int categoryID)
        {
            bool check = _categoryService.DeleteCategory(categoryID);
            if (check == true)
            {
                return RedirectToAction("Category", "Admin");
            }
            return RedirectToAction("Category", "Admin");
        }
        #endregion



        #region news
        public IActionResult AddNews()
        {
            NewsModel model = new NewsModel();

            model.CategoryList = _categoryService.GetAllCategories();

            return View(model);
        }
        [HttpPost]
        public IActionResult AddNews(NewsDTO model,IFormFile headImage, IFormFile detailImage)
        {
            return View();
        }


        #endregion

        #region utils

        public JsonResult UploadImage()
        {
            DefaultClass rd = new DefaultClass();
            try
            {
                var file = HttpContext.Request.Form.Files["file"];

                if (file.ContentType.ToLower().Contains("jpg") || file.ContentType.ToLower().Contains("png") || file.ContentType.ToLower().Contains("jpeg"))
                {
                    string fileName = UploadFileAjax(file, "GalleryPath");
                    rd.imgDefault = _config["CDNURL"] + "/gallery/" + fileName;
                    return Json(rd);
                }


            }
            catch (Exception ex)
            {
                rd.imgDefault = ex.InnerException.Message;
            }

            return Json(rd);
        }

        public class DefaultClass
        {
            public string imgDefault { get; set; }
        }

        private string UploadFileAjax(IFormFile file, string path)
        {
            string fileName;
            var qq = "." + file.FileName.ToString().Split('.').Last();
            fileName = Guid.NewGuid().ToString().Substring(0, 8) + qq;

            var filePath = Path.Combine(_config[path], fileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }

        #endregion
    }
}
