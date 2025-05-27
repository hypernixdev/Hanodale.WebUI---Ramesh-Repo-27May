using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanodale.Entity.Core;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Xml;
using System.ServiceModel;
using System.Data.Objects.SqlClient;
using System.Collections;
using System.Globalization;
using Hanodale.Domain.DTOs;

namespace Hanodale.DataAccessLayer.Interfaces
{
    public interface INewsService
    {
        #region News

        NewsDetails GetNewsBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search, object filterModel);

        NewsDetails GetNews(int currentUserId, bool all, int userId, int startIndex, int pageSize);

        Newss CreateNews(int currentUserId, Newss entity, string pageName);

        Newss UpdateNews(int currentUserId, Newss entity, string pageName);

        bool DeleteNews(int currentUserId, int id, string pageName);

        Newss GetNewsById(int id);

        List<Newss> GetListNews();

        bool IsNewsExists(Newss entity);

        #endregion
    }
}
