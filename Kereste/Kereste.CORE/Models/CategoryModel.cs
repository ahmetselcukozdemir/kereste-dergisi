using Kereste.BLL.DTO;

namespace Kereste.CORE.Models
{
    public class CategoryModel
    {
        public CategoryDTO Category { get; set; }
        public List<NewsDTO> NewsList { get; set; }
        public List<NewsDTO> MostPopularNews { get; set; }
    }
}
