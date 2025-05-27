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
    public class OrganizationEmailService : IOrganizationEmailService
    {
        #region OrganizationEmail

        /// <summary>
        /// This method is to get the Task details with search
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>User list</returns>  
        public OrganizationEmailDetails GetOrganizationEmailBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            OrganizationEmailDetails _result = new OrganizationEmailDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record
                    _result.recordDetails.totalRecords = model.OrganizationEmails.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    var result = model.OrganizationEmails
                                .OrderBy(a => a.modifiedDate)
                                .Where(a => a.ModuleItem.name.Contains(search)
                                     || a.emailFrom.Contains(search)
                                     || a.emailTo.Contains(search))
                                     .Select(p => new OrganizationEmails
                                     {
                                         id = p.id,
                                         organization_Id = p.organization_Id,
                                         department_Id = p.department_Id,
                                         departmentName = p.ModuleItem.name,
                                         emailFrom = p.emailFrom,
                                         emailTo = p.emailTo,
                                         userName = p.userName,
                                         password = p.password,
                                         smptPort = p.smptPort,
                                         smtp = p.smtp,
                                         isSSL = p.isSSL,
                                         createdBy = p.createdBy,
                                         createdDate = p.createdDate,
                                         modifiedBy = p.modifiedBy,
                                         modifiedDate = p.modifiedDate
                                     }).ToList();

                    //Get filter data
                    _result.recordDetails.totalDisplayRecords = result.Count;
                    _result.lstOrganizationEmails = result.Skip(startIndex).Take(pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        /// <summary>
        /// This method is to get the Task details
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>User list</returns>  
        public OrganizationEmailDetails GetOrganizationEmail(int currentUserId, int userId, int startIndex, int pageSize)
        {
            OrganizationEmailDetails _result = new OrganizationEmailDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record
                    _result.recordDetails.totalRecords = model.OrganizationEmails.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    _result.lstOrganizationEmails = model.OrganizationEmails
                                      .OrderBy(a => a.modifiedDate)
                                      .Skip(startIndex).Take(pageSize)
                                      .Select(p => new OrganizationEmails
                                      {
                                          id = p.id,
                                          organization_Id = p.organization_Id,
                                          department_Id = p.department_Id,
                                          departmentName = p.ModuleItem.name,
                                          emailFrom = p.emailFrom,
                                          emailTo = p.emailTo,
                                          userName = p.userName,
                                          password = p.password,
                                          smptPort = p.smptPort,
                                          smtp = p.smtp,
                                          isSSL = p.isSSL,
                                          createdBy = p.createdBy,
                                          createdDate = p.createdDate,
                                          modifiedBy = p.modifiedBy,
                                          modifiedDate = p.modifiedDate
                                      }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        /// <summary>
        /// This method is to save the OrganizationEmail details
        /// </summary> 
        public OrganizationEmails CreateOrganizationEmail(int currentUserId, OrganizationEmails entity, string pageName)
        {
            OrganizationEmail _entity = new OrganizationEmail();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    //Add new Task
                    _entity.organization_Id = entity.organization_Id;
                    _entity.department_Id = entity.department_Id;
                    _entity.emailTo = entity.emailTo;
                    _entity.emailFrom = entity.emailFrom;
                    _entity.userName = entity.userName;
                    _entity.password = entity.password;
                    _entity.smtp = entity.smtp;
                    _entity.smptPort = entity.smptPort;
                    _entity.isSSL = entity.isSSL;
                    _entity.createdBy = entity.createdBy;
                    _entity.createdDate = entity.createdDate;
                    _entity.modifiedBy = entity.modifiedBy;
                    _entity.modifiedDate = entity.modifiedDate;

                    model.OrganizationEmails.Add(_entity);
                    model.SaveChanges();

                    entity.id = _entity.id;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entity;
        }

        /// <summary>
        /// This method is to update the OrganizationEmail details
        /// </summary> 
        public OrganizationEmails UpdateOrganizationEmail(int currentUserId, OrganizationEmails entity, string pageName)
        {
            OrganizationEmail _entity = new OrganizationEmail();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update Task
                    _entity = model.OrganizationEmails.SingleOrDefault(p => p.id == entity.id);
                    if (_entity != null)
                    {
                        _entity.department_Id = entity.department_Id;
                        _entity.emailTo = entity.emailTo;
                        _entity.emailFrom = entity.emailFrom;
                        _entity.userName = entity.userName;
                        _entity.password = entity.password;
                        _entity.smtp = entity.smtp;
                        _entity.smptPort = entity.smptPort;
                        _entity.isSSL = entity.isSSL;
                        _entity.modifiedBy = entity.modifiedBy;
                        _entity.modifiedDate = entity.modifiedDate;
                    }
                    model.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entity;
        }

        /// <summary>
        /// This method is to delete the OrganizationEmail details
        /// </summary>
        /// <param name="stockId">OrganizationEmail id</param>  
        public bool DeleteOrganizationEmail(int currentUserId, int id, string pageName)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    OrganizationEmail _entity = model.OrganizationEmails.SingleOrDefault(p => p.id == id);

                    if (_entity != null)
                    {
                        model.OrganizationEmails.Remove(_entity);
                    }
                    model.SaveChanges();
                }
                isDeleted = true;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return isDeleted;
        }

        /// <summary>
        /// This method is to get the OrganizationEmail by id
        /// </summary>
        /// <param name="assetId">OrganizationEmail Id</param>
        /// <returns>OrganizationEmail details</returns>
        public OrganizationEmails GetOrganizationEmailById(int id)
        {
            OrganizationEmails _entity = new OrganizationEmails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.OrganizationEmails.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _entity = new OrganizationEmails
                        {
                            id = entity.id,
                            organization_Id = entity.organization_Id,
                            department_Id = entity.department_Id,
                            emailFrom = entity.emailFrom,
                            emailTo = entity.emailTo,
                            userName = entity.userName,
                            password = entity.password,
                            smptPort = entity.smptPort,
                            smtp = entity.smtp,
                            isSSL = entity.isSSL,
                            createdBy = entity.createdBy,
                            createdDate = entity.createdDate,
                            modifiedBy = entity.modifiedBy,
                            modifiedDate = entity.modifiedDate

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _entity;
        }

        /// <summary>
        /// Get List of OrganizationEmail
        /// </summary> 
        /// <returns></returns>
        public List<OrganizationEmails> GetListOrganizationEmail()
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get Module Type List
                    return model.OrganizationEmails.Select(p => new OrganizationEmails
                    {
                        id = p.id,
                        organization_Id = p.organization_Id,
                        department_Id = p.department_Id,
                        emailFrom = p.emailFrom,
                        emailTo = p.emailTo,
                        userName = p.userName,
                        password = p.password,
                        smptPort = p.smptPort,
                        smtp = p.smtp,
                        isSSL = p.isSSL,
                        createdBy = p.createdBy,
                        createdDate = p.createdDate,
                        modifiedBy = p.modifiedBy,
                        modifiedDate = p.modifiedDate

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        ///// <summary>
        ///// This method is to check the Tasks exists or not.
        ///// </summary>
        ///// <param name="Asset">Asset</param>  
        public bool IsOrganizationEmailExists(OrganizationEmails entity)
        {
            OrganizationEmail _entity = new OrganizationEmail();
            bool isExists = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entity.id == 0)
                    {
                        _entity = model.OrganizationEmails.SingleOrDefault(p => p.organization_Id == entity.organization_Id && p.department_Id == entity.department_Id);
                        if (_entity != null)
                            isExists = true;
                    }
                    else
                    {
                        _entity = model.OrganizationEmails.SingleOrDefault(p => p.organization_Id == entity.organization_Id && p.department_Id == entity.department_Id && p.id != entity.id);
                        if (_entity != null)
                            isExists = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //we don't want to reveal any details to the client
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return isExists;
        }

        #endregion
    }
}
