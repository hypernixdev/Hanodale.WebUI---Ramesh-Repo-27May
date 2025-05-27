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
using System.Web;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;
using System.Reflection;
using System.Configuration;


namespace Hanodale.WebUI.Controllers
{
    public partial class AdhocReportController : AuthorizedController
    {
        #region Declaration
        const string PAGE_URL = "AdhocReport/Index";
        #endregion

        #region Constructor

        private readonly IAdhocReportService svc; private readonly ICommonService svcCommon;

        public AdhocReportController(IAdhocReportService _bLService, ICommonService _commonService)
            
        {
            this.svc = _bLService; this.svcCommon = _commonService;
        }

        #endregion

        #region AdhocReport Details

        [AppAuthorize]
        public virtual ActionResult Index()
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
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.AdhocReport.Views.Index, _accessRight)
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
        public virtual JsonResult AdhocReport()
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
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.AdhocReport.Views.Index, null)
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
        public virtual ActionResult BindAdhocReport(DataTableModel param)
        {
            int totalRecordCount = 0;
            IEnumerable<AdhocReports> filteredAdhocReports = null;
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

                        var AdhocReportModel = this.svc.GetAdhocReport(this.CurrentUserId, this.SubCostCenter, param.iDisplayStart, param.iDisplayLength, param.sSearch);

                        if (svc != null)
                        {
                            AdhocReportViewModel _AdhocReportViewModel = new AdhocReportViewModel();

                            //Sorting
                            var sortColumnIndex = param.iSortCol_0;
                            Func<AdhocReports, string> orderingFunction = (c => sortColumnIndex == 0 ? c.reportType_Id.ToString() :
                                                                         sortColumnIndex == 1 ? c.reportName : c.isVisible.ToString()
                                                            );

                            filteredAdhocReports = AdhocReportModel.lstAdhocReport;
                            if (param.sSortDir_0 != null)
                            {
                                if (param.sSortDir_0 == "asc")
                                    filteredAdhocReports = filteredAdhocReports.OrderBy(orderingFunction);
                                else
                                    filteredAdhocReports = filteredAdhocReports.OrderByDescending(orderingFunction);
                            }

                            var result = AdhocReportData(filteredAdhocReports, this.CurrentUserId);
                            return Json(new
                            {
                                sEcho = param.sEcho,
                                iTotalRecords = AdhocReportModel.recordDetails.totalRecords,
                                iTotalDisplayRecords = AdhocReportModel.recordDetails.totalDisplayRecords,
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
        /// This method is to get the AdhocReport data as string array to bind into datatbale
        /// </summary>
        /// <param name="AdhocReportEntry">AdhocReport list</param>
        /// <returns></returns>
        public static List<string[]> AdhocReportData(IEnumerable<AdhocReports> AdhocReportEntry, int currentUserId)
        {
            return AdhocReportEntry.Select(entry => new string[]
            {  
                entry.createdBy.ToString(),
                entry.reportName, 
                entry.isVisible.ToString(),
                 Common.Encrypt(currentUserId.ToString(), entry.id.ToString())
            }).ToList();
        }

        [Authorize]
        [HttpPost] public virtual JsonResult RenderAction()
        {

            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                return Json(new
                {
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.AdhocReport.Views.RenderAction, _accessRight)
                });
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        #endregion

        #region AdhocReport ADD,EDIT,DELETE

        [Authorize]
        public virtual JsonResult Create()
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            AdhocReportModel _model = new AdhocReportModel();

            _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");
            _model.isEdit = false;
            _model.isVisible = true;
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight.canView && _accessRight.canAdd)
                {

                    int currencyId = Convert.ToInt32(WebConfigurationManager.AppSettings["ReportType"]);

                    var currency = svcCommon.GetListModuleItem(currencyId);
                    _model.lstAdhocReportType = currency.Select(a => new SelectListItem
                    {
                        Text = a.name,
                        Value = a.id.ToString()
                    });

                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, MVC.AdhocReport.Views.Create, _model)
                    });
                }
                else
                {
                    //Redirect to access denied page
                    return Json(new
                    {
                        status = Common.Status.Denied.ToString(),
                        message = Resources.NO_ACCESS_RIGHTS_ADD
                    });
                }
            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public virtual JsonResult SaveAdhocReport(AdhocReportModel model)
        {
            if (ModelState.IsValid)
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                bool isExists = false;
                int check = 0;
                try
                {
                    _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                    if (_accessRight != null)
                    {
                        int newId = 0;
                        newId = Common.DecryptToID(this.CurrentUserId.ToString(), model.id);

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
                            if (!_accessRight.canAdd)
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
                            AdhocReports entity = new AdhocReports();
                            entity.organization_Id = this.SubCostCenter;
                            entity.reportType_Id = model.reportType_Id;
                            entity.reportName = model.reportName;
                            entity.reportFileName = Request.Files[0].FileName;
                            entity.remarks = model.remarks;
                            entity.isCommon = model.isCommon;
                            entity.isVisible = model.isVisible;

                            if (newId > 0)
                            {
                                entity.modifiedBy = this.UserName;
                                entity.modifiedDate = DateTime.Now;
                                entity.id = Common.DecryptToID(this.CurrentUserId.ToString(), model.id);

                            }
                            else
                            {
                                entity.createdBy = this.UserName;
                                entity.createdDate = DateTime.Now;
                            }
                            if (model.reportType_Id == 464)//183= Crystal report type
                            {
                                check = svc.IsAdhocReportExists(entity);
                            }
                            if (check == 0)
                            {
                                var OldEntity = svc.GetAdhocReportById(newId);
                                var save = svc.SaveAdhocReport(this.CurrentUserId, entity, _accessRight.pageName);
                                SaveFile(newId, OldEntity.reportFileName);
                                if (save != null)
                                {
                                    if (newId > 0)
                                    {
                                        return Json(new
                                        {
                                            status = Common.Status.Success.ToString(),
                                            message = Resources.MSG_UPDATE,
                                            id = Common.Encrypt(this.CurrentUserId.ToString(), save.id.ToString()),
                                        });
                                    }
                                    else
                                    {
                                        return Json(new
                                        {
                                            status = Common.Status.Success.ToString(),
                                            message = Resources.MSG_SAVE,
                                            id = Common.Encrypt(this.CurrentUserId.ToString(), save.id.ToString()),
                                        });
                                    }
                                }
                                else
                                {
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
                            }
                            else
                            {
                                if (check == 1)
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Warning.ToString(),
                                        message = Resources.AdhocReport_REPORTNAME_REPORTTYPE_RECORD_EXISTS
                                    });
                                }
                                else
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Warning.ToString(),
                                        message = Resources.AdhocReport_FILENAME_REPORTTYPE_RECORD_EXISTS
                                    });
                                }
                               
                            }
                        }
                        else
                        {
                            return Json(new
                            {
                                status = Common.Status.Error.ToString(),
                                message = Resources.NO_ACCESS_RIGHTS
                            });
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Error.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS
                        });
                    }
                }
                catch (Exception err)
                {
                    throw new ErrorException(err.Message);
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
        public virtual JsonResult Edit(string id, bool readOnly)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            AdhocReportModel _model = new AdhocReportModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    int newId = 0;
                    newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    _model.readOnly = readOnly;
                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        if (svc != null)
                        {
                            var AdhocReport = svc.GetAdhocReportById(newId);

                            if (AdhocReport != null)
                            {
                                _model.id = id;
                                _model.isEdit = true;
                                _model.organization_Id = AdhocReport.organization_Id;
                                _model.reportType_Id = AdhocReport.reportType_Id;
                                _model.reportFileName = AdhocReport.reportFileName;
                                _model.reportName = AdhocReport.reportName;
                                _model.remarks = AdhocReport.remarks;
                                _model.isCommon = AdhocReport.isCommon;
                                _model.isVisible = AdhocReport.isVisible;
                                int currencyId = Convert.ToInt32(WebConfigurationManager.AppSettings["ReportType"]);

                                var currency = svcCommon.GetListModuleItem(currencyId);
                                _model.lstAdhocReportType = currency.Select(a => new SelectListItem
                                {
                                    Text = a.name,
                                    Value = a.id.ToString(),
                                    Selected = (a.id == AdhocReport.reportType_Id)
                                });
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
                        if (!_accessRight.canView)
                        {
                            return Json(new
                            {
                                status = Common.Status.Denied.ToString(),
                                message = Resources.NO_ACCESS_RIGHTS_VIEW
                            });
                        }
                        if (!_accessRight.canEdit)
                        {
                            return Json(new
                            {
                                status = Common.Status.Denied.ToString(),
                                message = Resources.NO_ACCESS_RIGHTS_EDIT
                            });
                        }
                    }
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }

            return Json(new
            {
                viewMarkup = Common.RenderPartialViewToString(this, MVC.AdhocReport.Views.Create, _model)
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
                    int newId = 0;
                    newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    if (_accessRight.canView && _accessRight.canDelete)
                    {
                        if (svc != null)
                        {
                            var entity = svc.GetAdhocReportById(newId);
                            bool isSuccess = svc.DeleteAdhocReport(this.CurrentUserId, newId, _accessRight.pageName);
                            if (isSuccess)
                            {
                                DeleteFile(entity.reportFileName);
                            }
                            if (isSuccess)
                            {
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

        private void SaveFile(int id, string OldFileName)
        {
            HttpPostedFileBase file = Request.Files[0];
            string path = string.Empty;
            string filename = Path.GetFileName(file.FileName);
            string Directory = System.Configuration.ConfigurationManager.AppSettings["AdhocFilePath"];
            if (id > 0)
            {
                path = Path.Combine(@"" + Directory, OldFileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
               
                    //path = Path.Combine(@"" + Directory, filename);
                    file.SaveAs(path);
            }
            else
            {
                path = Path.Combine(@"" + Directory, filename);
                if (!System.IO.Directory.Exists(Directory))
                {
                    System.IO.Directory.CreateDirectory(Directory);
                }
                file.SaveAs(path);
            }
        }

        private void DeleteFile(string filename)
        {
            string path = string.Empty;
            string Directory = System.Configuration.ConfigurationManager.AppSettings["AdhocFilePath"];
            path = Path.Combine(@"" + Directory, filename);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        public virtual ActionResult DownloadAdhocReport(string id)
        {
            int newId = 0;
            newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
            var assetfile = svc.GetAdhocReportById(newId);
            string direction = System.Configuration.ConfigurationManager.AppSettings["AdhocFilePath"];
            var filepath = System.IO.Path.Combine(@"" + direction, assetfile.reportFileName);
            if (!System.IO.File.Exists(filepath))
                return HttpNotFound();
            if (System.IO.File.Exists(Server.MapPath("~/Content/") + assetfile.reportFileName))
            {
                System.IO.File.Delete(Server.MapPath("~/Content/") + assetfile.reportFileName);
            }
            System.IO.File.Copy(filepath, Server.MapPath("~/Content/") + assetfile.reportFileName);

            Stream stFile = Common.GetPDFFile(filepath, Server.MapPath("~/Content/") + assetfile.reportFileName, this.SubCostCenter.ToString(), this.CurrentUserId.ToString());

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            stFile.Seek(0, SeekOrigin.Begin);
            return File(stFile, "application/pdf", assetfile.reportFileName.TrimEnd('.') + ".pdf");

            //ReportDocument crystalReport = new ReportDocument(); // creating object of crystal report
            //crystalReport.Load(Server.MapPath("~/Content/")+assetfile.reportFileName); // path of report 
            //crystalReport.SetParameterValue(0, 2);
            //crystalReport.SetDatabaseLogon(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"], ConfigurationManager.AppSettings["DBServer"], ConfigurationManager.AppSettings["Database"]);
            ////crystalReport.SetDataSource(dt); // binding datatable
            //Stream st = crystalReport.ExportToStream(ExportFormatType.PortableDocFormat);
            //System.IO.File.Delete(Server.MapPath("~/Content/")+assetfile.reportFileName);

        }
        #endregion
    }
}
