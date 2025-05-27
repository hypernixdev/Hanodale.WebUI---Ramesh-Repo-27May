using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface INewsService
    {
        #region Newss

        NewsDetails GetNews(int currentUserId, bool all, int userId, int startIndex, int pageSize, string search, object filterModel);

        Newss SaveNews(int currentUserId, Newss entity, string pageName);

        bool DeleteNews(int currentUserId, int id, string pageName);

        Newss GetNewsById(int id);

        List<Newss> GetListNews();

        bool IsNewsExists(Newss entity);

        #endregion
    }
}
