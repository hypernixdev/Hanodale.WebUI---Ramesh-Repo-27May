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
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.ComponentModel;
using Hanodale.Utility;


namespace Hanodale.WebUI.Controllers
{
    public partial class CompanyProfileController : BaseController
    {

        #region Declaration
        readonly string PAGE_URL = string.Empty;

        #endregion

        #region Constructor

        private readonly ICompanyProfileService svc;

        //private DynamicDisplayNameMetadataProvider _metadataProvider = new DynamicDisplayNameMetadataProvider();


        public CompanyProfileController(ICompanyProfileService _bLService, ICommonService _svcCommon)
        {
            this.svcCommon = _svcCommon;
            this.sectionName = "CompanyProfile";
            this.svc = _bLService;
            this.menu_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["CompanyProfile_Menu_Id"]);
            PAGE_URL = this.sectionName + "/Index";

            //ModelMetadataProviders.Current = _metadataProvider;
        }
        #endregion

        #region Company Profile Details

        [AppAuthorize]
        public virtual ActionResult Index()
        {
            try
            {
                var _model = this.GetVisibleColumnForGridView(new CompanyProfileModel());

                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                _model.accessRight = _accessRight;

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.CompanyProfile.Views.Index, _model)
                        });
                    }
                    else
                    {
                        return this.Msg_AccessDeniedInView();
                    }
                }
                else
                {
                    return this.Msg_AccessDenied();
                }
            }
            catch (Exception ex)
            {
                //throw new ErrorException(err.Message);
                return Msg_ErrorInRetriveData(ex);
            }

        }

        [HttpPost]
        [AppAuthorize]
        public virtual JsonResult CompanyProfile()
        {
            try
            {
                var _model = this.GetVisibleColumnForGridView(new CompanyProfileModel(), 3);

                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                _model.accessRight = _accessRight;

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.CompanyProfile.Views.Index, _model)
                        });
                    }
                    else
                    {
                        return this.Msg_AccessDeniedInView();
                    }
                }
                else
                {
                    return this.Msg_AccessDenied();
                }
            }
            catch (Exception ex)
            {
                //throw new ErrorException(err.Message);
                return Msg_ErrorInRetriveData(ex);
            }
        }

        [Authorize]
        public virtual ActionResult BindCompanyProfile(DataTableModel param)
        {
            int totalRecordCount = 0;
            List<CompanyProfiles> filteredCompanyProfiles = null;
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        // Get login user Id
                        int userId = this.CurrentUserId;

                        var filter = new DatatableFilters { currentUserId = this.CurrentUserId, all = true, startIndex = param.iDisplayStart, pageSize = param.iDisplayLength, search = param.sSearch };
                        var companyProfileModel = this.svc.GetCompanyProfile(filter);

                        if (svc != null)
                        {
                            var lstFieldMetadata = this.GetVisibleIndexFieldMetadata();

                            //Sorting

                            filteredCompanyProfiles = companyProfileModel.lstCompanyProfile;
                            if (param.sSortDir_0 != null)
                            {
                                var sortColumnIndex = param.iSortCol_0;
                                var sortField = string.Empty;
                                if (lstFieldMetadata.Count > sortColumnIndex)
                                {
                                    if (sortColumnIndex > 0)
                                        sortColumnIndex--;

                                    sortField = lstFieldMetadata[sortColumnIndex].fieldName;
                                }

                                filteredCompanyProfiles = filteredCompanyProfiles.OrderByDynamic(sortField, (param.sSortDir_0 == "asc" ? false : true)).ToList();
                            }

                            var result = CompanyProfileData(filteredCompanyProfiles, this.CurrentUserId);
                            
                            var sEcho = param.sEcho;
                            var iTotalRecords = companyProfileModel.recordDetails.totalRecords;
                            var iTotalDisplayRecords = companyProfileModel.recordDetails.totalDisplayRecords;

                            return GetDataBindingJsonResult(sEcho, iTotalRecords, iTotalDisplayRecords, result);
                        }
                        else
                        {
                            return this.Msg_ErrorInService();
                        }
                    }
                    else
                    {
                            return this.Msg_AccessDeniedInViewOrEdit();
                    }
                }
                else
                {
                    return this.Msg_AccessDenied();
                }
            }
            catch (Exception ex)
            {
                //throw new ErrorException(err.Message);
                return Msg_ErrorInRetriveData(ex);
            }
        }

        public List<string[]> CompanyProfileData(List<CompanyProfiles> companyProfileEntry, int currentUserId)
        {
            var result = this.GetDatatableData<CompanyProfiles>(companyProfileEntry, currentUserId);
            return result;
        }



        [Authorize]
        [HttpPost]
        public virtual JsonResult RenderAction()
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                return Json(new
                {
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.CompanyProfile.Views.RenderAction, _accessRight)
                });
            }
            catch (Exception ex)
            {
                //throw new ErrorException(err.Message);
                return Msg_ErrorInRetriveData(ex);
            }
        }

        #endregion

        #region Add,Edit and Delete

        //private void SetDisplayName(string propertyName, string displayName)
        //{
        //    var modelMetaData = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(CompanyProfileModel));
        //    var property = modelMetaData.Properties.FirstOrDefault(p => p.PropertyName == propertyName);

        //    if (property != null)
        //    {
        //        bool hasKey = property.AdditionalValues.ContainsKey("Display");
        //        bool hasKey1 = property.AdditionalValues.ContainsKey("DisplayName");

        //        var displayAttribute = hasKey
        //            ? (DisplayAttribute)property.AdditionalValues["Display"]
        //            : new DisplayAttribute();

        //        var displayAttribute1 = hasKey1
        //           ? (DisplayAttribute)property.AdditionalValues["DisplayName"]
        //           : new DisplayAttribute();

        //        displayAttribute.Name = displayName;
        //        displayAttribute1.Name = displayName;
        //        property.AdditionalValues["Display"] = displayAttribute;
        //        property.AdditionalValues["DisplayName"] = displayAttribute1;
        //    }
        //}

        //private void OverrideMetadataForProperty<TModel>(string propertyName, string newDisplayName)
        //{
        //    ModelMetadata metadata = ModelMetadataProviders.Current.GetMetadataForProperty(null, typeof(TModel), propertyName);
        //    metadata.DisplayName = newDisplayName;
        //}

        //private void AddDynamicValidation(string propertyName, string validationType, string errorMessage)
        //{
        //    ModelState modelState;
        //    if (ModelState.TryGetValue(propertyName, out modelState))
        //    {
        //        var value = modelState.Value.AttemptedValue;

        //        switch (validationType.ToLower())
        //        {
        //            case "required":
        //                if (string.IsNullOrWhiteSpace(value))
        //                {
        //                    ModelState.AddModelError(propertyName, errorMessage);
        //                }
        //                break;
        //            // Add other validation types as needed
        //        }
        //    }
        //}

        //private void ModifyDisplayName(object model, string propertyName, string displayName)
        //{
        //    var property = model.GetType().GetProperty(propertyName);
        //    if (property != null)
        //    {
        //        var displayAttribute = (DisplayAttribute)Attribute.GetCustomAttribute(property, typeof(DisplayAttribute));
        //        if (displayAttribute != null)
        //        {
        //            displayAttribute.Name = displayName;
        //        }
        //    }
        //}

        //private void ModifyDisplayName(object model, string propertyName, string newDisplayName)
        //{
        //    // Get the property info
        //    var propertyInfo = model.GetType().GetProperty(propertyName);

        //    // Set the custom display name attribute
        //    var displayAttribute = new DisplayNameAttribute(newDisplayName);
        //    propertyInfo.SetCustomAttribute(displayAttribute);
        //}

        //private void ModifyDisplayName(Type modelType, string propertyName, string newDisplayName)
        //{
        //    // Get the metadata for the model
        //    var metadataType = ModelMetadataProviders.Current.GetMetadataForType(null, modelType);

        //    // Get the property metadata
        //    var property = metadataType.Properties.FirstOrDefault(p => p.PropertyName == propertyName);
        //    if (property != null)
        //    {
        //        // Set the new display name
        //        property.DisplayName = newDisplayName;
        //    }
        //}

        //private void ModifyDisplayName(object model, string propertyName, string newDisplayName)
        //{
        //    // Get the property info
        //    var propertyInfo = model.GetType().GetProperty(propertyName);

        //    // Modify the display name attribute
        //    var attribute = (DisplayAttribute)propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault();
        //    if (attribute != null)
        //    {
        //       // var field = attribute.GetType().GetField("_displayName", BindingFlags.Instance | BindingFlags.NonPublic);
        //        var field0 = attribute.GetType().GetField("_displayName");
        //        var field1 = attribute.GetType().GetField("_displayName", BindingFlags.Instance | BindingFlags.Public);
        //        var field2 = attribute.GetType().GetField("_displayName", BindingFlags.Instance | BindingFlags.NonPublic);

        //        var field01 = attribute.GetType().GetField("DisplayAttribute");
        //        var field11 = attribute.GetType().GetField("DisplayAttribute", BindingFlags.Instance | BindingFlags.Public);
        //        var field22 = attribute.GetType().GetField("DisplayAttribute", BindingFlags.Instance | BindingFlags.NonPublic);

        //        var field000 = attribute.GetType();
        //        var field = attribute.GetType().GetField("_name", BindingFlags.Instance | BindingFlags.NonPublic);
        //        if (field != null)
        //        {
        //            field.SetValue(attribute, newDisplayName);
        //        }
        //    }
        //}


        //private void ModifyDisplayName1<TModel>(string propertyName, string newDisplayName)
        //{
        //    var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(TModel));
        //    var property = metadata.Properties.SingleOrDefault(p=>p.PropertyName== propertyName);
        //    var displayAttribute = new DisplayAttribute { Name = property.DisplayName };
        //    if (displayAttribute != null)
        //    {
        //        var field = typeof(DisplayAttribute).GetField("_name", BindingFlags.Instance | BindingFlags.NonPublic);
        //        if (field != null)
        //        {
        //            field.SetValue(displayAttribute, newDisplayName);
        //        }
        //    }
        //}

        //private object ModifyDisplayName2(object model, string propertyName, string newDisplayName)
        //{
        //    var propertyInfo = model.GetType().GetProperty(propertyName);
        //    if (propertyInfo != null)
        //    {
        //        var attributes = propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true);
        //        if (attributes.Length > 0)
        //        {
        //            var displayAttribute = attributes[0] as DisplayAttribute;
        //            if (displayAttribute != null)
        //            {
        //                displayAttribute.Name = newDisplayName;
        //            }
        //        }
        //    }

        //    return model;
        //}

        [HttpPost]
        [Authorize]
        public virtual JsonResult Create()
        {
            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (!_accessRight.canAdd)
                {
                    return this.Msg_AccessDeniedInAdd();
                }

                if (svc != null)
                {
                    var _model = new CompanyProfileModel();

                    this.FillupFieldMetadata(_model, false);
                    //OverrideMetadataForProperty<CompanyProfileModel>("code", "Codeeeerirrt");


                    //var propertyInfo = typeof(CompanyProfileModel).GetProperty("code");
                    //propertyInfo.SetCustomAttribute(new CustomRangeAttribute(10, 100));

                    //var propertyInfo = _companyProfileModel.GetType().GetProperty("code");

                    //// Add Required attribute at runtime
                    //var requiredAttribute = new RequiredAttribute();
                    //propertyInfo.SetCustomAttribute(requiredAttribute);

                    //var propertyInfo = _companyProfileModel.GetType().GetProperty("code");
                    //var nameAttribute = (RequiredAttribute)propertyInfo.GetCustomAttribute(typeof(RequiredAttribute));
                    //nameAttribute.ErrorMessage = "Custom error message for Name field";

                    //var property = typeof(CompanyProfileModel).GetProperty("code");
                    //var requiredAttribute = (RequiredAttribute)Attribute.GetCustomAttribute(property, typeof(RequiredAttribute));
                    //requiredAttribute.ErrorMessage = "Custom error message";

                    //PropertyInfo propertyInfo = typeof(CompanyProfileModel).GetProperty("code");

                    //// Check if the property has the Required attribute
                    //RequiredAttribute requiredAttribute = propertyInfo.GetCustomAttribute<RequiredAttribute>();
                    //if (requiredAttribute != null)
                    //{
                    //    // Modify the error message at runtime
                    //    requiredAttribute.ErrorMessage = "Custom error message for this field.";
                    //}

                    //_metadataProvider.SetDynamicDisplayName<CompanyProfileModel>("code", "PAGE_HEADER_EDIT_COMPANYPROFILE");

                    //ModifyDisplayName(_companyProfileModel, "code", "PAGE_HEADER_EDIT_COMPANYPROFILE");

                    // ModifyDisplayName(_companyProfileModel, "code", "PAGE_HEADER_EDIT_COMPANYPROFILE");
                    //ModifyDisplayName1<CompanyProfileModel>("code", "PAGE_HEADER_EDIT_COMPANYPROFILE");
                    //_companyProfileModel = (CompanyProfileModel)ModifyDisplayName2(_companyProfileModel, "code", "PAGE_HEADER_EDIT_COMPANYPROFILE");

                    //var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(CompanyProfileModel));

                    //// Change the display name of the property dynamically
                    //var property = metadata.Properties.FirstOrDefault(p => p.PropertyName == "code");
                    //if (property != null)
                    //{
                    //    property.DisplayName = "PAGE_HEADER_EDIT_COMPANYPROFILE";
                    //    property.ShortDisplayName = "PAGE_HEADER_EDIT_COMPANYPROFILE";
                    //}



                    //var settings = this.GetFieldMetadata();

                    ////_companyProfileModel.GetType().InvokeMember("Name",
                    ////    BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                    ////    Type.DefaultBinder, _companyProfileModel, "Value");

                    //PropertyInfo prop = _companyProfileModel.GetType().GetProperty("code", BindingFlags.Public | BindingFlags.Instance);
                    //if (null != prop && prop.CanWrite)
                    //{
                    //    prop.SetValue(_companyProfileModel, "tesssss", null);
                    //}

                    //// Check if the property has the Required attribute
                    //RequiredAttribute requiredAttribute = prop.GetCustomAttribute<RequiredAttribute>();
                    //if (requiredAttribute != null)
                    //{
                    //    // Modify the error message at runtime
                    //    requiredAttribute.ErrorMessage = "Custom error message for this field.";
                    //    prop.SetValue(_companyProfileModel, "tesssss", null);

                    //}

                    _model.isActive = true;

                    var _companyTypeList = svcCommon.GetListModuleItem(_model.companyType_Id_Metadata.dropdownModuleType_Id);

                    _model.lstCompanyType = _companyTypeList.Select(a => new SelectListItem
                    {
                        Text = a.name,
                        Value = a.id.ToString(),
                        Selected = (a.id == _model.companyType_Id_Metadata.dropdownDefaultValue)
                    });

                    var _serviceList = svcCommon.GetListModuleItem(_model.service_Id_Metadata.dropdownModuleType_Id);

                    _model.lstService = _serviceList.Select(a => new SelectListItem
                    {
                        Text = a.name,
                        Value = a.id.ToString(),
                        Selected = (a.id == _model.service_Id_Metadata.dropdownDefaultValue)
                    });

                    // _model.effectiveDate = _model.effectiveDate_Metadata.dateDefaultValue;


                    _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");

                    //TryValidateModel(_companyProfileModel);


                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, MVC.CompanyProfile.Views.Create, _model)
                    });
                }
                else
                {
                    return this.Msg_ErrorInService();
                }

            }
            catch (Exception ex)
            {
                //throw new ErrorException(ex.Message);
                return Msg_ErrorInRetriveData(ex);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public virtual JsonResult SaveCompanyProfile(CompanyProfileModel model)
        {
            try
            {
                CheckModelState(ModelState, model.isEdit);
                if (ModelState.IsValid)
                {
                    AccessRightsModel _accessRight = new AccessRightsModel();

                    _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                    if (_accessRight != null)
                    {
                        int decrypted_Id = 0;
                        decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), model.id);
                        bool isUpdate = false;
                        if (decrypted_Id > 0)
                            isUpdate = true;

                        if (isUpdate)
                        {
                            if (!_accessRight.canEdit)
                            {
                                return this.Msg_AccessDeniedInEdit();
                            }
                        }
                        else
                        {
                            if (!_accessRight.canAdd)
                            {
                                return this.Msg_AccessDeniedInEdit();
                            }
                        }

                        if (svc != null)
                        {
                            CompanyProfiles entity = new CompanyProfiles();
                            entity.id = decrypted_Id;
                            entity.code = model.code;
                            entity.name = model.name;
                            entity.code = model.code;
                            entity.companyType_Id = model.companyType_Id;
                            entity.effectiveDate = model.effectiveDate;
                            entity.emailAddress = model.emailAddress;
                            entity.noOfUser = model.noOfUser;
                            entity.phoneNo = model.phoneNo;
                            entity.service_Id = model.service_Id;
                            entity.totalCapital = model.totalCapital;
                            entity.totalRevenue = model.totalRevenue;
                            entity.description = model.description;
                            entity.isActive = model.isActive;


                            bool isExists = svc.IsCompanyProfileExists(entity);
                            if (!isExists)
                            {
                                var save = svc.SaveCompanyProfile(entity);

                                if (save != null && save.isSuccess)
                                {
                                    var new_Id = Common.Encrypt(this.CurrentUserId.ToString(), save.id.ToString());
                                    return this.Msg_SuccessInSave(new_Id, isUpdate);

                                }
                                else
                                {
                                    return this.Msg_ErrorInSave(isUpdate);
                                }
                            }
                            else
                            {
                                return this.Msg_WarningExistRecord(Resources.MSG_COMPANYPROFILE_EXIST);
                            }
                        }
                        else
                        {
                            return this.Msg_ErrorInService();
                        }
                    }
                    else
                    {
                        return this.Msg_AccessDenied();
                    }

                }
                else
                {
                    return this.Msg_ErrorInvalidModel();
                }

            }
            catch (Exception ex)
            {
                //throw new ErrorException(err.Message);
                return Msg_ErrorInRetriveData(ex);
            }
        }

        private CompanyProfileModel GetCompanyProfileModel(string id, bool readOnly)
        {
            try
            {
                CompanyProfileModel _model = new CompanyProfileModel();
                this.FillupFieldMetadata(_model, true);
                _model.readOnly = readOnly;

                var decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), id);

                var companyProfile = svc.GetCompanyProfileById(decrypted_Id);

                if (companyProfile != null)
                {
                    _model.id = id;
                    _model.isEdit = true;
                    _model.name = companyProfile.name;
                    _model.code = companyProfile.code;
                    _model.companyType_Id = companyProfile.companyType_Id;
                    _model.effectiveDate = companyProfile.effectiveDate;
                    _model.emailAddress = companyProfile.emailAddress;
                    _model.noOfUser = companyProfile.noOfUser;
                    _model.phoneNo = companyProfile.phoneNo;
                    _model.service_Id = companyProfile.service_Id;
                    _model.totalCapital = companyProfile.totalCapital;
                    _model.totalRevenue = companyProfile.totalRevenue;
                    _model.description = companyProfile.description;
                    _model.isActive = companyProfile.isActive;


                    var _companyTypeList = svcCommon.GetListModuleItem(_model.companyType_Id_Metadata.dropdownModuleType_Id);

                    _model.lstCompanyType = _companyTypeList.Select(a => new SelectListItem
                    {
                        Text = a.name,
                        Value = a.id.ToString(),
                        Selected = (a.id == companyProfile.companyType_Id)
                    });

                    var _serviceList = svcCommon.GetListModuleItem(_model.service_Id_Metadata.dropdownModuleType_Id);

                    _model.lstService = _serviceList.Select(a => new SelectListItem
                    {
                        Text = a.name,
                        Value = a.id.ToString(),
                        Selected = (a.id == companyProfile.service_Id)
                    });
                }

                return _model;
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        [Authorize]
        public virtual JsonResult Edit(string id, bool readOnly)
        {
            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    int decrypted_Id = 0;
                    if ((_accessRight.canView && readOnly) || _accessRight.canEdit)
                    {
                        var _model = GetCompanyProfileModel(id, readOnly);

                        if (_model == null)
                        {
                            return this.Msg_ErrorInRetriveData();
                        }

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.CompanyProfile.Views.Create, _model)
                        });
                    }
                    else
                    {
                        return this.Msg_AccessDeniedInViewOrEdit();
                    }
                }
                else
                {
                    return this.Msg_AccessDenied();
                }
            }
            catch (Exception ex)
            {
                //throw new ErrorException(err.Message);
                return Msg_ErrorInRetriveData(ex);
            }
        }

        [HttpPost]
        [Authorize]
        public virtual JsonResult Maintenance(string id, bool readOnly)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            var _model = new CompanyProfileMaintenanceModel();

            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                _model.record_Id = id;
                _model.readOnly = readOnly;
                if (_accessRight != null)
                {
                    if ((_accessRight.canView && readOnly) || _accessRight.canEdit)
                    {
                        _model.tableProfile = this.GetTableProfileWithTab();
                        _model.companyProfile = GetCompanyProfileModel(id, readOnly);

                        if (_model.companyProfile == null)
                        {
                            return this.Msg_ErrorInService();
                        }

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.CompanyProfile.Views.Maintenance, _model)
                        });
                    }
                    else
                    {
                        return this.Msg_AccessDeniedInViewOrEdit();
                    }
                }
                else
                {
                    return this.Msg_AccessDenied();
                }
            }
            catch (Exception ex)
            {
                //throw new ErrorException(err.Message);
                return Msg_ErrorInRetriveData(ex);
            }

        }

        [HttpPost]
        [Authorize]
        public virtual ActionResult Delete(string id)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    int decrypted_Id = 0;
                    decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    if (_accessRight.canDelete)
                    {
                        if (svc != null)
                        {
                            bool isSuccess = svc.DeleteCompanyProfile(decrypted_Id);

                            if (isSuccess)
                            {
                                return this.Msg_SucessInDelete();
                            }
                            else
                            {
                                return this.Msg_ErrorInDelete();
                            }
                        }
                        else
                        {
                            return this.Msg_ErrorInService();
                        }
                    }
                    else
                    {
                        return this.Msg_AccessDeniedInDelete();
                    }
                }
            }
            catch (Exception ex)
            {
                return this.Msg_ErrorInRetriveData(ex);
            }
            return View();
        }
        #endregion

    }
}
