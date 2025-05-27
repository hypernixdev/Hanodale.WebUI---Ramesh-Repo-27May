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
    public class ModuleItemService : IModuleItemService
    {
        #region ModuleItems

        /// <summary>
        /// This method is to get the Module Item details with search
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>ModuleItems list</returns> 

        public ModuleItemDetails GetModuleItemBySearch(int currentUserId, bool all, int startIndex, int pageSize, string search)
        {
            ModuleItemDetails _result = new ModuleItemDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    bool c, d;
                    c = Common.Visibility.True.ToString().ToLower().Contains(search.ToLower()); d = Common.Visibility.False.ToString().ToLower().Contains(search.ToLower());
                    //get total record
                    _result.recordDetails.totalRecords = model.ModuleItems.Where(p => (all ? true : p.visibility)).Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    //Filtered count
                    var result = model.ModuleItems.OrderByDescending(p => p.id).Where(p => (all ? true : p.visibility) && (p.name.Contains(search)
                        || p.description.Contains(search)
                        || p.ModuleType.name.Contains(search)
                        || (c ? p.visibility == true : d ? p.visibility == false : false)))
                        .Select(p => new ModuleItems
                        {
                            id = p.id,
                            name = p.name,
                            moduleName = p.ModuleType.name,
                            description = p.description,
                            visibility = p.visibility,
                            sortOrder = p.sortOrder
                        }).ToList();

                    //Get filter data
                    _result.recordDetails.totalDisplayRecords = result.Count;
                    _result.lstModuleItem = result.Skip(startIndex).Take(pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        /// <summary>
        /// This method is to get the ModuleItems details
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>ModuleItems list</returns>  
        public ModuleItemDetails GetModuleItem(int currentUserId, bool all, int startIndex, int pageSize)
        {
            ModuleItemDetails _result = new ModuleItemDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    //get total record
                    _result.recordDetails.totalRecords = model.ModuleItems.Where(p => (all ? true : p.visibility)).Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    _result.lstModuleItem = model.ModuleItems.Where(p => (all ? true : p.visibility)).OrderByDescending(p => p.id).Skip(startIndex).Take(pageSize).Select(p => new ModuleItems
                    {
                        id = p.id,
                        name = p.name,
                        description = p.description,
                        visibility = p.visibility,
                        sortOrder = p.sortOrder,
                        moduleName = p.ModuleType.name,
                        remarks = p.remarks
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
        /// This method is to save the ModuleItems details
        /// </summary> 
        public ModuleItems CreateModuleItem(int currentUserId, ModuleItems moduleItemEn, string pageName)
        {
            ModuleItem _moduleItemEn = new ModuleItem();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Add new stock
                    _moduleItemEn.modulType_Id = moduleItemEn.modulType_Id;
                    _moduleItemEn.name = moduleItemEn.name;
                    _moduleItemEn.description = moduleItemEn.description;
                    _moduleItemEn.sortOrder = moduleItemEn.sortOrder;
                    _moduleItemEn.visibility = moduleItemEn.visibility;
                    _moduleItemEn.remarks = moduleItemEn.remarks;

                    model.ModuleItems.Add(_moduleItemEn);
                    model.SaveChanges();

                    moduleItemEn.id = _moduleItemEn.id;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return moduleItemEn;
        }

        /// <summary>
        /// This method is to update the ModuleItems details
        /// </summary> 
        public ModuleItems UpdateModuleItem(int currentUserId, ModuleItems moduleItemEn, string pageName)
        {
            ModuleItem _moduleItemEn = new ModuleItem();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update stock
                    _moduleItemEn = model.ModuleItems.SingleOrDefault(p => p.id == moduleItemEn.id);
                    if (_moduleItemEn != null)
                    {
                        _moduleItemEn.modulType_Id = moduleItemEn.modulType_Id;
                        _moduleItemEn.name = moduleItemEn.name;
                        _moduleItemEn.description = moduleItemEn.description;
                        _moduleItemEn.sortOrder = moduleItemEn.sortOrder;
                        _moduleItemEn.visibility = moduleItemEn.visibility;
                        _moduleItemEn.remarks = moduleItemEn.remarks;

                    }
                    model.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return moduleItemEn;
        }

        /// <summary>
        /// This method is to delete the stock details
        /// </summary>
        /// <param name="stockId">stock id</param>  
        public bool DeleteModuleItem(int currentUserId, int moduleItemId, string pageName)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    ModuleItem _moduleItemEn = model.ModuleItems.SingleOrDefault(p => p.id == moduleItemId);

                    if (_moduleItemEn != null)
                    {
                        model.ModuleItems.Remove(_moduleItemEn);
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
        /// This method is to get the ModuleItems by ModuleItems id
        /// </summary>
        /// <param name="roleId">ModuleItems Id</param>
        /// <returns>ModuleItems details</returns>
        public ModuleItems GetModuleItemById(int id)
        {
            ModuleItems _moduleItemEn = new ModuleItems();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.ModuleItems.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _moduleItemEn = new ModuleItems
                        {
                            id = entity.id,
                            name = entity.name,
                            description = entity.description,
                            visibility = entity.visibility,
                            sortOrder = entity.sortOrder,
                            modulType_Id = entity.modulType_Id,
                            remarks = entity.remarks
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _moduleItemEn;
        }

        ///// <summary>
        ///// This method is to check the ModuleItems exists or not.
        ///// </summary>
        ///// <param name="stockName">ModuleItem Name</param>  
        public bool IsModuleItemExists(ModuleItems moduleItem)
        {
            ModuleItem _moduleItemEn = new ModuleItem();
            bool isExists = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (moduleItem.id == 0)
                    {
                        _moduleItemEn = model.ModuleItems.SingleOrDefault(p => p.name == moduleItem.name && p.modulType_Id == moduleItem.modulType_Id);
                        if (_moduleItemEn != null)
                            isExists = true;
                    }
                    else
                    {
                        _moduleItemEn = model.ModuleItems.SingleOrDefault(p => p.name == moduleItem.name && p.modulType_Id == moduleItem.modulType_Id && p.id != moduleItem.id);
                        if (_moduleItemEn != null)
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
