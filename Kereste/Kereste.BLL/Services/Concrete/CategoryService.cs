using Kereste.BLL.DTO;
using Kereste.BLL.Helpers;
using Kereste.BLL.Services.Abstract;
using Kereste.DATA.Contexts;
using Kereste.DATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kereste.BLL.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        public KeresteDBContext _context;

        public CategoryService(KeresteDBContext context)
        {
            _context = context;
        }
        public bool AddCategory(CategoryDTO model)
        {
            if (model != null)
            {
                Category category = new Category();
                category.Name = model.categoryName;
                category.isActive = model.isActive;
                category.Description = model.categoryDesc;
                category.Image = model.categoryImg;
                category.SelfLink = Utils.ToSeoUrl(model.categoryName);
                _context.Categories.Add(category);
                _context.SaveChanges();
                return true;
            }
            else { return false; }
        }

        public bool DeleteCategory(int id)
        {
            var check = _context.Categories.Where(x => x.ID == id).FirstOrDefault();
            if (check != null)
            {
                check.isActive = false;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<CategoryDTO> GetAllCategories()
        {
            return _context.Categories.Select(x=> new CategoryDTO
            {
                categoryDesc = x.Description,
                categoryID = x.ID,
                categoryImg = x.Image,
                categoryName = x.Name,
                isActive = x.isActive
            }).ToList();
        }

        public CategoryDTO GetCategoryById(int id)
        {
            return _context.Categories.Where(x=> x.ID == id).Select(x => new CategoryDTO
            {
                categoryDesc = x.Description,
                categoryID = x.ID,
                categoryImg = x.Image,
                categoryName = x.Name,
                isActive = x.isActive
            }).FirstOrDefault();
        }

        public bool UpdateCategory(CategoryDTO model)
        {
            if (model != null)
            {
                var checkCategory = _context.Categories.Where(x => x.ID == model.categoryID).FirstOrDefault();
                if (checkCategory != null)
                {
                    checkCategory.Name = model.categoryName;
                    checkCategory.isActive = model.isActive;
                    checkCategory.Description = model.categoryDesc;
                    checkCategory.SelfLink = Utils.ToSeoUrl(model.categoryName);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
    }
}
