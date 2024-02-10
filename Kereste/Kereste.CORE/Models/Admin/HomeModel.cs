using Kereste.BLL.DTO;

namespace Kereste.CORE.Models.Admin
{
    public class HomeModel
    {
        public int NewsCount{ get; set; }
        public int CategoryCount { get; set; }
        public int UserCount { get; set; }
        public List<NewsDTO> LastNewsList { get; set; }
        public List<CategoryDTO> LastCategoryList { get; set; }
        public List<UserDTO> LastUserList { get; set; }
    }
}
