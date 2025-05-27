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
using System.Globalization;

namespace Hanodale.WebUI.Controllers
{
    public partial class ChartPanelTicketDashboardController : AuthorizedController
    {
        #region Declaration
        const string PAGE_URL = "HelpDesk/Index";
        #endregion

        #region Constructor

        private readonly IHelpDeskService svc; private readonly ICommonService svcCommon;

        public ChartPanelTicketDashboardController(IHelpDeskService _bLService, ICommonService _commonService)
        {
            this.svc = _bLService;
            this.svcCommon = _commonService;
        }
        #endregion


        [AppAuthorize]
        public virtual ActionResult Dashboard(string dashboardBoxName)
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                string currentUserStr = this.CurrentUserId.ToString();



                var model = new ChartPanelTicketDashboardModel();
                model.dashboardBoxName = dashboardBoxName;
                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        //var currentDate = DateTime.Now.Date;

                        //model.loadedDateFrom = new DateTime(currentDate.Year, currentDate.Month, 1);
                        //model.loadedDateTo = currentDate;

                        var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        model.loadedDateFrom = currentDate;
                        model.loadedDateTo = DateTime.Now;

                        var sectionType = Common.Encrypt(currentUserStr, Common.TicketChartDashboardSectionType.none.ToString());

                        model.section = sectionType;

                        var obj = new ChartPanelInfo();

                        obj.loadDateFrom = model.loadedDateFrom;
                        obj.loadDateTo = model.loadedDateTo;

                        var totalTickets = Common.TicketChartDashboardType.totalTickets.ToString();
                        var totalCRFSS = Common.TicketChartDashboardType.totalCRFSS.ToString();
                        var totalCompleted = Common.TicketChartDashboardType.totalCompleted.ToString();
                        var totalTicketOverdue = Common.TicketChartDashboardType.totalTicketOverdue.ToString();

                        obj.typeList = new List<string>();
                        obj.typeList.Add(totalTickets);
                        obj.typeList.Add(totalCRFSS);
                        obj.typeList.Add(totalCompleted);
                        obj.typeList.Add(totalTicketOverdue);
                        obj.organizationId = this.SubCostCenter;

                        var result = this.svc.GetChartPanelInfo(obj);
                        var lst = new List<DonutHoleChartItemModel>();
                        if (result != null)
                        {
                            foreach (var item in result.chartItems)
                            {
                                var entity = new DonutHoleChartItemModel();
                                entity.value = item.value;
                                entity.valueStr = item.value.ToString();
                                entity.valuePercentage = item.valuePercentage;
                                entity.type = Common.Encrypt(currentUserStr, item.type);
                                entity.chartType = item.type;
                                entity.backColor = item.backColor;
                                if (item.type == totalTickets)
                                {
                                    entity.sortId = 1;
                                    entity.title = Resources.PAGE_HEADER_CHART_PANEL_TICKET_DASHBOARD_TOTAL_TICKET;
                                }
                                else if (item.type == totalCRFSS)
                                {
                                    entity.sortId = 2;
                                    entity.title = Resources.PAGE_HEADER_CHART_PANEL_TICKET_DASHBOARD_TOTAL_CRFSS_TICKET;
                                }
                                else if (item.type == totalCompleted)
                                {
                                    entity.sortId = 3;
                                    entity.title = Resources.PAGE_HEADER_CHART_PANEL_TICKET_DASHBOARD_TOTAL_COMPLETED;
                                }
                                else if (item.type == totalTicketOverdue)
                                {
                                    entity.sortId = 4;
                                    entity.title = Resources.PAGE_HEADER_CHART_PANEL_TICKET_DASHBOARD_TOTAL_TICKET_OVERDUE;
                                }
                                
                                lst.Add(entity);
                            }
                        }

                        model.lstDonutHole = lst;

                        var ticketStatus = Common.TicketChartDashboardType.ticketStatus.ToString();
                        var totalTicketByCRFSS = Common.TicketChartDashboardType.totalTicketByCRFSS.ToString();

                        


                        var lst3 = new List<StackingBarChartDashboardModel>();
                        lst3.Add(new StackingBarChartDashboardModel { sectionType = sectionType, sortId = 1, type = Common.Encrypt(currentUserStr, ticketStatus), chartType = ticketStatus, title = Resources.PAGE_HEADER_CHART_PANEL_TICKET_DASHBOARD_TICKET_STATUS });
                        lst3.Add(new StackingBarChartDashboardModel { sectionType = sectionType, sortId = 2, type = Common.Encrypt(currentUserStr, totalTicketByCRFSS), chartType = totalTicketByCRFSS, title = Resources.PAGE_HEADER_CHART_PANEL_TICKET_DASHBOARD_TOTAL_TICKET_BY_CRFSS_ELEMENT });

                        model.lstStackingBarChart = lst3;

                        model.loadedDateFromString = model.loadedDateFrom.ToString("dd/MM/yyyy");
                        model.loadedDateToString = model.loadedDateTo.ToString("dd/MM/yyyy");

                        model.lastUpdatedDate = DateTime.Now;
                        model.lastUpdatedDateString = DateTime.Now.Date.ToString("dd/MM/yyyy");
                        model.lastUpdatedTimeString = DateTime.Now.ToString("hh:mm ttt");


                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.ChartPanelTicketDashboard.Views._TicketDashboard, model)
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

        [AppAuthorize]
        public virtual ActionResult Index()
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                _accessRight.organizationId = this.SubCostCenter;
                var model = new ChartPanelTicketDashboardModel();
                if (_accessRight != null)
                {
                    model.type = TempData["pieChartId"] as string;// Common.Encrypt(this.CurrentUserId.ToString(), "Priority"); 
                    model.organizationId = this.SubCostCenter;
                    if (_accessRight.canView)
                    {
                        return View(MVC.ChartPanelTicketDashboard.Views.Index, model);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        [HttpPost]
        [AppAuthorize]
        public virtual ActionResult DetailDashboard(string id, string dashboardName, string section)
        {
            try
            {
                //AccessRightsModel _accessRight = new AccessRightsModel();
                //_accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                //_accessRight.organizationId = this.SubCostCenter;
                ////_accessRight. = ViewBag.pieChartId;
                //// _user = svcUser.GetUserById(this.CurrentUserId, this.CurrentUserId);
                ////_accessRight.userRoleIds = _user.userRole_Id.ToString();
                var model = new ChartPanelTicketDashboardModel();
                //if (_accessRight != null)
                //{
                model.type = id; // TempData["pieChartId"] as string;
                model.section = section;
                model.organizationId = this.SubCostCenter;
                model.dashboardBoxName = dashboardName;
                //if (_accessRight.canView)
                //{
                return Json(new
                {
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.ChartPanelTicketDashboard.Views._TicketDetail, model)
                });
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        [HttpPost]
        [AppAuthorize]
        public virtual ActionResult ViewMoreDetail(string id, string dashboardName, string section)
        {
            try
            {
                //AccessRightsModel _accessRight = new AccessRightsModel();
                //_accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                //_accessRight.organizationId = this.SubCostCenter;
                ////_accessRight. = ViewBag.pieChartId;
                //// _user = svcUser.GetUserById(this.CurrentUserId, this.CurrentUserId);
                ////_accessRight.userRoleIds = _user.userRole_Id.ToString();
                var model = new ChartPanelTicketDashboardModel();
                //if (_accessRight != null)
                //{
                model.type = id; // TempData["pieChartId"] as string;
                model.section = section;
                model.organizationId = this.SubCostCenter;
                model.dashboardBoxName = dashboardName;
                //if (_accessRight.canView)
                //{
                return Json(new
                {
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.ChartPanelTicketDashboard.Views.TicketViewMoreDetail, model)
                });
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        [AppAuthorize]
        public virtual ActionResult GetCustomSearchPanel()
        {
            var obj = new ChartPanelTicketDashboardModel();
            var lst = new Dictionary<int, string>();
            lst.Add(1, Resources.LABEL_ITEM_LAST_MONTH);
            lst.Add(2, Resources.LABEL_ITEM_LAST_3_MONTHS);
            lst.Add(3, Resources.LABEL_ITEM_LAST_6_MONTHS);
            lst.Add(4, Resources.LABEL_ITEM_LAST_YEAR);
            lst.Add(5, Resources.LABEL_ITEM_CURRENT_MONTH);
            lst.Add(6, Resources.LABEL_ITEM_CURRENT_YEAR);
            lst.Add(0, Resources.LABEL_ITEM_OTHER_DATE);

            obj.lstFilterType = lst.Select(a => new SelectListItem
            {
                Text = a.Value,
                Value = a.Key.ToString(),
                Selected = (a.Key == 5)
            });

            //var currentDate = DateTime.Now.Date;
            //obj.loadedDateFrom = new DateTime(currentDate.Year, currentDate.Month, 1);
            //obj.loadedDateTo = currentDate;

            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            obj.loadedDateFrom = currentDate;
            obj.loadedDateTo = DateTime.Now;

            return PartialView(MVC.ChartPanelTicketDashboard.Views._SearchPanel, obj);
        }

        private DateTime CheckDateByFilterDate(int filterType_Id)
        {
            var date = DateTime.Now.Date;
            if (filterType_Id == 1)
            {
                return date.AddMonths(-1);
            }
            else if (filterType_Id == 2)
            {
                return date.AddMonths(-3);
            }
            else if (filterType_Id == 3)
            {
                return date.AddMonths(-6);
            }
            else if (filterType_Id == 4)
            {
                return date.AddYears(-1);
            }
            else if (filterType_Id == 5)
            {
                return date.AddDays(-date.Day + 1);
            }
            else if (filterType_Id == 6)
            {
                return date.AddMonths(-(date.Month + 1)).AddDays(-(date.Day + 1));
            }
            else
            {
                return date;
            }
        }

        [Authorize]
        public virtual ActionResult BindChartPanelTicketDashboard(DataTableModel param, string myKey, string recordId)
        {
            ChartPanelInfo filterModel = null;
            var idFilter0 = Convert.ToString(Request["sSearch_0"]);
            var idFilter1 = Convert.ToString(Request["sSearch_1"]);
            var idFilter2 = Convert.ToString(Request["sSearch_2"]);
            filterModel = new ChartPanelInfo();
            filterModel.organizationId = this.SubCostCenter;
            filterModel.sectionType = Common.GetEnumDescription(Common.DashboardSectionType.Ticket);

            //var currentDate = DateTime.Now.Date;
            //filterModel.loadDateFrom = new DateTime(currentDate.Year, currentDate.Month, 1);
            //filterModel.loadDateTo = currentDate;

            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            filterModel.loadDateFrom = currentDate;
            filterModel.loadDateTo = DateTime.Now;

            if (!string.IsNullOrEmpty(recordId))
            {
                filterModel.filterType = Common.Decrypt(this.CurrentUserId.ToString(), recordId);

            }
            if (!string.IsNullOrEmpty(idFilter0) && idFilter0 == "0")
            {
                if (!string.IsNullOrEmpty(idFilter1) || !string.IsNullOrEmpty(idFilter2))
                {
                    //filterModel.isDashboard = true;
                    if (!string.IsNullOrEmpty(idFilter1))
                        filterModel.loadDateFrom = DateTime.ParseExact(idFilter1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(idFilter2))
                        filterModel.loadDateTo = DateTime.ParseExact(idFilter2, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
                }
                else
                {
                    return Json(new
                    {
                        status = Common.Status.Error.ToString(),
                        message = Resources.MSG_ERR_INVALIDMODEL
                    });
                }
            }
            else
            {
                int filterType_Id = 0;
                int.TryParse(idFilter0, out filterType_Id);
                filterModel.loadDateFrom = CheckDateByFilterDate(filterType_Id);
                filterModel.loadDateTo = DateTime.Now.AddDays(1).Date.AddSeconds(-1);
            }

            IEnumerable<ChartPanelTicket> filteredTickets = null;
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

                        var ticketModel = this.svc.GetChartPanelDetails(this.CurrentUserId, 0, this.SubCostCenter, param.iDisplayStart, param.iDisplayLength, param.sSearch, filterModel);

                        if (svc != null)
                        {
                            //Sorting
                            var sortColumnIndex = param.iSortCol_0;
                            Func<ChartPanelTicket, string> orderingFunction = (c => sortColumnIndex == 0 ? c.code :
                                                            sortColumnIndex == 1 ? c.name :
                                                            sortColumnIndex == 2 ? c.feedback :
                                                            sortColumnIndex == 3 ? c.department : c.designation
                                                            );


                            //param.sSortDir_0 = sortColumnIndex >= 0 ? param.sSortDir_0 : "desc";

                            filteredTickets = ticketModel.lstTicket == null ? new List<ChartPanelTicket>() : ticketModel.lstTicket;
                            if (param.sSortDir_0 != null)
                            {
                                if (param.sSortDir_0 == "asc")
                                    filteredTickets = filteredTickets.OrderBy(orderingFunction);
                                else
                                    filteredTickets = filteredTickets.OrderByDescending(orderingFunction);
                            }

                            var result = ChartPanelTicketDashboardData(filteredTickets, this.CurrentUserId);
                            return Json(new
                            {
                                sEcho = param.sEcho,
                                iTotalRecords = ticketModel.recordDetails.totalRecords,
                                iTotalDisplayRecords = ticketModel.recordDetails.totalDisplayRecords,
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

        public static List<string[]> ChartPanelTicketDashboardData(IEnumerable<ChartPanelTicket> ticketEntry, int currentUserId)
        {
            return ticketEntry.Select(entry => new string[]
            {  
                entry.code,
                entry.name,
                entry.feedback,
                entry.createDateStr,
                entry.status,
                Common.Encrypt(currentUserId.ToString(), entry.id.ToString()),
            }).ToList();
        }

        [Authorize]
        [HttpPost] public virtual JsonResult RenderAction()
        {
            var _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                return Json(new
                {
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.ChartPanelTicketDashboard.Views._RenderAction, _accessRight)
                });
            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public virtual ActionResult DrawChartDetail(string startDate, string endDate, string type, string section)
        {
            try
            {
                string currentUserStr = this.CurrentUserId.ToString();
                var chartType = Common.Decrypt(currentUserStr, type);


                //if (!string.IsNullOrEmpty(chartType))
                //    chartType = chartType.ToLower();
                var model = new ChartPanelTicketDashboardModel();
                model.section = section;


                var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                if (!string.IsNullOrEmpty(startDate))
                {
                    if (!string.IsNullOrEmpty(startDate))
                        model.loadedDateFrom = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    model.loadedDateFrom = currentDate;// new DateTime(currentDate.Year, currentDate.Month, 1);
                }

                if (!string.IsNullOrEmpty(endDate))
                {
                    if (!string.IsNullOrEmpty(endDate))
                        model.loadedDateTo = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
                }
                else
                {
                    model.loadedDateTo = DateTime.Now;// currentDate;
                }


                var ticketStatus = Common.TicketChartDashboardType.ticketStatus.ToString();
                var totalTicket = Common.TicketChartDashboardType.totalTicketByCRFSS.ToString();

                bool isStackingBar = false;
                if (chartType == ticketStatus || chartType == totalTicket )
                {
                    isStackingBar = true;
                    model.lstStackingBarChart = new List<StackingBarChartDashboardModel>();

                    var title = Resources.PAGE_HEADER_CHART_PANEL_TICKET_DASHBOARD_TICKET_STATUS;
                    if (chartType == totalTicket)
                        title = Resources.PAGE_HEADER_CHART_PANEL_TICKET_DASHBOARD_TOTAL_TICKET_BY_CRFSS_ELEMENT;
                    
                    var lst = new List<StackingBarChartDashboardModel>();

                    lst.Add(new StackingBarChartDashboardModel { sectionType = section, sortId = 1, type = Common.Encrypt(currentUserStr, chartType), chartType = chartType, title = title });

                    model.lstStackingBarChart = lst;
                }
                else
                {
                    model.lstDonutHole = new List<DonutHoleChartItemModel>();

                    var obj = new ChartPanelInfo();
                    obj.typeList = new List<string>();
                    obj.typeList.Add(chartType);
                    obj.organizationId = this.SubCostCenter;

                    var sectionType = string.Empty;
                    if (!string.IsNullOrEmpty(section))
                    {
                        sectionType = Common.Decrypt(currentUserStr, section);
                    }

                    var totalTickets = Common.TicketChartDashboardType.totalTickets.ToString();
                    var totalCRFSS = Common.TicketChartDashboardType.totalCRFSS.ToString();
                    var totalCompleted = Common.TicketChartDashboardType.totalCompleted.ToString();
                    var totalTicketOverdue = Common.TicketChartDashboardType.totalTicketOverdue.ToString();

                    var result = this.svc.GetChartPanelInfo(obj);
                    var lst = new List<DonutHoleChartItemModel>();
                    if (result != null)
                    {
                        foreach (var item in result.chartItems)
                        {
                            var entity = new DonutHoleChartItemModel();
                            entity.value = item.value;
                            entity.valueStr = item.value.ToString();
                            entity.valuePercentage = item.valuePercentage;
                            entity.type = Common.Encrypt(currentUserStr, item.type);
                            entity.chartType = item.type;
                            entity.backColor = item.backColor;
                            if (item.type == totalTickets)
                            {
                                entity.sortId = 1;
                                entity.title = Resources.PAGE_HEADER_CHART_PANEL_TICKET_DASHBOARD_TOTAL_TICKET;
                            }
                            else if (item.type == totalCRFSS)
                            {
                                entity.sortId = 2;
                                entity.title = Resources.PAGE_HEADER_CHART_PANEL_TICKET_DASHBOARD_TOTAL_CRFSS_TICKET;
                            }
                            else if (item.type == totalCompleted)
                            {
                                entity.sortId = 3;
                                entity.title = Resources.PAGE_HEADER_CHART_PANEL_TICKET_DASHBOARD_TOTAL_COMPLETED;
                            }
                            else if (item.type == totalTicketOverdue)
                            {
                                entity.sortId = 4;
                                entity.title = Resources.PAGE_HEADER_CHART_PANEL_TICKET_DASHBOARD_TOTAL_TICKET_OVERDUE;
                            }

                            model.lstDonutHole.Add(entity);
                        }
                    }
                }


                model.loadedDateFromString = model.loadedDateFrom.ToShortDateString();
                model.loadedDateToString = model.loadedDateTo.ToShortDateString();

                model.lastUpdatedDate = DateTime.Now;
                model.lastUpdatedDateString = DateTime.Now.Date.ToShortDateString();
                model.lastUpdatedTimeString = DateTime.Now.ToShortTimeString();

                if (isStackingBar)
                {
                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, MVC.ChartPanelTicketDashboard.Views._StackingBarChart, model)
                    });
                }
                else
                {
                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, MVC.ChartPanelTicketDashboard.Views._DonutHoleChart, model)
                    });
                }

            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        [Authorize]
        public virtual JsonResult GetStackingBarChartInfo(string startDate, string endDate, string type)
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                var pie = new StackingBarChartPanelTicketInfo();
                if (!string.IsNullOrEmpty(startDate))
                    pie.loadDateFrom = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (!string.IsNullOrEmpty(endDate))
                    pie.loadDateTo = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);

                var lst = new List<StackingBarChartDashboardModel>();

                var ticketStatus = Common.TicketChartDashboardType.ticketStatus.ToString();
                var totalTicketByCRFSS = Common.TicketChartDashboardType.totalTicketByCRFSS.ToString();


                pie.typeList = new List<string>();
                if (string.IsNullOrEmpty(type))
                {
                    pie.typeList.Add(ticketStatus);
                    pie.typeList.Add(totalTicketByCRFSS);
                }
                else
                {
                    pie.typeList.Add(Common.Decrypt(this.CurrentUserId.ToString(), type));
                }

                pie.organizationId = this.SubCostCenter;
                pie.sectionType = Common.GetEnumDescription(Common.DashboardSectionType.Ticket);
                var barResult = this.svc.GetStackingBarChartPanelTicketInfo(pie);
                if (barResult != null)
                {
                    foreach (var obj in barResult)
                    {
                        var subItem = new StackingBarChartDashboardModel();
                        subItem.chartType = obj.filterType;
                        subItem.lstGroupedItems = new List<StackingBarChartDashboardSubItemModel>();
                        var filterType = obj.filterType;
                        foreach (var item in obj.stackingBarChartPanelList)
                        {
                            var s = new StackingBarChartDashboardSubItemModel();
                            s.categoryName = item.categoryName;
                            s.lstItems = item.subItems.Select(p => new StackingBarChartItemModel { FullDateName = p.DataType, Count = p.Count }).ToList();
                            subItem.lstGroupedItems.Add(s);
                        }

                        lst.Add(subItem);
                    }
                }
                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            list = lst
                        });
                    }
                    else
                    {
                        return Json(null);
                    }
                }
                else
                {
                    return Json(null);
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }
    }
}
