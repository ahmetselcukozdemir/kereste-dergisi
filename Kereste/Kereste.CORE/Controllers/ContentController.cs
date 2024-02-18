using Kereste.BLL.Services.Abstract;
using Kereste.CORE.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kereste.CORE.Controllers
{
    public class ContentController : Controller
    {
        private readonly IContentService _contentService;

        public ContentController(IContentService contentService)
        {
          _contentService = contentService;
        }

        public IActionResult Details(string categoryName, string selflink, int? id)
        {
            ViewData["ClassName"] = "home-style1 tc-single-post-creative-page pace-running";
            ContentDetailModel model = new ContentDetailModel();

            model.News = _contentService.GetNewsByID(id.Value);

            model.EditorNews = _contentService.GetNewsByUser(model.News.User.ID,id.Value,4);

            model.InterestedNews = _contentService.GetInterestedNews(id.Value, 4);

            model.MostNews = _contentService.GetNewsByHitCount(id.Value,4);

            return View(model);
        }
    }
}
