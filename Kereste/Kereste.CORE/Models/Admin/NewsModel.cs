using Kereste.BLL.DTO;
using Kereste.DATA.Entities;

namespace Kereste.CORE.Models.Admin
{
    public class NewsModel
    {
        public List<CategoryDTO> CategoryList { get; set; }
        public List<NewsDTO> NewsList { get; set; }
        public NewsDTO News { get; set; }
    }
}
