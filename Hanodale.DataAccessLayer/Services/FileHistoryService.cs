//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Hanodale.Domain.DTOs;
//using Hanodale.Entity.Core;
//using System.ServiceModel;
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
using Hanodale.DataAccessLayer.Interfaces;
using Hanodale.Domain;
namespace Hanodale.DataAccessLayer.Services
{
    public class FileHistoryService : IFileHistoryService
    {

        public FileUploadHistoryDetails GetFileHistoryBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search,bool Istraining)
        {
            FileUploadHistoryDetails _result = new FileUploadHistoryDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record

                    if (Istraining)
                    {
                        _result.recordDetails.totalRecords = model.FileUploadHistories.Where(ff => ff.uploadType == model.ModuleItems.Where(mm => mm.name == "Training").FirstOrDefault().id).Count();
                        _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;
                        var result = model.FileUploadHistories
                               .OrderByDescending(a => a.modifiedDate)
                               .Where(a => (a.fileName.Contains(search)
                                    ||  SqlFunctions.StringConvert((double)a.totalRecords).Contains(search)
                                    || a.createdBy.Contains(search)
                                    ||((a.createdDate.Day < 10 ? "0" : "") + SqlFunctions.StringConvert((double)a.createdDate.Day).Trim() + "/" + (a.createdDate.Month < 10 ? "0" : "") + (SqlFunctions.StringConvert((double)a.createdDate.Month)).Trim() + "/" + (SqlFunctions.StringConvert((double)a.createdDate.Year)).Trim()).Contains(search))
                                    && a.uploadType == model.ModuleItems.Where(md => md.name == "Training").FirstOrDefault().id)
                                    .Select(p => new FileUploadHistorys
                                    {
                                        id = p.id,
                                        fileName = p.fileName,
                                        totalRecords = p.totalRecords,
                                        uploadType = p.uploadType,
                                        user_Id = p.user_Id,
                                        userName = p.createdBy,
                                        createdBy = p.createdBy,
                                        createdDate = p.createdDate,
                                        modifiedBy = p.modifiedBy,
                                        modifiedDate = p.modifiedDate
                                    }).ToList();

                        //Get filter data
                        _result.recordDetails.totalDisplayRecords = result.Count;
                        _result.lstFileUploadHistory = result.Skip(startIndex).Take(pageSize).ToList();
                    }
                    else
                    {
                        _result.recordDetails.totalRecords = model.FileUploadHistories.Where(ff => ff.uploadType == model.ModuleItems.Where(mm => mm.name == "Attendance").FirstOrDefault().id).Count();
                        _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;
                        var result = model.FileUploadHistories
                               .OrderByDescending(a => a.modifiedDate)
                               .Where(a => (a.fileName.Contains(search)
                                    || SqlFunctions.StringConvert((double)a.totalRecords).Contains(search)
                                    || a.createdBy.Contains(search)
                                    || ((a.createdDate.Day < 10 ? "0" : "") + SqlFunctions.StringConvert((double)a.createdDate.Day).Trim() + "/" + (a.createdDate.Month < 10 ? "0" : "") + (SqlFunctions.StringConvert((double)a.createdDate.Month)).Trim() + "/" + (SqlFunctions.StringConvert((double)a.createdDate.Year)).Trim()).Contains(search)) && a.uploadType == model.ModuleItems.Where(md => md.name == "Attendance").FirstOrDefault().id)
                                    .Select(p => new FileUploadHistorys
                                    {
                                        id = p.id,
                                        fileName = p.fileName,
                                        totalRecords = p.totalRecords,
                                        uploadType = p.uploadType,
                                        user_Id = p.user_Id,
                                        userName = p.createdBy,
                                        createdBy = p.createdBy,
                                        createdDate = p.createdDate,
                                        modifiedBy = p.modifiedBy,
                                        modifiedDate = p.modifiedDate
                                    }).ToList();

                        //Get filter data
                        _result.recordDetails.totalDisplayRecords = result.Count;
                        _result.lstFileUploadHistory = result.Skip(startIndex).Take(pageSize).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public FileUploadHistoryDetails GetFileHistory(int currentUserId, int userId, int startIndex, int pageSize,bool Istraining)
        {
            FileUploadHistoryDetails _result = new FileUploadHistoryDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record
                    if (Istraining)
                    {
                        _result.recordDetails.totalRecords = model.FileUploadHistories.Where(ff => ff.uploadType == model.ModuleItems.Where(mm => mm.name == "Training").FirstOrDefault().id).Count();
                        _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;
                        _result.lstFileUploadHistory = model.FileUploadHistories
                                    .Where(ff => ff.uploadType == model.ModuleItems.Where(mm => mm.name == "Training").FirstOrDefault().id)
                                    .OrderByDescending(a => a.modifiedDate)
                                    .Skip(startIndex).Take(pageSize)
                                    .Select(p => new FileUploadHistorys
                                    {
                                        id = p.id,
                                        fileName = p.fileName,
                                        totalRecords = p.totalRecords,
                                        uploadType = p.uploadType,
                                        user_Id = p.user_Id,
                                        userName = p.createdBy,
                                        createdBy = p.createdBy,
                                        createdDate = p.createdDate,
                                        modifiedBy = p.modifiedBy,
                                        modifiedDate = p.modifiedDate
                                    }).ToList();
                    }
                    else
                    {
                        _result.recordDetails.totalRecords = model.FileUploadHistories.Where(ff => ff.uploadType == model.ModuleItems.Where(mm => mm.name == "Attendance").FirstOrDefault().id).Count();
                        _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;
                        _result.lstFileUploadHistory = model.FileUploadHistories
                                    .Where(ff => ff.uploadType == model.ModuleItems.Where(mm => mm.name == "Attendance").FirstOrDefault().id)
                                    .OrderByDescending(a => a.modifiedDate)
                                    .Skip(startIndex).Take(pageSize)
                                    .Select(p => new FileUploadHistorys
                                    {
                                        id = p.id,
                                        fileName = p.fileName,
                                        totalRecords = p.totalRecords,
                                        uploadType = p.uploadType,
                                        user_Id = p.user_Id,
                                        userName = p.createdBy,
                                        createdBy = p.createdBy,
                                        createdDate = p.createdDate,
                                        modifiedBy = p.modifiedBy,
                                        modifiedDate = p.modifiedDate
                                    }).ToList();
                    }
                  
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public FileUploadHistorys CreateFileHistory(int currentUserId, FileUploadHistorys entity, string pageName)
        {
            throw new NotImplementedException();
        }

        public FileUploadHistorys UpdateFileHistory(int currentUserId, FileUploadHistorys entity, string pageName)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFileHistory(int currentUserId, int id, string pageName)
        {
            bool IsSuccess = false;
            using (HanodaleEntities model=new HanodaleEntities())
            {
                var fileHistory = model.FileUploadHistories.Where(ff => ff.id == id && ff.user_Id==currentUserId).FirstOrDefault();

                var Uploadtype = model.ModuleItems.Where(mm => mm.id == fileHistory.uploadType).FirstOrDefault().name;

                if (fileHistory!=null)
                {
                    if (Uploadtype == "Training")
                    {
                        model.FileUploadHistories.Remove(fileHistory);
                        model.SaveChanges();
                        IsSuccess = true;
                    }
                    else
                    {
                        model.FileUploadHistories.Remove(fileHistory);
                        model.SaveChanges();
                        IsSuccess = true;
                    }
                    
                }
            }
            return IsSuccess;
        }

        public FileUploadHistorys GetFileHistoryById(int id)
        {
            FileUploadHistorys entity = new FileUploadHistorys();
            using (HanodaleEntities model = new HanodaleEntities())
            {
                var fileHistory = model.FileUploadHistories.Where(ff => ff.id == id).FirstOrDefault();

                if (fileHistory != null)
                {
                    entity.createdBy = fileHistory.createdBy;
                    entity.fileName = fileHistory.fileName;
                    entity.id = fileHistory.id;
                    entity.createdDate = fileHistory.createdDate;
                    entity.totalRecords = fileHistory.totalRecords;
                    entity.uploadType = fileHistory.uploadType;
                }
            }
            return entity;
        }

        public List<FileUploadHistorys> GetListFileHistory()
        {
            throw new NotImplementedException();
        }

        public bool IsFileHistoryExists(FileUploadHistorys entity)
        {
            throw new NotImplementedException();
        }
    }
}
