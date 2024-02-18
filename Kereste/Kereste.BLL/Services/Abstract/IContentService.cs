using Kereste.BLL.DTO;
using Kereste.DATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kereste.BLL.Services.Abstract
{
    public interface IContentService
    {
        bool AddContent(News model);
        bool UpdateContent(News model);
        bool UpdateContentRating(int newsID);
        int GetNewsCount();

        List<NewsDTO> GetNews(int userID, int count, int page);
        List<NewsDTO> GetNews(int count);
        List<NewsDTO> GetNewsByUser(int userID,int skipNewsID,int count);
        List<NewsDTO> GetInterestedNews(int skipNewsID, int count);

        List<NewsDTO> GetNewsByCategoryId(int categoryID, int count);

        List<NewsDTO> GetNewsByHitCount(int skipNewsID, int count);
        NewsDTO GetNewsByID(int id);
    }
}
