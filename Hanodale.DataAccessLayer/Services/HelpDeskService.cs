using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.Domain.DTOs;
using Hanodale.DataAccessLayer.Interfaces;
using Hanodale.Entity.Core;
using System.ServiceModel;
namespace Hanodale.DataAccessLayer.Services
{
    public class HelpDeskService : IHelpDeskService
    {
        public HelpDeskDetails GetHelpDeskBySearch(int currentUserId, int organizationId, int startIndex, int pageSize, string search, object filterEntity)
        {

            HelpDeskDetails _result = new HelpDeskDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                var filter = new HelpDesks();
                if (filterEntity != null)
                    filter = (HelpDesks)filterEntity;

                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _moduleId = string.IsNullOrEmpty(filter.feedbackModule) ? 0 : Convert.ToInt32(filter.feedbackModule);
                    var _module = string.IsNullOrEmpty(filter.feedbackModule) ? "" : model.ModuleItems.FirstOrDefault(a => a.id == _moduleId).name;

                    model.Configuration.ProxyCreationEnabled = false;
                    model.Configuration.LazyLoadingEnabled = false;
                    model.Configuration.AutoDetectChangesEnabled = false;
                    //get total record
                    _result.recordDetails.totalRecords = model.HelpDesks.AsNoTracking().Include("ModuleItem").Join(model.Users, f => f.name, s => s.firstName, (f, s) => new { hd = f, usr = s }).Count(p => p.hd.organization_Id == organizationId
                        && (filter.createdDateFrom == null ? true : p.hd.createdDate >= filter.createdDateFrom)
                        && (filter.createdDateTo == null ? true : p.hd.createdDate < filter.createdDateTo)
                        && (filter.workFollowStatus_Id != 0 ? p.hd.workFollowStatus_Id == filter.workFollowStatus_Id : true)
                        && (!string.IsNullOrEmpty(filter.feedbackModule) ? p.hd.feedbackModule.Contains(_module) : true)
                         && (string.IsNullOrEmpty(search) ? true :
                                (p.hd.code.Contains(search)
                            || p.hd.ModuleItem.name.Contains(search)
                            || p.hd.name.Contains(search)
                            || p.hd.receivedBy.Contains(search)
                            || p.usr.firstName.Contains(search)
                            || p.hd.feedback.Contains(search))));
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    //Filtered count
                    var result = model.HelpDesks.AsNoTracking().Include("ModuleItem").Join(model.Users, f => f.name, s => s.firstName, (f, s) => new { hd = f, usr = s }).OrderByDescending(p => (p.hd.modifiedDate != null ? p.hd.modifiedDate : p.hd.createdDate)).Where(p => p.hd.organization_Id == organizationId
                        && (filter.createdDateFrom == null ? true : p.hd.createdDate >= filter.createdDateFrom)
                        && (filter.createdDateTo == null ? true : p.hd.createdDate < filter.createdDateTo)
                        && (filter.workFollowStatus_Id != 0 ? p.hd.workFollowStatus_Id == filter.workFollowStatus_Id : true)
                        && (!string.IsNullOrEmpty(filter.feedbackModule) ? p.hd.feedbackModule.Contains(_module) : true)
                         && (string.IsNullOrEmpty(search) ? true :
                                (p.hd.code.Contains(search)
                            || p.hd.ModuleItem.name.Contains(search)
                            || p.hd.name.Contains(search)
                            || p.hd.receivedBy.Contains(search)
                            || p.usr.firstName.Contains(search)
                            || p.hd.feedback.Contains(search))))
                        .Skip(startIndex).Take(pageSize).ToList().Select(p => new HelpDesks
                        {
                            cellPhone = p.hd.cellPhone,
                            createdBy = p.hd.createdBy,
                            createdDate = p.hd.createdDate,
                            department = p.hd.department,
                            designation = p.hd.designation,
                            code = p.hd.code,
                            email = p.hd.email,
                            feedback = p.hd.feedback,
                            id = p.hd.id,
                            modifiedBy = p.hd.modifiedBy,
                            modifiedDate = p.hd.modifiedDate,
                            name = p.usr != null ? p.usr.firstName : p.hd.name,
                            officePhone = p.hd.officePhone,
                            organization_Id = p.hd.organization_Id,
                            user_Id = p.hd.user_Id,
                            workFollowStatus_Id = p.hd.workFollowStatus_Id,
                            workFollowStatusName = p.hd.ModuleItem != null ? p.hd.ModuleItem.name : string.Empty,
                            receivedBy = model.Users.Count(x => x.userName == p.hd.receivedBy) > 0 ? model.Users.FirstOrDefault(x => x.userName == p.hd.receivedBy).firstName : p.hd.receivedBy,
                            receivedDate = p.hd.receivedDate,
                            actionDate = p.hd.actionDate,
                            inprogressDate = p.hd.inprogressDate,
                        })
                        .ToList();



                    _result.lstHelpDesk = result;


                    foreach (var item in _result.lstHelpDesk)
                    {
                        if (item.workFollowStatusName.ToUpper() == "NEW")
                        {
                            item.newFlag = (item.receivedDate == null && item.inprogressDate == null && item.actionDate == null) ? ((((DateTime.Now - item.createdDate).TotalMinutes) > 30) ? true : false) : false;
                        }
                        else if (item.workFollowStatusName.ToUpper() == "RECEIVED")
                        {
                            item.receiveFlag = (item.receivedDate != null && item.inprogressDate == null && item.actionDate == null) ? (((DateTime.Now - (item.receivedDate ?? DateTime.MinValue)).TotalMinutes > 30) ? true : false) : false;
                        }
                        else if (item.workFollowStatusName.ToUpper() == "IN PROGRESS")
                        {
                            item.actionFlag = ((item.receivedDate != null && item.inprogressDate != null) && item.actionDate == null) ? (((DateTime.Now - (item.inprogressDate ?? DateTime.MinValue)).TotalHours > 3) ? true : false) : false;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public HelpDeskDetails GetHelpDesk(int currentUserId, int organizationId, int startIndex, int pageSize, string search, object filterEntity)
        {
            HelpDeskDetails _result = new HelpDeskDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                var filter = new HelpDesks();
                if (filterEntity != null)
                    filter = (HelpDesks)filterEntity;

                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _moduleId = string.IsNullOrEmpty(filter.feedbackModule) ? 0 : Convert.ToInt32(filter.feedbackModule);
                    var _module = string.IsNullOrEmpty(filter.feedbackModule) ? "" : model.ModuleItems.FirstOrDefault(a => a.id == _moduleId).name;
                    model.Configuration.ProxyCreationEnabled = false;
                    model.Configuration.LazyLoadingEnabled = false;
                    model.Configuration.AutoDetectChangesEnabled = false;

                    //get total record
                    _result.recordDetails.totalRecords = model.HelpDesks.AsNoTracking().Include("ModuleItem").Join(model.Users, f => f.name, s => s.firstName, (f, s) => new { hd = f, usr = s }).Count(p => p.hd.organization_Id == organizationId
                        && (filter.createdDateFrom == null ? true : p.hd.createdDate >= filter.createdDateFrom)
                        && (filter.createdDateTo == null ? true : p.hd.createdDate < filter.createdDateTo)
                        && (filter.workFollowStatus_Id != 0 ? p.hd.workFollowStatus_Id == filter.workFollowStatus_Id : true)
                        && (!string.IsNullOrEmpty(filter.feedbackModule) ? p.hd.feedbackModule.Contains(_module) : true)
                         && (string.IsNullOrEmpty(search) ? true :
                                (p.hd.code.Contains(search)
                            || p.hd.ModuleItem.name.Contains(search)
                            || p.hd.name.Contains(search)
                            || p.hd.receivedBy.Contains(search)
                            || p.usr.firstName.Contains(search)
                            || p.hd.feedback.Contains(search))));
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    //Filtered count
                    var result = model.HelpDesks.AsNoTracking().Include("ModuleItem").Join(model.Users, f => f.name, s => s.firstName, (f, s) => new { hd = f, usr = s }).OrderByDescending(p => (p.hd.modifiedDate != null ? p.hd.modifiedDate : p.hd.createdDate)).Where(p => p.hd.organization_Id == organizationId
                        && (filter.createdDateFrom == null ? true : p.hd.createdDate >= filter.createdDateFrom)
                        && (filter.createdDateTo == null ? true : p.hd.createdDate < filter.createdDateTo)
                        && (filter.workFollowStatus_Id != 0 ? p.hd.workFollowStatus_Id == filter.workFollowStatus_Id : true)
                        && (!string.IsNullOrEmpty(filter.feedbackModule) ? p.hd.feedbackModule.Contains(_module) : true)
                         && (string.IsNullOrEmpty(search) ? true :
                                (p.hd.code.Contains(search)
                            || p.hd.ModuleItem.name.Contains(search)
                            || p.hd.name.Contains(search)
                            || p.hd.receivedBy.Contains(search)
                            || p.usr.firstName.Contains(search)
                            || p.hd.feedback.Contains(search))))
                        .Skip(startIndex).Take(pageSize).ToList().Select(p => new HelpDesks
                        {
                            cellPhone = p.hd.cellPhone,
                            createdBy = p.hd.createdBy,
                            createdDate = p.hd.createdDate,
                            department = p.hd.department,
                            designation = p.hd.designation,
                            code = p.hd.code,
                            email = p.hd.email,
                            feedback = p.hd.feedback,
                            id = p.hd.id,
                            modifiedBy = p.hd.modifiedBy,
                            modifiedDate = p.hd.modifiedDate,
                            name = p.usr!=null ? p.usr.firstName : p.hd.name,
                            officePhone = p.hd.officePhone,
                            organization_Id = p.hd.organization_Id,
                            user_Id = p.hd.user_Id,
                            workFollowStatus_Id = p.hd.workFollowStatus_Id,
                            workFollowStatusName =p.hd.ModuleItem!=null? p.hd.ModuleItem.name: string.Empty,
                            receivedBy = model.Users.Count(x => x.userName == p.hd.receivedBy) > 0 ? model.Users.FirstOrDefault(x => x.userName == p.hd.receivedBy).firstName : p.hd.receivedBy,
                            receivedDate = p.hd.receivedDate,
                            actionDate = p.hd.actionDate,
                            inprogressDate = p.hd.inprogressDate,
                        })
                        .ToList();

                    _result.lstHelpDesk = result;

                    foreach (var item in _result.lstHelpDesk)
                    {
                        if (item.workFollowStatusName.ToUpper() == "NEW")
                        {
                            item.newFlag = (item.receivedDate == null && item.inprogressDate == null && item.actionDate == null) ? ((((DateTime.Now - item.createdDate).TotalMinutes) > 30) ? true : false) : false;
                        }
                        else if (item.workFollowStatusName.ToUpper() == "RECEIVED")
                        {
                            item.receiveFlag = (item.receivedDate != null && item.inprogressDate == null && item.actionDate == null) ? (((DateTime.Now - (item.receivedDate ?? DateTime.MinValue)).TotalMinutes > 30) ? true : false) : false;
                        }
                        else if (item.workFollowStatusName.ToUpper() == "IN PROGRESS")
                        {
                            item.actionFlag = ((item.receivedDate != null && item.inprogressDate != null) && item.actionDate == null) ? (((DateTime.Now - (item.inprogressDate ?? DateTime.MinValue)).TotalHours > 3) ? true : false) : false;
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public HelpDesks CreateHelpDesk(int currentUserId, HelpDesks helpDeskEn, string pageName)
        {
            HelpDesk _helpDeskEn = new HelpDesk();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Add new stock
                    _helpDeskEn.code = helpDeskEn.code;
                    _helpDeskEn.cellPhone = helpDeskEn.cellPhone;
                    _helpDeskEn.createdBy = helpDeskEn.createdBy;
                    _helpDeskEn.createdDate = helpDeskEn.createdDate;
                    _helpDeskEn.department = helpDeskEn.department;
                    _helpDeskEn.designation = helpDeskEn.designation;
                    _helpDeskEn.email = helpDeskEn.email;
                    _helpDeskEn.feedback = helpDeskEn.feedback;
                    _helpDeskEn.modifiedBy = helpDeskEn.modifiedBy;
                    _helpDeskEn.modifiedDate = helpDeskEn.modifiedDate;
                    _helpDeskEn.name = helpDeskEn.name;
                    _helpDeskEn.officePhone = helpDeskEn.officePhone;
                    _helpDeskEn.organization_Id = helpDeskEn.organization_Id;
                    _helpDeskEn.user_Id = helpDeskEn.user_Id;
                    _helpDeskEn.workFollowStatus_Id = helpDeskEn.workFollowStatus_Id;

                    string _prefix = helpDeskEn.prefix;
                    var _org = model.Organizations.FirstOrDefault(a => a.id == _helpDeskEn.organization_Id);
                    if (_org != null)
                    {
                        _prefix += _org.prefix;
                    }

                    var _runningObj = model.RunningCodes.Where(a => a.moduleName == "HELPDESK" &&
                         a.organization_Id == _helpDeskEn.organization_Id).FirstOrDefault();

                    if (_runningObj == null)
                    {
                        int _totalRecord = model.HelpDesks.Count(a => a.organization_Id == _helpDeskEn.organization_Id);
                        RunningCode _codeObj = new RunningCode();
                        _codeObj.organization_Id = _helpDeskEn.organization_Id;
                        _codeObj.prefix = _prefix;
                        _codeObj.moduleName = "HELPDESK";
                        _codeObj.runningNo = _totalRecord > 0 ? _totalRecord + 1 : 1;
                        model.RunningCodes.Add(_codeObj);
                        _helpDeskEn.code = _prefix + _codeObj.runningNo.ToString();
                    }
                    else
                    {
                        _runningObj.runningNo = _runningObj.runningNo + 1;
                        _helpDeskEn.code = _prefix + _runningObj.runningNo.ToString();
                    }

                    model.HelpDesks.Add(_helpDeskEn);


                    model.SaveChanges();

                    HelpDeskLog _log = new HelpDeskLog();
                    _log.helpDesk_Id = _helpDeskEn.id;
                    _log.auditLog = "Status is [" + model.ModuleItems.FirstOrDefault(a => a.id == helpDeskEn.workFollowStatus_Id).name + "]";
                    _log.createdBy = helpDeskEn.modifiedBy;
                    _log.createdDate = DateTime.Now;
                    _log.remarks = helpDeskEn.remarks;
                    _log.fileNames = string.Join(",", helpDeskEn.fileNames.Select(p => p));
                    model.HelpDeskLogs.Add(_log);

                    model.SaveChanges();
                    helpDeskEn.isPass = true;
                    helpDeskEn.id = _helpDeskEn.id;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return helpDeskEn;
        }

        public HelpDesks UpdateHelpDesk(int currentUserId, HelpDesks helpDeskEn, string pageName)
        {
            HelpDesk _helpDeskEn = new HelpDesk();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update stock
                    _helpDeskEn = model.HelpDesks.SingleOrDefault(p => p.id == helpDeskEn.id);
                    if (_helpDeskEn != null)
                    {
                        var lst = new List<string>();
                        if (helpDeskEn.isReceived || helpDeskEn.isCreate)
                        {
                            var obj = model.HelpDeskLogs.OrderByDescending(p => p.id).FirstOrDefault(p => p.helpDesk_Id == helpDeskEn.id);
                            if (obj != null)
                            {
                                if (obj.fileNames != null)
                                {
                                    if (!string.IsNullOrEmpty(obj.fileNames))
                                    {
                                        lst = obj.fileNames.Split(',').ToList();
                                        if (helpDeskEn.isCreate)
                                        {
                                            helpDeskEn.removeFileName = lst;
                                        }
                                    }
                                }

                                if (helpDeskEn.isCreate)
                                {
                                    if (helpDeskEn.fileNames != null)
                                    {
                                        obj.fileNames = string.Join(",", helpDeskEn.fileNames.Select(p => p));
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (helpDeskEn.fileNames != null)
                            {
                                lst = helpDeskEn.fileNames;
                            }
                        }

                        _helpDeskEn.modifiedBy = helpDeskEn.modifiedBy;
                        _helpDeskEn.workFollowStatus_Id = helpDeskEn.workFollowStatus_Id;
                        _helpDeskEn.remarks = helpDeskEn.remarks ?? "";
                        _helpDeskEn.modifiedDate = DateTime.Now;

                        if (_helpDeskEn.workFollowStatus_Id == 6550)
                        {
                            _helpDeskEn.receivedBy = helpDeskEn.modifiedBy;
                            _helpDeskEn.receivedDate = DateTime.Now;

                        }

                        if (helpDeskEn.isReceived || !helpDeskEn.isCreate)
                        {
                            HelpDeskLog _log = new HelpDeskLog();
                            _log.helpDesk_Id = helpDeskEn.id;
                            if (helpDeskEn.isReceived)
                                _log.auditLog = "Status changed to [" + model.ModuleItems.FirstOrDefault(a => a.id == helpDeskEn.workFollowStatus_Id).name + "]";
                            else
                                _log.auditLog = "Record updated";
                            _log.createdBy = helpDeskEn.modifiedBy;
                            _log.createdDate = DateTime.Now;
                            _log.remarks = helpDeskEn.remarks;
                            _log.fileNames = string.Empty;
                            if (lst != null)
                            {
                                if (lst.Count > 0)
                                {
                                    _log.fileNames = string.Join(",", lst.Select(p => p));
                                }
                            }
                            model.HelpDeskLogs.Add(_log);
                        }
                        
                        model.SaveChanges();

                        helpDeskEn.isPass = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return helpDeskEn;
        }

        public List<HelpDesks> UpdatedHelpDeskApproval(int currentUserId, List<HelpDesks> helpDeskEn, string pageName)
        {
            HelpDesk _helpDeskEn = new HelpDesk();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    foreach (var approve in helpDeskEn)
                    {
                        // update stock
                        _helpDeskEn = model.HelpDesks.SingleOrDefault(p => p.id == approve.id);
                        if (_helpDeskEn != null)
                        {
                            _helpDeskEn.cellPhone = approve.cellPhone;
                            _helpDeskEn.department = approve.department;
                            _helpDeskEn.designation = approve.designation;
                            _helpDeskEn.email = approve.email;
                            _helpDeskEn.feedback = approve.feedback;
                            _helpDeskEn.modifiedBy = approve.modifiedBy;
                            _helpDeskEn.modifiedDate = approve.modifiedDate;
                            _helpDeskEn.name = approve.name;
                            _helpDeskEn.officePhone = approve.officePhone;
                            _helpDeskEn.organization_Id = approve.organization_Id;
                            _helpDeskEn.user_Id = approve.user_Id;
                            _helpDeskEn.workFollowStatus_Id = approve.workFollowStatus_Id;
                        }
                        model.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return helpDeskEn;
        }

        public bool DeleteHelpDesk(int currentUserId, int helpDeskId, string pageName)
        {
            HelpDesk _helpDeskEn = new HelpDesk();
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update stock
                    _helpDeskEn = model.HelpDesks.SingleOrDefault(p => p.id == helpDeskId);
                    if (_helpDeskEn!=null)
                    {
                        model.HelpDesks.Remove(_helpDeskEn);
                        model.SaveChanges();
                        isDeleted = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return isDeleted;
        }

        public HelpDesks GetHelpDeskById(int id)
        {
            HelpDesks _helpDeskEn = new HelpDesks();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    var p = model.HelpDesks.SingleOrDefault(o => o.id == id);
                    if (p != null)
                    {
                        _helpDeskEn = new HelpDesks
                        {
                            cellPhone = p.cellPhone,
                            createdBy = p.createdBy,
                            createdDate = p.createdDate,
                            department = p.department,
                            designation = p.designation,
                            email = p.email,
                            feedback = p.feedback,
                            id = p.id,
                            code = p.code,
                            modifiedBy = p.modifiedBy,
                            modifiedDate = p.modifiedDate,
                            name = p.name,
                            officePhone = p.officePhone,
                            organization_Id = p.organization_Id,
                            user_Id = p.user_Id,
                            workFollowStatus_Id = p.workFollowStatus_Id,
                            remarks = p.remarks ?? "",
                            receivedBy = p.receivedBy ?? "",
                            receivedDate = p.receivedDate,
                        };

                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _helpDeskEn;
        }

        public bool IsHelpDeskExists(HelpDesks helpDesk)
        {
            HelpDesk _helpDeskEn = new HelpDesk();
            bool isExists = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    _helpDeskEn = model.HelpDesks.SingleOrDefault(p => p.id != helpDesk.id);
                    if (_helpDeskEn != null)
                        isExists = true;
                }
            }
            catch (Exception ex)
            {
                //we don't want to reveal any details to the client
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return isExists;
        }

        public List<HelpDesks> GetListHelpDesk()
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.HelpDesks.Select(p => new HelpDesks
                        {
                            cellPhone = p.cellPhone,
                            createdBy = p.createdBy,
                            createdDate = p.createdDate,
                            department = p.department,
                            designation = p.designation,
                            email = p.email,
                            feedback = p.feedback,
                            id = p.id,
                            modifiedBy = p.modifiedBy,
                            modifiedDate = p.modifiedDate,
                            name = p.name,
                            officePhone = p.officePhone,
                            organization_Id = p.organization_Id,
                            user_Id = p.user_Id,
                            workFollowStatus_Id = p.workFollowStatus_Id
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.Message);
            }
        }

        /// <summary>
        /// Get List of User
        /// </summary> 
        /// <returns></returns>
        public List<Users> GetListUserByStatus()
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get User List

                    return model.Users.Where(p => p.status == "Active")
                    .Select(p => new Users
                    {
                        id = p.id,
                        userName = p.userName,
                        status = p.status
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<HelpDesks> GetListHelpDeskByOrganizationAndApproved(int subCostCenter)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.HelpDesks.Where(p => p.organization_Id == subCostCenter && p.workFollowStatus_Id == 98).Select(p => new HelpDesks
                    {
                        code = p.code,
                        cellPhone = p.cellPhone,
                        createdBy = p.createdBy,
                        createdDate = p.createdDate,
                        department = p.department,
                        designation = p.designation,
                        email = p.email,
                        feedback = p.feedback,
                        id = p.id,
                        modifiedBy = p.modifiedBy,
                        modifiedDate = p.modifiedDate,
                        name = p.name,
                        officePhone = p.officePhone,
                        organization_Id = p.organization_Id,
                        user_Id = p.user_Id,
                        workFollowStatus_Id = p.workFollowStatus_Id
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.Message);
            }
        }

        private ChartInfo SetDefaultFilterDate(ChartInfo entity)
        {
            var obj = new ChartPanelInfo();
            if (entity.loadDateFrom == DateTime.MinValue || entity.loadDateTo == DateTime.MinValue)
            {
                var currentDate = DateTime.Now.Date;
                if (entity.loadDateFrom == DateTime.MinValue)
                    obj.loadDateFrom = new DateTime(currentDate.Year, currentDate.Month, 1); // firstOfTheMonth.AddMonths(-3).AddDays(1);
                else
                    obj.loadDateFrom = entity.loadDateFrom;

                if (entity.loadDateTo == DateTime.MinValue)
                    obj.loadDateTo = currentDate;
                else
                    obj.loadDateTo = entity.loadDateTo;

                return obj;
            }
            return entity;
        }

        public ChartPanelInfo GetChartPanelInfo(ChartPanelInfo entity)
        {
            try
            {
                var obj = SetDefaultFilterDate(entity);
                entity.loadDateFrom = obj.loadDateFrom;
                entity.loadDateTo = obj.loadDateTo;


                using (HanodaleEntities model = new HanodaleEntities())
                {
                    entity.chartItems = new List<ChartDashboard>();

                    foreach (var item in entity.typeList)
                    {

                        var result = model.DB_Ticket_Statistic(item, entity.loadDateFrom, entity.loadDateTo, entity.organizationId);
                        if (result != null)
                        {
                            var res = result.ToList().OrderBy(p => p.statisticCount).FirstOrDefault();
                            if (res != null)
                            {
                                entity.chartItems.Add(new ChartDashboard { type = item, value = res.statisticCount??0, valuePercentage = res.statisticPerc, backColor = res.backColor });
                            }
                        }
                    }
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }

        }

        public List<StackingBarChartPanelTicketInfo> GetStackingBarChartPanelTicketInfo(StackingBarChartPanelTicketInfo entity)
        {
            try
            {
                var _entity = new List<StackingBarChartPanelTicketInfo>();
                var obj = SetDefaultFilterDate(entity);
                entity.loadDateFrom = obj.loadDateFrom;
                entity.loadDateTo = obj.loadDateTo;

                using (HanodaleEntities model = new HanodaleEntities())
                {
                    foreach (var item in entity.typeList)
                    {
                        var newEntity = new StackingBarChartPanelTicketInfo();
                        newEntity.stackingBarChartPanelList = new List<StackingBarChartPanelTicketList>();
                        newEntity.filterType = item;

                        var result = model.DB_Ticket_Statistic_Bar(item, entity.loadDateFrom, entity.loadDateTo, entity.organizationId);
                        foreach (var lst in result.GroupBy(p => p.categoryName))
                        {
                            var subItem = new StackingBarChartPanelTicketList();
                            subItem.categoryName = lst.Key;
                            subItem.subItems = lst.Select(p => new StackingBarChartPanelTicketSubItem { Count = p.count??0, Category = p.categoryName, DataType = p.dataType }).ToList();
                            newEntity.stackingBarChartPanelList.Add(subItem);
                        }

                        _entity.Add(newEntity);
                    }
                }
                return _entity;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }

        }

        public ChartPanelTicketDetails GetChartPanelDetails(int currentUserId, int userId, int organization_Id, int startIndex, int pageSize, string search, object filterEntity)
        {
            var _result = new ChartPanelTicketDetails();
            try
            {
                _result.recordDetails = new RecordDetails();
                var filter = new ChartPanelInfo();
                if (filterEntity != null)
                    filter = (ChartPanelInfo)filterEntity;

                var obj = SetDefaultFilterDate(filter);
                filter.loadDateFrom = obj.loadDateFrom;
                filter.loadDateTo = obj.loadDateTo;

                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var totalRecordNo = new System.Data.Objects.ObjectParameter("totalRecordNo", typeof(int));

                    //Filtered count
                    var result = model.DB_Ticket_Datatable_Details(filter.filterType, filter.loadDateFrom, filter.loadDateTo, organization_Id, search, filter.sortColumn, filter.sortType, startIndex, pageSize, totalRecordNo)
                        .Select(p => new ChartPanelTicket
                        {
                            code = p.code,
                            feedback = p.feedback,
                            id = p.id,
                            name = p.name,
                            status = p.workFollowStatus_Id,
                            createDateStr = p.createdDate.ToString("dd/MM/yyyy hh:mm ttt"),

                        }).ToList();
                    //Get filter data
                    _result.recordDetails.totalRecords = Convert.ToInt32(totalRecordNo.Value);
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;
                    _result.lstTicket = result;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

    }
}
