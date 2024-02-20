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
using Microsoft.EntityFrameworkCore;
using Kereste.DATA.Contexts;

namespace Kereste.CORE.Controllers
{
    public class AdminController : Controller
    {
        private KeresteDBContext _context;
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;
        private readonly IDocumentService _documentService;
        private readonly IContentService _contentService;
        public AdminController(KeresteDBContext context,IConfiguration config,IUserService userService,ICategoryService categoryService,IDocumentService documentService,IContentService contentService)
        {
            _context = context;
            _config = config;
           _userService = userService;
            _categoryService = categoryService;
            _documentService = documentService;
            _contentService = contentService;
        }


		[Authorize]
		public IActionResult Index()
        {
            Models.Admin.HomeModel model = new Models.Admin.HomeModel();
            model.CategoryCount = _categoryService.GetCategoryCount();
            model.UserCount = _userService.GetUserCount();
            model.NewsCount = _contentService.GetNewsCount();
            model.LastNewsList = _contentService.GetNews(10);
            model.LastCategoryList = _categoryService.GetAllCategories(10);
            model.LastUserList = _userService.GetAllUsers(10);

            return View(model);
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
            string imageProfile = "";


            if (formFile.Length > 0)
            {
                imageProfile = _documentService.CopyImageProfile(formFile, "UserImagePathUpload");
                dto.image = imageProfile;
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
        [Authorize]
        public IActionResult Category()
        {
            CategoryModel model = new CategoryModel();

            model.CategoryList = _categoryService.GetAllCategories();

            return View(model);
        }

        [Authorize]
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
        [Authorize]
        public IActionResult EditCategory(int categoryID)
        {
            CategoryModel model = new CategoryModel();

            model.Category = _categoryService.GetCategoryById(categoryID);

            return View(model);
        }

        [Authorize]
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
        [Authorize]
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
        [Authorize]
        public IActionResult AddNews()
        {
            NewsModel model = new NewsModel();

            model.CategoryList = _categoryService.GetAllCategories();

            return View(model);
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddNews(NewsDTO model,IFormFile headImage, IFormFile detailImage)
        {
            ClaimsPrincipal user = HttpContext.User;

            string userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            User userModel = _userService.GetUserById(Convert.ToInt32(userId));


            string headImageLink = "";
            string detailImageLink = "";

            if (headImage != null)
            {
                headImageLink = _documentService.CopyImage(headImage, "ImagePathUpload");
            }

            if (detailImage != null)
            {
                detailImageLink = _documentService.CopyImage(detailImage, "ImagePathUpload");
            }

            var title = HttpContext.Request.Form["title"];
            var alternatetitle = HttpContext.Request.Form["alternatetitle"];
            var spot = HttpContext.Request.Form["spot"];
            var body = HttpContext.Request.Form["body"];
            var tags = HttpContext.Request.Form["tags"];
            var publishdate = Convert.ToDateTime(HttpContext.Request.Form["publishdate"]);
            var categoryid = Convert.ToInt32(HttpContext.Request.Form["categoryid"]);
            var category = _context.Categories.FirstOrDefault(c => c.ID == categoryid);
            var activestatus = HttpContext.Request.Form["activestatus"];
            var external = HttpContext.Request.Form["external"];
            string selflink = Kereste.BLL.Helpers.Utils.ToSeoUrl(title);

            News news = new News();

            news.Title = title;
            news.AlternativeTitle = alternatetitle;
            news.Spot = spot;
            news.Body = body;
            news.Tags = tags.ToString();
            news.Category = category;
            news.HeadImage = headImageLink;
            news.VerticalImage = detailImageLink;
            news.PublishDate = publishdate;
            news.InsertedDate = DateTime.Now;
            news.Status = activestatus == "true" ? 1 : 0;
            news.SelfLink = selflink;
            news.User = userModel;
            news.ExternalLink = external;
            var boolCheck = _contentService.AddContent(news);
            if (boolCheck == true)
            {
                return RedirectToAction("NewsList","Admin");
            }
            return RedirectToAction("AddNews", "Admin");
        }
        [Authorize]
        public IActionResult EditNews(int newsID)
        {
            NewsModel model = new NewsModel();
            model.News = _contentService.GetNewsByID(newsID);
            model.CategoryList = _categoryService.GetAllCategories();
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public IActionResult UpdateNews(NewsDTO model, IFormFile headImage, IFormFile detailImage)
        {
            ClaimsPrincipal user = HttpContext.User;

            string userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            User userModel = _userService.GetUserById(Convert.ToInt32(userId));


            string headImageLink = "";
            string detailImageLink = "";

            if (headImage != null)
            {
                headImageLink = _documentService.CopyImage(headImage, "ImagePathUpload");
            }

            if (detailImage != null)
            {
                detailImageLink = _documentService.CopyImage(detailImage, "ImagePathUpload");
            }

            var title = HttpContext.Request.Form["title"];
            var alternatetitle = HttpContext.Request.Form["alternatetitle"];
            var spot = HttpContext.Request.Form["spot"];
            var body = HttpContext.Request.Form["body"];
            var tags = HttpContext.Request.Form["tags"];
            var publishdate = Convert.ToDateTime(HttpContext.Request.Form["publishdate"]);
            var categoryid = Convert.ToInt32(HttpContext.Request.Form["categoryid"]);
            var category = _context.Categories.FirstOrDefault(c => c.ID == categoryid);
            var activestatus = HttpContext.Request.Form["activestatus"];
            string selflink = Kereste.BLL.Helpers.Utils.ToSeoUrl(title);
            var oldImageDetail = HttpContext.Request.Form["oldImageDetail"];
            var oldImageHead = HttpContext.Request.Form["oldImageHead"];
            var newsID = Convert.ToInt32(HttpContext.Request.Form["newsID"]);
            var external = HttpContext.Request.Form["external"];
            News news = new News();
            news.ID = newsID;
            news.Title = title;
            news.AlternativeTitle = alternatetitle;
            news.Spot = spot;
            news.Body = body;
            news.Tags = tags.ToString();
            news.Category = category;
            news.HeadImage = headImage != null ? headImageLink : oldImageHead;
            news.VerticalImage = detailImageLink != null ? detailImageLink : oldImageHead;
            news.PublishDate = publishdate;
            news.InsertedDate = DateTime.Now;
            news.Status = activestatus == "true" ? 1 : 0;
            news.SelfLink = selflink;
            news.User = userModel;
            news.ExternalLink = external;
            var boolCheck = _contentService.UpdateContent(news);
            if (boolCheck == true)
            {
                return RedirectToAction("NewsList", "Admin");
            }
            return RedirectToAction("AddNews", "Admin");
        }
        [Authorize]
        [HttpGet]
        public IActionResult NewsList(int page=0,int count=30,int? categorySelect=null,string? keyword = null)
        {
            NewsModel model = new NewsModel();

            ClaimsPrincipal user = HttpContext.User;

            string userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            User userModel = _userService.GetUserById(Convert.ToInt32(userId));
            
            model.NewsList = _contentService.GetNews(userModel.ID,count,page);

            model.CategoryList = _categoryService.GetAllCategories();

           

            return View(model);
        }

        #endregion

        #region utils
        [Authorize]
        public JsonResult UploadImage()
        {
            DefaultClass rd = new DefaultClass();
            try
            {
                var file = HttpContext.Request.Form.Files["file"];

                if (file.ContentType.ToLower().Contains("jpg") || file.ContentType.ToLower().Contains("png") || file.ContentType.ToLower().Contains("jpeg"))
                {
                    string fileName = _documentService.CopyImage(file, "ImagePathUpload");
                    rd.imgDefault = _config["ImagePath"] + fileName;
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
