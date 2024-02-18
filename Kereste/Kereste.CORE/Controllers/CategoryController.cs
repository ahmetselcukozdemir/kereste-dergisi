using Kereste.BLL.Services.Abstract;
using Kereste.CORE.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kereste.CORE.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IContentService _contentService;
        public CategoryController(ICategoryService categoryService, IContentService contentService)
        {
            _categoryService = categoryService;
            _contentService = contentService;
        }

        public IActionResult Index(string categorySelfLink)
        {
            CategoryModel model = new CategoryModel();
            ViewData["ClassName"] = "home-style1 tc-blog-page";

            model.Category = _categoryService.GetCategoryByName(categorySelfLink);

            if (model.Category == null)
            {
                return RedirectToAction("Index","Home");
            }
            model.NewsList = _contentService.GetNewsByCategoryId(model.Category.categoryID,10);

            model.MostPopularNews = _contentService.GetNewsByHitCount(0,4);

            return View(model);
        }
    }
}
