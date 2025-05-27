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
using System.Data;
using System.Data.OleDb;
using System.Xml;


namespace Hanodale.WebUI.Controllers
{
    public partial class FileHistoryController : AuthorizedController
    {
        #region Declaration
        const string PAGE_URL = "TrainingStaff/Index";
        #endregion

        #region Constructor

        private readonly IFileHistoryService svc; private readonly ICommonService svcCommon;
        private readonly ITrainingStaffService svcTrainingStaff;
        

        public FileHistoryController(IFileHistoryService _bLService, ICommonService _commonService
            , ITrainingStaffService _trainingStaffService)
            
        {
            this.svc = _bLService; this.svcCommon = _commonService;
            this.svcTrainingStaff = _trainingStaffService;
        }

        #endregion

        #region FileHistory Details

        [AppAuthorize]
        public virtual ActionResult Index(string id)
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
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.FileHistory.Views.Index, id)
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
        public virtual JsonResult FileHistory()
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
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.FileHistory.Views.Index, null)
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
        public virtual ActionResult BindFileHistory(DataTableModel param, string myKey)
        {
            int totalRecordCount = 0;
            bool Istraining = false;
            IEnumerable<FileUploadHistorys> filteredFileHistory = null;
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

                        Istraining = Convert.ToBoolean(myKey);

                        var FileHistoryModel = this.svc.GetFileHistory(this.CurrentUserId, this.CurrentUserId, param.iDisplayStart, param.iDisplayLength, param.sSearch, Istraining);

                        if (svc != null)
                        {
                            FileHistoryViewModel _FileHistoryViewModel = new FileHistoryViewModel();

                            //Sorting
                            var sortColumnIndex = param.iSortCol_0;
                            Func<FileUploadHistorys, string> orderingFunction = (c => sortColumnIndex == 0 ? c.fileName :
                                                                         sortColumnIndex == 1 ? c.totalRecords.ToString() :
                                                                         sortColumnIndex == 1 ? c.createdBy : c.createdDate.ToString()
                                                            );

                            filteredFileHistory = FileHistoryModel.lstFileUploadHistory;
                            if (param.sSortDir_0 != null)
                            {
                                if (param.sSortDir_0 == "asc")
                                    filteredFileHistory = filteredFileHistory.OrderBy(orderingFunction);
                                else
                                    filteredFileHistory = filteredFileHistory.OrderByDescending(orderingFunction);
                            }

                            var result = FileHistoryData(filteredFileHistory, this.CurrentUserId);
                            return Json(new
                            {
                                sEcho = param.sEcho,
                                iTotalRecords = FileHistoryModel.recordDetails.totalRecords,
                                iTotalDisplayRecords = FileHistoryModel.recordDetails.totalDisplayRecords,
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
        /// This method is to get the TrainingStaff data as string array to bind into datatbale
        /// </summary>
        /// <param name="TrainingStaffEntry">TrainingStaff list</param>
        /// <returns></returns>
        public static List<string[]> FileHistoryData(IEnumerable<FileUploadHistorys> FileHistoryEntry, int currentUserId)
        {
            return FileHistoryEntry.Select(entry => new string[]
            {  
                entry.fileName,
                entry.totalRecords.ToString(), 
                entry.userName,
                entry.createdDate.ToString("dd/MM/yyyy"),
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
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.FileHistory.Views.RenderAction, _accessRight)
                });
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        #endregion

        [HttpPost]
        [Authorize]
        public virtual JsonResult Edit(string id, bool readOnly)
        {
            bool istraining = readOnly;
            AccessRightsModel _accessRight = new AccessRightsModel();
            FileHistoryViewModel _model = new FileHistoryViewModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    int newId = 0;
                    newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    _model.readOnly = true;
                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        if (svc != null)
                        {
                            var FileHistory = svc.GetFileHistoryById(newId);

                            if (FileHistory != null)
                            {
                                _model.id = id;
                                _model.isEdit = false;
                                _model.fileName = FileHistory.fileName;
                                _model.totalRecords = FileHistory.totalRecords;
                                _model.createdBy = FileHistory.createdBy;
                                _model.createdDate = FileHistory.createdDate;
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
            if (istraining)
            {
                return Json(new
                {
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.FileHistory.Views.Edit, _model)
                });
            }
            else
            {
                return Json(new
                {
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.FileHistory.Views.EditAttendance, _model)
                });
            }

        }

        [HttpPost]
        [Authorize]
        public virtual ActionResult Delete(string id)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                int fileId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView && _accessRight.canDelete)
                    {
                        if (svc != null)
                        {
                            bool isSuccess = svc.DeleteFileHistory(this.CurrentUserId, fileId, _accessRight.pageName);
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

        [HttpPost]
        public virtual JsonResult UploadTrainingFile()
        {
            if (ModelState.IsValid)
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                try
                {
                    _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                    if (_accessRight != null)
                    {
                        if (!_accessRight.canEdit)
                        {
                            return Json(new
                            {
                                status = Common.Status.Denied.ToString(),
                                message = Resources.NO_ACCESS_RIGHTS_EDIT
                            });
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
                            DataSet ds = new DataSet();
                            List<TrainingStaffs> lstModel = new List<TrainingStaffs>();
                            FileUploadHistorys objHistory = new FileUploadHistorys();

                            if (Request.Files["file"].ContentLength > 0)
                            {


                                //objHistory.userName = this.UserName;

                                string fileExtension =
                                                     System.IO.Path.GetExtension(Request.Files["file"].FileName);

                                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                                {
                                    string fileLocation = Server.MapPath("~/Content/") + Request.Files["file"].FileName;
                                    if (System.IO.File.Exists(fileLocation))
                                    {

                                        System.IO.File.Delete(fileLocation);
                                    }
                                    Request.Files["file"].SaveAs(fileLocation);
                                    string excelConnectionString = string.Empty;
                                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                                    //connection String for xls file format.
                                    if (fileExtension == ".xls")
                                    {
                                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                                    }
                                    //connection String for xlsx file format.
                                    else if (fileExtension == ".xlsx")
                                    {

                                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                                    }
                                    //Create Connection to Excel work book and add oledb namespace
                                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                                    excelConnection.Open();
                                    DataTable dt = new DataTable();

                                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    if (dt == null)
                                    {
                                        return null;
                                    }

                                    String[] excelSheets = new String[dt.Rows.Count];
                                    int t = 0;
                                    //excel data saves in temp file here.
                                    foreach (DataRow row in dt.Rows)
                                    {
                                        excelSheets[t] = row["TABLE_NAME"].ToString();
                                        t++;
                                    }
                                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                                    {
                                        dataAdapter.Fill(ds);
                                    }
                                }
                                DataTable dtValues = ds.Tables[0];
                                objHistory.createdBy = this.UserName;
                                objHistory.createdDate = DateTime.Now;
                                objHistory.fileName = Request.Files["file"].FileName + " " + DateTime.Now.ToString("yyyyMMdd");
                                objHistory.uploadType = 1;
                                objHistory.user_Id = this.CurrentUserId;
                                objHistory.totalRecords = Convert.ToInt32(dtValues.Rows[dtValues.Rows.Count - 1][3]);
                                for (int i = 0; i < dtValues.Rows.Count; i++)
                                {
                                    if (!string.IsNullOrEmpty(dtValues.Rows[i]["SALARY ID"].ToString()))
                                    {
                                        TrainingStaffs objModel = new TrainingStaffs();
                                        objModel.createdBy = this.UserName;
                                        objModel.createdDate = DateTime.Now;
                                        objModel.name = !string.IsNullOrEmpty(dtValues.Rows[i]["NAME"].ToString()) ? dtValues.Rows[i]["NAME"].ToString() : "";
                                        objModel.salaryId = !string.IsNullOrEmpty(dtValues.Rows[i]["SALARY ID"].ToString()) ? Convert.ToInt32(dtValues.Rows[i]["SALARY ID"]) : 0;
                                        objModel.businessevent = !string.IsNullOrEmpty(dtValues.Rows[i]["BUSINESS_EVENT"].ToString()) ? dtValues.Rows[i]["BUSINESS_EVENT"].ToString() : "";
                                        if (!string.IsNullOrEmpty(dtValues.Rows[i]["START DATE"].ToString()))
                                        {
                                            string date = dtValues.Rows[i]["START DATE"].ToString();
                                            string[] dates = date.Split('.');
                                            DateTime dt = new DateTime(Convert.ToInt32(dates[2]), Convert.ToInt32(dates[1]), Convert.ToInt32(dates[0]));
                                            objModel.startDate = dt;
                                        }
                                        else
                                        {
                                            objModel.startDate = null;
                                        }
                                        if (!string.IsNullOrEmpty(dtValues.Rows[i]["END DATE"].ToString()))
                                        {
                                            string date = dtValues.Rows[i]["END DATE"].ToString();
                                            string[] dates = date.Split('.');
                                            DateTime dt = new DateTime(Convert.ToInt32(dates[2]), Convert.ToInt32(dates[1]), Convert.ToInt32(dates[0]));
                                            objModel.endDate = dt;
                                        }
                                        else
                                        {
                                            objModel.endDate = null;
                                        }
                                        objModel.subarea = !string.IsNullOrEmpty(dtValues.Rows[i][6].ToString()) ? dtValues.Rows[i][6].ToString() : null;
                                        objModel.costCenterId = !string.IsNullOrEmpty(dtValues.Rows[i]["COST CENTER"].ToString()) ? Convert.ToInt32(dtValues.Rows[i]["COST CENTER"]) : 0;
                                        objModel.costCenterText = !string.IsNullOrEmpty(dtValues.Rows[i]["COST CENTER TEXT"].ToString()) ? dtValues.Rows[i]["COST CENTER TEXT"].ToString() : null;
                                        objModel.location = !string.IsNullOrEmpty(dtValues.Rows[i]["LOCATION"].ToString()) ? dtValues.Rows[i]["LOCATION"].ToString() : null;
                                        objModel.organizer = !string.IsNullOrEmpty(dtValues.Rows[i]["ORGANIZER"].ToString()) ? dtValues.Rows[i]["ORGANIZER"].ToString() : null;
                                        objModel.days = !string.IsNullOrEmpty(dtValues.Rows[i][12].ToString()) ? Convert.ToDecimal(dtValues.Rows[i][12]) : 0;
                                        objModel.hours = !string.IsNullOrEmpty(dtValues.Rows[i][11].ToString()) ? Convert.ToDecimal(dtValues.Rows[i][11]) : 0;
                                        lstModel.Add(objModel);
                                    }

                                }
                            }
                            var save = svcTrainingStaff.SaveTrainingStaff(this.CurrentUserId, lstModel, objHistory, _accessRight.pageName);

                            if (save != null)
                            {
                                return Json(new
                                {
                                    status = Common.Status.Success.ToString(),
                                    message = Resources.MSG_UPDATE
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
                        else
                        {
                            return Json(new
                            {

                                status = Common.Status.Warning.ToString(),
                                message = Resources.RECORD_EXISTS
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
                    //}
                    //else
                    //{
                    //    return Json(new
                    //    {
                    //        status = Common.Status.Denied.ToString(),
                    //        message = Resources.NO_ACCESS_RIGHTS
                    //    });
                    //}
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
    }
}
