using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.DataAccessLayer
{
   public interface IFileHistoryService
    {
        #region File History

        FileUploadHistoryDetails GetFileHistoryBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search,bool Istraining);

        FileUploadHistoryDetails GetFileHistory(int currentUserId, int userId, int startIndex, int pageSize,bool Istraining);

        FileUploadHistorys CreateFileHistory(int currentUserId, FileUploadHistorys entity, string pageName);

        FileUploadHistorys UpdateFileHistory(int currentUserId, FileUploadHistorys entity, string pageName);

        bool DeleteFileHistory(int currentUserId, int id, string pageName);

        FileUploadHistorys GetFileHistoryById(int id);

        List<FileUploadHistorys> GetListFileHistory();

        bool IsFileHistoryExists(FileUploadHistorys entity);

        #endregion
    }
}
