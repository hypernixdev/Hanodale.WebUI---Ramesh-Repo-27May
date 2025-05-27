using Hanodale.Domain.DTOs;
using Hanodale.Utility.Globalize;
using Hanodale.BusinessLogic;
using Hanodale.WebUI.Authentication;
using Hanodale.WebUI.Helpers;
using Hanodale.WebUI.Logging.Elmah;
using Hanodale.WebUI.Models;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using System.IO;
using Hanodale.Utility;
using System.Web.Configuration;


namespace Hanodale.WebUI.Controllers
{
    public partial class BusinessFileController : AuthorizedController
    {
        #region Declaration
        const string PAGE_URL = "Business/Index";
        const string UserId = "";
        #endregion

        #region Constructor

        private readonly IBusinessFileService svc; private readonly ICommonService svcCommon;

        public BusinessFileController(IBusinessFileService _bLService, ICommonService _commonService)
            
        {
            this.svc = _bLService; this.svcCommon = _commonService;
        }
        #endregion

        #region Business File Details

        [AppAuthorize]
        public virtual ActionResult Index(string id, bool readOnly)
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                _accessRight.elementId = id;
                _accessRight.readOnly = readOnly;
                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.BusinessFile.Views._Index, _accessRight)
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_VIEW
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        status = Common.Status.Denied.ToString(),
                        message = Resources.NO_ACCESS_RIGHTS
                    });
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        [HttpPost]
        [AppAuthorize]
        public virtual ActionResult BusinessFile()
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.BusinessFile.Views._Index, null)
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_VIEW
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        status = Common.Status.Denied.ToString(),
                        message = Resources.NO_ACCESS_RIGHTS
                    });
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        [Authorize]
        public virtual ActionResult BindBusinessFile(DataTableModel param, string myKey)
        {
            IEnumerable<BusinessFiles> filteredBusinessFiles = null;
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
                        // Get business Id
                        int businessId = 0;
                        if (myKey != null)
                            businessId = Common.DecryptToID(this.CurrentUserId.ToString(), myKey);

                        var businessFileModel = this.svc.GetBusinessFile(this.CurrentUserId, this.CurrentUserId, businessId, param.iDisplayStart, param.iDisplayLength, param.sSearch);

                        if (svc != null)
                        {
                            //Sorting
                            var sortColumnIndex = param.iSortCol_0;
                            Func<BusinessFiles, string> orderingFunction = (c => sortColumnIndex == 0 ? c.name :
                                                            sortColumnIndex == 1 ? c.description :
                                                            sortColumnIndex == 2 ? c.fileTypeName : c.modifiedDate.ToString()
                                                            );

                            filteredBusinessFiles = businessFileModel.lstBusinessFile;
                            if (param.sSortDir_0 != null)
                            {
                                if (param.sSortDir_0 == "asc")
                                    filteredBusinessFiles = filteredBusinessFiles.OrderBy(orderingFunction);
                                else
                                    filteredBusinessFiles = filteredBusinessFiles.OrderByDescending(orderingFunction);
                            }

                            var result = BusinessFileData(filteredBusinessFiles, this.CurrentUserId);
                            return Json(new
                            {
                                sEcho = param.sEcho,
                                iTotalRecords = businessFileModel.recordDetails.totalRecords,
                                iTotalDisplayRecords = businessFileModel.recordDetails.totalDisplayRecords,
                                aaData = result
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new
                            {
                                status = Common.Status.Error.ToString(),
                                message = Resources.MSG_ERR_SERVICE
                            });
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_EDIT
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        status = Common.Status.Denied.ToString(),
                        message = Resources.NO_ACCESS_RIGHTS
                    });
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        /// <summary>
        /// This method is to get the StockFile data as string array to bind into datatbale
        /// </summary>
        /// <param name="StockFileEntry">StockFile list</param>
        /// <returns></returns>
        public static List<string[]> BusinessFileData(IEnumerable<BusinessFiles> userEntry, int currentUserId)
        {
            return userEntry.Select(entry => new string[]
            {  
                entry.name,
                entry.description, 
                entry.fileTypeName, 
                entry.modifiedDate!=null?entry.modifiedDate.GetValueOrDefault().ToString("dd/MM/yyyy"):"-",
                Common.Encrypt(currentUserId.ToString(), entry.id.ToString())
            }).ToList();
        }

        [Authorize]
        [HttpPost] public virtual JsonResult RenderAction(bool readOnly)
        {

            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                _accessRight.readOnly = readOnly;
                return Json(new
                {
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.BusinessFile.Views._RenderAction, _accessRight)
                });
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        #endregion

        # region Add,Edit,Delete

        [HttpPost]
        [Authorize]
        public virtual ActionResult Create(string id)
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                BusinessFileModel _model = new BusinessFileModel();

                if (_accessRight != null)
                {
                    _model.business_Id = id;
                    if (_accessRight.canView && _accessRight.canEdit)
                    {
                        int businessfileId = Convert.ToInt32(WebConfigurationManager.AppSettings["BusinessFileType"]);

                        var businessfile = svcCommon.GetListModuleItem(businessfileId);
                        _model.lstbusinessFile = businessfile.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString()
                        });

                        _model.isEdit = false;
                        _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.BusinessFile.Views._Create, _model)
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_VIEW
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        status = Common.Status.Denied.ToString(),
                        message = Resources.NO_ACCESS_RIGHTS
                    });
                }

            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public virtual JsonResult SaveBusinessFile(BusinessFileModel model)
        {
            if (ModelState.IsValid)
            {
                var bussineId = Common.DecryptToID(this.CurrentUserId.ToString(), model.business_Id.ToString());
                AccessRightsModel _accessRight = new AccessRightsModel();
                string fileName = bussineId + "_" + PublicFunc.GenerateRandonCode() + "_";
                string direction = System.Configuration.ConfigurationManager.AppSettings["BusinessFilePath"];
                try
                {
                    int newId = 0;
                    newId = Common.DecryptToID(this.CurrentUserId.ToString(), model.id);
                    _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                    if (_accessRight != null)
                    {

                        if (newId > 0)
                        {
                            if (!_accessRight.canEdit)
                            {
                                return Json(new
                                {
                                    status = Common.Status.Denied.ToString(),
                                    message = Resources.NO_ACCESS_RIGHTS_EDIT
                                });
                            }
                        }
                        else
                        {
                            if (!_accessRight.canEdit)
                            {
                                return Json(new
                                {
                                    status = Common.Status.Denied.ToString(),
                                    message = Resources.NO_ACCESS_RIGHTS_ADD
                                });
                            }
                        }

                        if (svc != null)
                        {
                            bool hasFile = false;
                            for (int i = 0; i < Request.Files.Count; i++)
                            {
                                HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                                if (!string.IsNullOrEmpty(file.FileName))
                                {
                                    fileName += Path.GetFileName(file.FileName);
                                    file.SaveAs(Path.Combine(@"" + direction, fileName));
                                    hasFile = true;
                                    if (newId > 0)
                                    {
                                        var businessfile = svc.GetBusinessFileById(newId);
                                        if (businessfile.urlPath != fileName)
                                        {
                                            if (!string.IsNullOrEmpty(businessfile.urlPath))
                                            {
                                                string filePath = Path.Combine(@"" + direction, businessfile.urlPath);
                                                if (System.IO.File.Exists(filePath))
                                                {
                                                    System.IO.File.Delete(Path.Combine(@"" + direction, businessfile.urlPath));
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            BusinessFiles entity = new BusinessFiles();

                            entity.business_Id = bussineId;
                            entity.name = model.name;
                            entity.description = model.description;
                            entity.fileType_Id = model.fileType_Id;
                            if (hasFile)
                                entity.urlPath = fileName;
                            entity.createdBy = model.createdBy;
                            entity.createdDate = model.createdDate;
                            entity.modifiedBy = model.modifiedBy;
                            entity.modifiedDate = model.modifiedDate;
                            entity.modifiedBy = this.UserName;
                            entity.modifiedDate = DateTime.Now;

                            if (newId > 0)
                            {
                                entity.id = newId;
                            }
                            else
                            {
                                entity.createdBy = this.UserName;
                                entity.createdDate = DateTime.Now;
                            }
                            //bool isExists = svc.IsBusinessFileExists(entity);
                            //if (!isExists)
                            //{
                            var save = svc.SaveBusinessFile(this.CurrentUserId, entity, _accessRight.pageName);

                            if (save != null)
                            {
                                if (newId > 0)
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Success.ToString(),
                                        message = Resources.MSG_UPDATE,
                                        id = Common.Encrypt(this.CurrentUserId.ToString(), save.id.ToString())
                                    });
                                }
                                else
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Success.ToString(),
                                        message = Resources.MSG_SAVE,
                                        id = Common.Encrypt(this.CurrentUserId.ToString(), save.id.ToString())
                                    });
                                }
                            }
                            else
                            {
                                if (Request.Files.Count > 0 && newId > 0)
                                {
                                    if (!string.IsNullOrEmpty(model.urlPath))
                                    {
                                        string filePath = Path.Combine(@"" + direction, model.urlPath);
                                        if (System.IO.File.Exists(filePath))
                                        {
                                            System.IO.File.Delete(Path.Combine(@"" + direction, model.urlPath));
                                        }
                                    }
                                }

                                if (newId > 0)
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Success.ToString(),
                                        message = Resources.MSG_ERR_UPDATE
                                    });
                                }
                                else
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Error.ToString(),
                                        message = Resources.MSG_ERR_SAVE
                                    });
                                }
                            }
                            //}
                            //else
                            //{
                            //    return Json(new
                            //    {

                            //        status = Common.Status.Warning.ToString(),
                            //        message = Resources.BUSINESSFILE_RECORD_EXISTS.Replace("$BUSINESSFILE_NAME$", model.name)
                            //    });
                            //}
                        }
                        else
                        {
                            return Json(new
                            {
                                status = Common.Status.Error.ToString(),
                                message = Resources.MSG_ERR_SERVICE
                            });
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS
                        });
                    }
                }
                catch (Exception err)
                {
                    //string filePath = Path.Combine(@"" + direction, model.urlPath);
                    //if (System.IO.File.Exists(filePath))
                    //{
                    //    System.IO.File.Delete(Path.Combine(@"" + direction, model.urlPath));
                    //}

                    return Json(new
                    {
                        status = Common.Status.Error.ToString(),
                        message = Resources.MSG_ERR_SERVICE
                    });
                }
            }
            return Json(new
            {
                status = Common.Status.Error.ToString(),
                message = Resources.MSG_ERR_INVALIDMODEL
            });
        }

        [HttpPost]
        [Authorize]
        public virtual JsonResult Edit(string id)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            BusinessFileModel _model = new BusinessFileModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        if (svc != null)
                        {
                            int newId = 0;
                            newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                            var businessfile = svc.GetBusinessFileById(newId);

                            if (businessfile != null)
                            {
                                _model.id = Common.Encrypt(this.CurrentUserId.ToString(), businessfile.id.ToString());
                                _model.business_Id = Common.Encrypt(this.CurrentUserId.ToString(), businessfile.business_Id.ToString());
                                _model.name = businessfile.name;
                                _model.description = businessfile.description;
                                _model.urlPath = businessfile.urlPath;
                                _model.fileType_Id = businessfile.fileType_Id;

                                int businessfileId = Convert.ToInt32(WebConfigurationManager.AppSettings["BusinessFileType"]);

                                var businessfileType = svcCommon.GetListModuleItem(businessfileId);
                                _model.lstbusinessFile = businessfileType.Select(a => new SelectListItem
                                {
                                    Text = a.name,
                                    Value = a.id.ToString(),
                                    Selected = (a.id == businessfile.fileType_Id)
                                });

                                _model.isEdit = true;
                            }

                            else
                            {
                                return Json(new
                                {
                                    status = Common.Status.Success.ToString(),
                                    message = Resources.MSG_ERR_RETRIEVE
                                });
                            }
                        }
                        else
                        {
                            return Json(new
                            {
                                status = Common.Status.Error.ToString(),
                                message = Resources.MSG_ERR_SERVICE
                            });
                        }
                    }
                    else
                    {
                        //Redirect to access denied page
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_DELETE
                        });
                    }
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }

            return Json(new
            {
                viewMarkup = Common.RenderPartialViewToString(this, MVC.BusinessFile.Views._Create, _model)
            });
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
                    if (_accessRight.canView && _accessRight.canDelete)
                    {
                        if (svc != null)
                        {
                            int newId = 0;
                            newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                            var businessfile = svc.GetBusinessFileById(newId);
                            bool isSuccess = svc.DeleteBusinessFile(this.CurrentUserId, newId, _accessRight.pageName);
                            if (isSuccess)
                            {
                                if (!string.IsNullOrEmpty(businessfile.urlPath))
                                {
                                    string direction = System.Configuration.ConfigurationManager.AppSettings["BusinessFilePath"];
                                    string filePath = Path.Combine(@"" + direction, businessfile.urlPath);
                                    if (System.IO.File.Exists(filePath))
                                    {
                                        System.IO.File.Delete(Path.Combine(@"" + direction, businessfile.urlPath));
                                    }
                                }
                                return Json(new
                                {
                                    status = Common.Status.Success.ToString(),
                                    message = Resources.MSG_DELETE
                                });
                            }
                            else
                            {
                                return Json(new
                                {
                                    status = Common.Status.Error.ToString(),
                                    message = Resources.MSG_ERR_DELETE
                                });
                            }
                        }
                        else
                        {
                            return Json(new
                            {
                                status = Common.Status.Error.ToString(),
                                message = Resources.MSG_ERR_SERVICE
                            });
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_DELETE
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("REFERENCE"))
                {
                    return Json(new
                    {
                        status = Common.Status.Warning.ToString(),
                        message = Resources.MSG_RECORD_IN_USE
                    });
                }
                else
                {
                    throw new ErrorException(ex.Message);
                }
            }
            return View();
        }

        public virtual ActionResult DownloadBusinessFile(string id)
        {
            int newId = 0;
            newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
            var businessfile = svc.GetBusinessFileById(newId);
            string direction = System.Configuration.ConfigurationManager.AppSettings["BusinessFilePath"];
            var filepath = System.IO.Path.Combine(@"" + direction, businessfile.urlPath);
            if (!System.IO.File.Exists(filepath))
                return HttpNotFound();

            return File(filepath, MimeMapping.GetMimeMapping(filepath), businessfile.urlPath);
        }
        #endregion

    }
}
