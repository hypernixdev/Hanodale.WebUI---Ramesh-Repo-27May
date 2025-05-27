using Hanodale.Utility.Globalize;
using Hanodale.BusinessLogic;
using Hanodale.WebUI.Helpers;
using Hanodale.WebUI.Logging.Elmah;
using Hanodale.WebUI.Models;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Web.Mvc;
using Hanodale.Domain.DTOs;
using System.Linq;

namespace Hanodale.WebUI.Controllers
{
    [Authorize]
    public partial class ReportController : AuthorizedController
    {

        #region Declaration

        const string PAGE_URL = "Report/Reports";

        #endregion

        private readonly IReportService svc; private readonly ICommonService svcCommon;

        #region Initialize
        public ReportController(IReportService _bLService, ICommonService _commonService)
            
        {
            this.svc = _bLService; this.svcCommon = _commonService;
        }
        #endregion

        [Authorize]
        public virtual ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public virtual JsonResult Reports()
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        List<Reports> lst = svc.GetReportByUser(this.CurrentUserId);
                        var _model = lst.Select(p => new ReportModel
                        {
                            id = p.id,
                            parent_Id = p.parent_Id,
                            name = p.name,
                            description = p.description,
                            backColor = p.backColor,
                            fontColor = p.fontColor,
                            icon = p.icon,
                            ordering = p.ordering,
                            ChildList = p.ChildList.Where(a=>a.visibility).Select(c => new ReportModel
                            {
                                id = c.id,
                                parent_Id = c.parent_Id,
                                name = c.name,
                                description = c.description,
                                backColor = c.backColor,
                                fontColor = c.fontColor,
                                icon = c.icon,
                                ordering = c.ordering,
                            }).ToList()
                        });
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.Report.Views.Index, _model)
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
        public virtual JsonResult getReportContent(int ReportId)
        {
            {
                FileHistoryViewModel _model = new FileHistoryViewModel();
                _model.totalRecords = ReportId;
                _model.user_Id = this.CurrentUserId;
                _model.subCostId = this.SubCostCenter;

                return Json(_model);

            }
            //        //return Json(new
            //        //{
            //        //    viewMarkup = Common.RenderPartialViewToString(this, MVC.Report.Views.Report, _model)
            //        //});

        }

    }
}
