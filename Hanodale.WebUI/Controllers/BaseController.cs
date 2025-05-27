using Hanodale.BusinessLogic;
using Hanodale.WebUI.Authentication;
using Hanodale.WebUI.Helpers;
using Hanodale.WebUI.Models;
using Microsoft.Practices.ServiceLocation;
using System.ServiceModel;
using System.Web.Mvc;
using System.Linq;
using Hanodale.Utility.Globalize;
using Hanodale.WebUI.Logging.Elmah;
using System;
using Hanodale.Domain.DTOs;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Reflection;
using System.Data;
using System.ComponentModel.DataAnnotations;


namespace Hanodale.WebUI.Controllers
{
    public partial class BaseController : AuthorizedController
    {
        protected ICommonService svcCommon;

        protected string sectionName = string.Empty;
        protected int? menu_Id = null;
        protected int? parentMenu_Id = null;

        protected Dictionary<string, Nullable<int>> dropdownItemList = new Dictionary<string, Nullable<int>>();


        //private void SetDisplayLabel(object entity,TableProfileMetadataModel item)
        //{
        //    var elem = entity.GetType().GetProperty(item.fieldName, BindingFlags.Public | BindingFlags.Instance);
        //    if (null != elem && elem.CanWrite)
        //    {
        //        var _displayAttribute = elem.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
        //        if (_displayAttribute != null)
        //        {
        //            var name = _displayAttribute.Name;
        //            if (string.IsNullOrEmpty(item.resourceNameForGridView))
        //            {
        //                item.resourceNameForGridView = name;
        //            }

        //            item.labelNameForGridView = Resources.ResourceManager.GetString(item.resourceNameForGridView);

        //            if (string.IsNullOrEmpty(item.labelNameForGridView))
        //            {
        //                item.labelNameForGridView = item.fieldName;
        //            }


        //            if (string.IsNullOrEmpty(item.resourceName))
        //            {
        //                item.resourceName = name;
        //            }

        //            item.labelName = Resources.ResourceManager.GetString(item.resourceName);

        //            if (string.IsNullOrEmpty(item.labelNameForGridView))
        //            {
        //                item.labelName = item.fieldName;
        //            }
        //        }
        //    }
        //}

        //private string SetDisplayLabel(object entity, string fieldName, string resourceName)
        //{
        //    var labelName = fieldName;
        //    var elem = entity.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
        //    if (null != elem && elem.CanWrite)
        //    {
        //        var _displayAttribute = elem.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
        //        if (_displayAttribute != null)
        //        {
        //            var name = _displayAttribute.Name;

        //            if (string.IsNullOrEmpty(resourceName))
        //            {
        //                resourceName = name;
        //            }

        //            var resourceValue = Resources.ResourceManager.GetString(resourceName);

        //            if (!string.IsNullOrEmpty(resourceValue))
        //            {
        //                labelName = resourceValue;
        //            }
        //        }
        //    }

        //    return labelName;
        //}

        protected int GetDropdownParentValue(string key)
        {
            if (dropdownItemList == null || string.IsNullOrEmpty(key))
            {
                return 0;
            }

            var obj = dropdownItemList[key];
            if (obj == null)
                return 0;

            return obj??0;
        }

        protected void FillupFieldMetadata(object entity, bool isEdit)
        {
            try
            {
                string indicator = "_Metadata";
                var settings = GetFieldMetadata(isEdit);
                if (settings != null)
                {
                    foreach (var item in settings)
                    {
                        //item.labelName = SetDisplayLabel(entity, item.fieldName, item.resourceName);

                        PropertyInfo prop = entity.GetType().GetProperty(item.fieldName + indicator, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                        {
                            prop.SetValue(entity, item, null);
                        }
                    }
                }

                // for the properties which is not exist in the MetadatField table
                var lstProperty = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                if (lstProperty != null)
                {
                    var collection = lstProperty.Where(p => p.Name.EndsWith(indicator)).ToList();
                    if (collection != null)
                    {
                        var defaultValue = new TableProfileMetadataModel
                        {
                            isEditableInCreate = true,
                            visibilityInCreate = true,
                            visibilityInGridView = true
                        };

                        foreach (var item in collection)
                        {
                            if (item.GetValue(entity) == null && item.CanWrite)
                            {
                                item.SetValue(entity, defaultValue, null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }
        }

        private List<TableProfileMetadataModel> GetFieldMetadata(bool isEdit)
        {
            try
            {
                var _tableProfile = new TableProfiles();
                _tableProfile.name = sectionName;
                _tableProfile.menu_Id = menu_Id;
                _tableProfile.parentMenu_Id = parentMenu_Id;

                var result = new List<TableProfileMetadataModel>();

                var settings = this.svcCommon.GetFieldMetadata(_tableProfile);
                if (settings != null)
                {
                    foreach (var p in settings)
                    {
                        var obj = new TableProfileMetadataModel();
                        obj.fieldName = p.fieldName;
                        obj.valueFieldName = p.valueFieldName;
                        obj.id = p.id;
                        obj.isEditableInCreate = (p.visibility && (isEdit ? p.isEditableInEdit : p.isEditableInCreate));
                        obj.isMandatory = p.isMandatory;
                        obj.mandatoryClass = p.isMandatory ? "" : " ignore";
                        obj.mandatoryClass += obj.isEditableInCreate ? "" : " disabled";
                        obj.sortOrder = p.sortOrder;
                        obj.templateName = p.templateName;
                        obj.resourceKeyNameForGridView = p.resourceKeyNameForGridView;
                        obj.validationRule = p.validationRule;
                        obj.visibilityInCreate = (p.visibility && (isEdit ? p.visibilityInEdit : p.visibilityInCreate));
                        obj.visibilityInGridView = p.visibilityInGridView;
                        if (p.metadataList != null)
                        {
                            obj.dropdownModuleType_Id = p.metadataList.lookUpValue ?? 0;
                            obj.dropdownDefaultValue = p.metadataList.lookDefaultValue ?? 0;
                            if (p.metadataList.hasParent)
                            {
                                obj.hasDropdownParent = p.metadataList.hasParent;
                                obj.dropdownParentFieldName = p.metadataList.parentFieldName;
                                obj.dataRetrieveURL = p.metadataList.dataRetrieveURL;
                            }
                        }

                        if (p.metadataText != null)
                        {
                            obj.stringMaxLength = p.metadataText.maxLength ?? 0;
                            obj.stringMinLength = p.metadataText.minLength ?? 0;
                            obj.stringDefaultValue = p.metadataText.defaultValue;
                        }

                        if (p.metadataInteger != null)
                        {
                            obj.intMaxLength = p.metadataInteger.maxValue ?? 0;
                            obj.intMinLength = p.metadataInteger.minValue ?? 0;
                            obj.intDefaultValue = p.metadataInteger.defaultValue;
                        }

                        if (p.metadataDecimal != null)
                        {
                            obj.decimalMaxLength = p.metadataDecimal.maxValue ?? 0;
                            obj.decimalMinLength = p.metadataDecimal.minValue ?? 0;
                            obj.decimalDefaultValue = p.metadataDecimal.defaultValue;
                        }

                        if (p.metadataDateTime != null)
                        {
                            obj.maxDate = p.metadataDateTime.maxDate;
                            obj.minDate = p.metadataDateTime.minDate;
                            obj.isAllowPastDate = p.metadataDateTime.isAllowPastDate;
                            obj.isFollowCalendarSetting = p.metadataDateTime.isFollowCalendarSetting;
                            if (p.metadataDateTime.isCurrentDateDefault == true)
                            {
                                obj.dateDefaultValue = DateTime.Now;
                            }
                        }


                        result.Add(obj);

                    }
                }


                return result;
            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }
        }

        private TableProfileModel GetTableProfile()
        {
            try
            {
                var _tableProfileFilter = new TableProfiles();
                _tableProfileFilter.name = sectionName;
                _tableProfileFilter.menu_Id = menu_Id;
                _tableProfileFilter.parentMenu_Id = parentMenu_Id;


                var result = new TableProfileModel();
                result.lstTableProfileTab = new List<TableProfileModel>();

                var _tableProfile = this.svcCommon.GetTableProfileWithTabs(_tableProfileFilter);
                if (_tableProfile != null)
                {
                    result.id = _tableProfile.id;
                    result.name = _tableProfile.name;
                    result.resourceNameKey = _tableProfile.resourceNameKey;
                    result.url = _tableProfile.url;
                    if (!string.IsNullOrEmpty(_tableProfile.url))
                    {
                        var _urlArray = _tableProfile.url.Split('/');
                        if (_urlArray.Count() > 2)
                        {
                            result.controllerName = _urlArray[0];
                            result.actionName = _urlArray[1];
                        }
                        else if (_urlArray.Count() > 1)
                        {
                            result.controllerName = _urlArray[0];
                            result.actionName = "Index";
                        }
                    }
                    result.visibility = _tableProfile.visibility;
                    result.sortOrder = _tableProfile.sortOrder;
                    result.icon = _tableProfile.icon;

                    if (_tableProfile.lstTableProfileTabs != null)
                    {
                        var lstTab = _tableProfile.lstTableProfileTabs.OrderBy(p => p.sortOrder).ToList();
                        foreach (var p in lstTab)
                        {
                            var obj = new TableProfileModel();
                            obj.id = p.id;
                            obj.name = p.name;
                            obj.resourceNameKey = p.resourceNameKey;
                            obj.url = p.url;
                            obj.visibility = p.visibility;
                            obj.sortOrder = p.sortOrder;
                            obj.icon = p.icon;

                            result.lstTableProfileTab.Add(obj);
                        }
                    }
                }


                return result;
            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }
        }

        protected TableProfileModel GetTableProfileWithTab()
        {
            try
            {
                var result = GetTableProfile();
                if (result == null)
                {
                    result = new TableProfileModel();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }
        }

        protected List<TableProfileMetadataModel> GetVisibleIndexFieldMetadata()
        {
            try
            {
                var result = GetFieldMetadata(false);
                if (result != null)
                    result = result.Where(p => p.visibilityInGridView).OrderBy(p => p.sortOrder).ThenBy(p => p.fieldName).ToList();
                else
                    result = new List<TableProfileMetadataModel>();

                return result;
            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }
        }

        protected GridViewIndexModel GetVisibleColumnForGridView(object entity, int gridViewButtonCount = 3)
        {
            try
            {
                var resultModel = new GridViewIndexModel();
                resultModel.lstColumn = new List<GridViewColumnModel>();
                resultModel.gridViewButtonColumnWidth = gridViewButtonCount * 30;

                var result = GetFieldMetadata(false);
                if (result != null)
                {
                    resultModel.lstColumn = result.Where(p => p.visibilityInGridView).OrderBy(p => p.sortOrder).ThenBy(p => p.fieldName).Select(p => new GridViewColumnModel
                    {
                        fieldName = p.fieldName,
                        resourceKeyName = p.resourceKeyNameForGridView,
                        //label = SetDisplayLabel(entity, p.fieldName, p.resourceNameForGridView),
                        sortOrder = p.sortOrder ?? 0
                    }).ToList();

                    if (resultModel.lstColumn.Count > 0)
                    {
                        var obj = resultModel.lstColumn.FirstOrDefault(p => p.sortOrder == 1);
                        if (obj != null)
                            obj.isExpandable = true;
                    }
                }

                resultModel.columnCount = resultModel.lstColumn.Count + 1;

                return resultModel;
            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }
        }

        protected void CheckModelState(ModelStateDictionary modelState, bool isEdit)
        {
            var collection = GetFieldMetadata(isEdit);

            if (collection != null && modelState != null)
            {
                foreach (var item in collection)
                {
                    var _field = modelState.SingleOrDefault(p => p.Key == item.fieldName);
                    if (!string.IsNullOrEmpty(_field.Key) && (!item.isMandatory || !item.visibilityInCreate))
                    {
                        modelState.Remove(_field.Key);
                    }
                }
            }
        }

        protected List<string[]> GetDatatableData<T>(List<T> lstData, int currentUserId)
        {
            try
            {
                var lstField = GetVisibleIndexFieldMetadata();
                var result = new List<string[]>();
                if (lstField != null)
                {
                    result = GetResultsTable(lstField, lstData, currentUserId);
                    return result;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }
        }

        public List<string[]> GetResultsTable<T>(List<TableProfileMetadataModel> lstField, List<T> lstData, int currentUserId)
        {
            var user_Id = currentUserId.ToString();
            var result = new List<string[]>();
            // Create the output table.
            DataTable dt = new DataTable();
            var fieldCount = lstField.Count;
            var rowCount = lstData.Count;
            // Loop through all process names.
            for (int i = 0; i < rowCount; i++)
            {
                var obj = new string[fieldCount + 1];
                var _data = lstData[i];
                for (int j = 0; j < fieldCount; j++)
                {
                    var _fieldName = lstField[j].fieldName;
                    var _valueFieldName = lstField[j].valueFieldName;
                    if (!string.IsNullOrEmpty(_valueFieldName))
                    {
                        _fieldName = _valueFieldName;
                    }

                    obj[j] = GetPropValue(_data, _fieldName);
                }

                obj[fieldCount] = Common.Encrypt(user_Id, GetPropValue(_data, "id").ToString());

                result.Add(obj);
            }
            return result;
        }

        //public object GetPropValueList(List<object> obj, string propName)
        //{
        //    var res = obj.Select(p => GetType().GetProperty(propName).GetValue(obj, null));
        //    return res;
        //}

        //public string GetPropValue(object obj, string propName)
        //{
        //    var res = obj.GetType().GetProperty(propName).GetValue(obj, null);
        //    if (res != null)
        //        return res.ToString();
        //    else
        //        return string.Empty;
        //}

        public object GetPropValueList(List<object> obj, string propName)
        {
            var _property = GetType().GetProperty(propName);
            if (_property != null)
            {
                var res = obj.Select(p => _property.GetValue(obj, null));
                return res;
            }
            else
            {
                return null;
            }
        }

        public string GetPropValue(object obj, string propName)
        {
            var result = string.Empty;
            
            var _property = obj.GetType().GetProperty(propName);
            if (_property != null)
            {
                var type = _property.PropertyType.Name;
                var res = _property.GetValue(obj, null);
                DateTime parsedDateTime;
                // System.Diagnostics.Debug.WriteLine(type + " " + res);
                if (res != null)
                {
                    if (type == "String[]")
                    {
                        object[] castValue = (object[])res;

                        if (castValue.Count() > 0)
                        {
                            result = string.Join(",", castValue);
                        }
                    }
                    //else if (type == "Nullable`1")
                    //{
                    //    DateTime castValue = Convert.ToDateTime(res);

                    //    if (castValue != null)
                    //    {
                    //        result = castValue.ToString("dd/MM/yyyy HH:mm");
                    //    }
                    //}
                    else if (_property.PropertyType.FullName.Contains("System.DateTime"))
                    {
                        try
                        {
                            DateTime castValue = Convert.ToDateTime(res);
                            result = castValue.ToString("dd/MM/yyyy hh:mm tt");
                        } catch (Exception ex)
                        {
                            result = res.ToString();
                        }
                    }
                    else
                    {
                        result = res.ToString();
                    }

                }
            }

            return result;
        }

        protected JsonResult Msg_ErrorInService()
        {
            return Json(new
            {
                status = Common.Status.Error.ToString(),
                message = Resources.MSG_ERR_SERVICE
            });
        }

        protected JsonResult Msg_ErrorInRetriveData()
        {
            return Json(new
            {
                status = Common.Status.Error.ToString(),
                message = Resources.MSG_ERR_RETRIEVE
            });
        }

        protected JsonResult Msg_ErrorInRetriveData(Exception ex)
        {
            if (ex.Message.Contains("REFERENCE"))
            {
                return Msg_RecordInUse();
            }
            else
            {
                return Json(new
                {
                    status = Common.Status.Error.ToString(),
                    message = Resources.MSG_ERR_RETRIEVE
                });
            }
        }

        protected JsonResult Msg_ErrorInDelete()
        {
            return Json(new
            {
                status = Common.Status.Error.ToString(),
                message = Resources.MSG_ERR_DELETE
            });
        }

        protected JsonResult Msg_ErrorInSave(bool isUpdate)
        {
            if (isUpdate)
            {
                return Msg_ErrorInUpdate();
            }
            else
            {
                return Msg_ErrorInSave();
            }
        }

        protected JsonResult Msg_ErrorInSave()
        {
            return Json(new
            {
                status = Common.Status.Error.ToString(),
                message = Resources.MSG_ERR_SAVE
            });
        }

        protected JsonResult Msg_ErrorInUpdate()
        {
            return Json(new
            {
                status = Common.Status.Error.ToString(),
                message = Resources.MSG_ERR_UPDATE
            });
        }

        protected JsonResult Msg_ErrorInvalidModel()
        {
            return Json(new
            {
                status = Common.Status.Error.ToString(),
                message = Resources.MSG_ERR_INVALIDMODEL
            });
        }




        protected JsonResult Msg_AccessDenied()
        {
            return Json(new
            {
                status = Common.Status.Denied.ToString(),
                message = Resources.NO_ACCESS_RIGHTS
            });
        }

        protected JsonResult Msg_AccessDeniedInAdd()
        {
            return Json(new
            {
                status = Common.Status.Denied.ToString(),
                message = Resources.NO_ACCESS_RIGHTS_ADD
            });
        }

        protected JsonResult Msg_AccessDeniedInEdit()
        {
            return Json(new
            {
                status = Common.Status.Denied.ToString(),
                message = Resources.NO_ACCESS_RIGHTS_EDIT
            });
        }

        protected JsonResult Msg_AccessDeniedInView()
        {
            return Json(new
            {
                status = Common.Status.Denied.ToString(),
                message = Resources.NO_ACCESS_RIGHTS_VIEW
            });
        }

        protected JsonResult Msg_AccessDeniedInDelete()
        {
            return Json(new
            {
                status = Common.Status.Denied.ToString(),
                message = Resources.NO_ACCESS_RIGHTS_DELETE
            });
        }

        protected JsonResult Msg_AccessDeniedInViewOrEdit()
        {
            return Json(new
            {
                status = Common.Status.Denied.ToString(),
                message = Resources.NO_ACCESS_RIGHTS_VIEW_OR_EDIT
            });
        }

        protected JsonResult Msg_RecordInUse()
        {
            return Json(new
            {
                status = Common.Status.Warning.ToString(),
                message = Resources.MSG_RECORD_IN_USE
            });
        }

        protected JsonResult Msg_WarningExistRecord(string message, Dictionary<string, string> dic)
        {
            if (message == null)
                message = string.Empty;

            foreach (var item in dic)
            {
                message = message.Replace(item.Key, item.Value);
            }

            return Json(new
            {
                status = Common.Status.Warning.ToString(),
                message = message
            });
        }

        protected JsonResult Msg_WarningExistRecord(string message, params string[] arguments)
        {
            if (message == null)
                message = string.Empty;

            
            return Json(new
            {
                status = Common.Status.Warning.ToString(),
                message = string.Format(message, arguments)
            });
        }

        protected JsonResult Msg_WarningExistRecord(string message)
        {
            return Json(new
            {
                status = Common.Status.Warning.ToString(),
                message = message
            });
        }




        protected JsonResult Msg_SucessInSave()
        {
            return Json(new
            {
                status = Common.Status.Success.ToString(),
                message = Resources.MSG_SAVE
            });
        }

        protected JsonResult Msg_SucessInUpdate()
        {
            return Json(new
            {
                status = Common.Status.Success.ToString(),
                message = Resources.MSG_UPDATE
            });
        }

        protected JsonResult Msg_SucessInDelete()
        {
            return Json(new
            {
                status = Common.Status.Success.ToString(),
                message = Resources.MSG_DELETE
            });
        }

        protected JsonResult Msg_SuccessInSave(string id, bool isUpdate)
        {
            if (isUpdate)
            {
                return Msg_SuccessInUpdate(id);
            }
            else
            {
                return Msg_SuccessInSave(id);
            }
        }

        protected JsonResult Msg_SuccessInSave(string id)
        {
            return Json(new
            {
                status = Common.Status.Success.ToString(),
                message = Resources.MSG_SAVE,
                id = id,
            });
        }

        protected JsonResult Msg_SuccessInUpdate(string id)
        {
            return Json(new
            {
                status = Common.Status.Success.ToString(),
                message = Resources.MSG_UPDATE,
                id = id,
            });
        }

        protected JsonResult GetDataBindingJsonResult(string sEcho, int iTotalRecords, int iTotalDisplayRecords, List<string[]> result)
        {
            return Json(new
            {
                sEcho = sEcho,
                iTotalRecords = iTotalRecords,
                iTotalDisplayRecords = iTotalDisplayRecords,
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }


    }
}
