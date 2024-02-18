using Kereste.BLL.DTO;

namespace Kereste.CORE.Models
{
    public class ContentDetailModel
    {
        public NewsDTO News { get; set; }
        public List<NewsDTO> EditorNews { get; set; }
        public List<NewsDTO> InterestedNews { get; set; }
        public List<NewsDTO> MostNews { get; set; }
    }
}
