using Azure;
using Kereste.BLL.DTO;
using Kereste.BLL.Services.Abstract;
using Kereste.DATA.Contexts;
using Kereste.DATA.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Kereste.BLL.Services.Concrete
{
    public class ContentService : IContentService
    {
        public KeresteDBContext _context;
        private readonly IConfiguration _config;

        public ContentService(KeresteDBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public bool AddContent(News model)
        {
           if (model != null)
            {
                _context.News.Add(model);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public List<NewsDTO> GetNews(int userID, int count, int page)
        {
            List<NewsDTO> getList = (from news in _context.News.Where(t => t.PublishDate <= DateTime.Now && t.User.ID == userID)
                                     orderby news.ID descending
                                     select new NewsDTO
                                     {
                                        NewsID = news.ID,
                                        Title = news.Title,
                                        AlternativeTitle = news.AlternativeTitle,
                                        Spot = news.Spot,
                                        Body = news.Body,
                                        Status = news.Status,
                                        Category = news.Category,
                                        PublishDate = news.PublishDate,
                                        Tags = news.Tags,
                                        User = news.User,
                                        UpdatedDate = news.UpdatedDate,
                                        HeadImage = _config["ImagePath"] + news.HeadImage,
                                        DetailImage = _config["ImagePath"] + news.VerticalImage,
                                     }).Skip(page).Take(count).ToList();
            return getList;
        }

        public NewsDTO GetNewsByID(int id)
        {
            NewsDTO getNews = (from news in _context.News.Where(t => t.ID == id)
                                     select new NewsDTO
                                     {
                                         NewsID = news.ID,
                                         Title = news.Title,
                                         AlternativeTitle = news.AlternativeTitle,
                                         Spot = news.Spot,
                                         Body = news.Body,
                                         Status = news.Status,
                                         Category = news.Category,
                                         PublishDate = news.PublishDate,
                                         Tags = news.Tags,
                                         User = news.User,
                                         UpdatedDate = news.UpdatedDate,
                                         HeadImage = _config["ImagePath"] + news.HeadImage,
                                         DetailImage = _config["ImagePath"] + news.VerticalImage,
                                     }).FirstOrDefault();
            return getNews;
        }

        public bool UpdateContent(News model)
        {
            if (model != null)
            {
                var news = _context.News.FirstOrDefault(x => x.ID == model.ID);
                if (news != null)
                {
                    news.Spot = model.Spot;
                    news.SelfLink = model.SelfLink;
                    news.Title = model.Title;
                    news.AlternativeTitle = model.AlternativeTitle;
                    news.Tags = model.Tags;
                    news.User = model.User;
                    news.UpdatedDate = DateTime.Now;
                    news.HeadImage = model.HeadImage;
                    news.VerticalImage = model.VerticalImage;
                    news.Tags = model.Tags;
                    news.Status = model.Status;
                    news.Category = model.Category;
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public int GetNewsCount()
        {
            return _context.News.Where(x => x.Status == 1).Count();
        }

        public List<NewsDTO> GetNews(int count)
        {
            List<NewsDTO> getList = (from news in _context.News.Where(t => t.PublishDate <= DateTime.Now)
                                     orderby news.ID descending
                                     select new NewsDTO
                                     {
                                         NewsID = news.ID,
                                         Title = news.Title,
                                         AlternativeTitle = news.AlternativeTitle,
                                         Spot = news.Spot,
                                         Body = news.Body,
                                         Status = news.Status,
                                         Category = news.Category,
                                         PublishDate = news.PublishDate,
                                         Tags = news.Tags,
                                         User = news.User,
                                         UpdatedDate = news.UpdatedDate,
                                         HeadImage = _config["ImagePath"] + news.HeadImage,
                                         DetailImage = _config["ImagePath"] + news.VerticalImage,
                                     }).Take(count).ToList();
            return getList;
        }
    }
}
