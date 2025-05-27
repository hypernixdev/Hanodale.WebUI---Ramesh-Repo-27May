using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class NewsService : INewsService
    {
        #region StandardJob

        public Hanodale.DataAccessLayer.Interfaces.INewsService DataProvider;

        public NewsService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.NewsService();
        }

        public NewsDetails GetNews(int currentUserId, bool all, int userId, int startIndex, int pageSize, string search, object filterModel)
        {
            if (string.IsNullOrEmpty(search) && filterModel==null)
                return this.DataProvider.GetNews(currentUserId, all, userId, startIndex, pageSize);
            else
                return this.DataProvider.GetNewsBySearch(currentUserId, userId, startIndex, pageSize, search, filterModel);
        }

        public Newss SaveNews(int currentUserId, Newss entity, string pageName)
        {
            if (entity.id > 0)
                return this.DataProvider.UpdateNews(currentUserId, entity, pageName);
            else
                return this.DataProvider.CreateNews(currentUserId, entity, pageName);
        }

        public bool DeleteNews(int currentUserId, int id, string pageName)
        {
            return this.DataProvider.DeleteNews(currentUserId, id, pageName);
        }

        public Newss GetNewsById(int id)
        {
            return this.DataProvider.GetNewsById(id);
        }

        public bool IsNewsExists(Newss entity)
        {
            return this.DataProvider.IsNewsExists(entity);
        }

        public List<Newss> GetListNews()
        {
            return this.DataProvider.GetListNews();
        }

        #endregion
    }
}
