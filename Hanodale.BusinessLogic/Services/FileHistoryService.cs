using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.Domain.DTOs;
namespace Hanodale.BusinessLogic
{
   public class FileHistoryService : IFileHistoryService
    {
       public Hanodale.DataAccessLayer.IFileHistoryService DataProvider;

       public FileHistoryService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.FileHistoryService();
        }

       public FileUploadHistoryDetails GetFileHistory(int currentUserId, int userId, int startIndex, int pageSize, string search, bool Istraining)
        {
            if (string.IsNullOrEmpty(search))
                return this.DataProvider.GetFileHistory(currentUserId, userId, startIndex, pageSize,Istraining);
            else
                return this.DataProvider.GetFileHistoryBySearch(currentUserId, userId, startIndex, pageSize, search,Istraining);
        }

        public FileUploadHistorys SaveFileHistory(int currentUserId, FileUploadHistorys entity, string pageName)
        {
            if (entity.id > 0)
                return this.DataProvider.UpdateFileHistory(currentUserId, entity, pageName);
            else
                return this.DataProvider.CreateFileHistory(currentUserId, entity, pageName);
        }

        public bool DeleteFileHistory(int currentUserId, int id, string pageName)
        {
            return this.DataProvider.DeleteFileHistory(currentUserId, id, pageName);
        }

        public FileUploadHistorys GetFileHistoryById(int id)
        {
            return this.DataProvider.GetFileHistoryById(id);
        }

        public List<FileUploadHistorys> GetListFileHistory()
        {
            return this.DataProvider.GetListFileHistory();
        }

        public bool IsFileHistoryExists(FileUploadHistorys entity)
        {
            return this.DataProvider.IsFileHistoryExists(entity);
        }
    }
}
