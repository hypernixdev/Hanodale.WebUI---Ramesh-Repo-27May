using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface ITrainingStaffService
    {
        #region  TrainingStaff

        TrainingStaffDetails GetTrainingStaff(int currentUserId, int userId, int startIndex, int pageSize, string search);

        TrainingStaffDetails GetFileTrainingStaff(int currentUserId, int userId, int startIndex, int pageSize, string search,int FileId);

        TrainingStaffs SaveTrainingStaff(int currentUserId, List<TrainingStaffs> entity, FileUploadHistorys objHistory, string pageName);
        
        TrainingStaffs SaveTrainingStaff(int currentUserId, TrainingStaffs entity, string pageName);

        bool DeleteTrainingStaff(int currentUserId, int id, string pageName);

        TrainingStaffs GetTrainingStaffById(int id);

        List<TrainingStaffs> GetListTrainingStaff();

        bool IsTrainingStaffExists(TrainingStaffs entity); 

        #endregion
    }
}
