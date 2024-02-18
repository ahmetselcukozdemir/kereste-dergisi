using Kereste.BLL.Services.Abstract;
using Kereste.CORE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Kereste.CORE.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IContentService _contentService;

        public HomeController(IContentService contentService)
        {
            _contentService = contentService;
        }

        public IActionResult Index()
        {
            ViewData["ClassName"] = "home-style1";
            HomeModel model = new HomeModel();

            model.BreakingNews = _contentService.GetNews(10);

            return View(model);
        }

        public IActionResult About()
        {
            ViewData["ClassName"] = "home-style1 tc-about-page pace-running";
            return View();
        }

        public IActionResult Team()
        {
            ViewData["ClassName"] = "home-style1 tc-team-page";
            return View();
        }


        public IActionResult UpdateCountNews(int newsID)
        {
            var check = _contentService.UpdateContentRating(newsID);
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
