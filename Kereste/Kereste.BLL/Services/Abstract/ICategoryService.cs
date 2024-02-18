using Kereste.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kereste.BLL.Services.Abstract
{
    public interface ICategoryService
    {
        bool AddCategory(CategoryDTO model);
        int GetCategoryCount();
        List<CategoryDTO> GetAllCategories();
        List<CategoryDTO> GetAllCategories(int count);
        CategoryDTO GetCategoryById(int id);
        CategoryDTO GetCategoryByName(string name);
        bool UpdateCategory(CategoryDTO model);
        bool DeleteCategory(int id);
    }
}
