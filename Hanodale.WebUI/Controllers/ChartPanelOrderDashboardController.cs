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
    public partial class ChartPanelOrderDashboardController : AuthorizedController
    {
        #region Declaration
        const string PAGE_URL = "Order/Index";
        #endregion

        #region Constructor

        private readonly IOrderService svc; private readonly ICommonService svcCommon;
        private readonly IDashboardService svcDashboard;
        public ChartPanelOrderDashboardController(IOrderService _bLService, ICommonService _commonService, IDashboardService _DLService)
        {
            this.svc = _bLService;
            this.svcCommon = _commonService;
            this.svcDashboard = _DLService;
        }
        #endregion

        [AppAuthorize]
        public virtual ActionResult Index()
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                _accessRight.organizationId = this.SubCostCenter;
                var model = new ChartPanelOrderDashboardModel();
                model.lstChartType = new List<SelectListItem>();
                model.lstChartType.Add(new SelectListItem { Text = Common.ChartType.Pie.ToString(), Value = Common.ChartType.Pie.ToString(), Selected = true });
                //model.lstChartType.Add(new SelectListItem { Text = Common.ChartType.Bar.ToString(), Value = Common.ChartType.Bar.ToString() });
                model.chartType_Id = Common.ChartType.Pie.ToString();
                if (_accessRight != null)
                {
                    model.organizationId = this.SubCostCenter;
                    //if (_accessRight.canView)
                    //{
                    return PartialView(MVC.ChartPanelOrderDashboard.Views.Index, model);
                    //}
                }

                return Json(new
                {
                    viewMarkup = ""
                }); ;
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        public virtual ActionResult DrawChartDetail(string startDate, string endDate, string chartType_Id, string section)
        {
            try
            {
                string currentUserStr = this.CurrentUserId.ToString();
                var chartType = chartType_Id;


                //if (!string.IsNullOrEmpty(chartType))
                //    chartType = chartType.ToLower();
                var model = new ChartPanelOrderDashboardModel();
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


                bool isBarChart = false;
                if (chartType == Common.ChartType.Bar.ToString())
                {
                    isBarChart = true;
                    model.lstBarChart = new List<BarChartDashboardModel>();

                    model.title = Resources.PAGE_HEADER_CHART_PANEL_ORDER_DASHBOARD_ORDER;

                    var lst = new List<BarChartDashboardModel>();

                    model.lstBarChart.Add(new BarChartDashboardModel { sectionType = Resources.PAGE_HEADER_CHART_PANEL_ORDER_DASHBOARD_ORDER_STATUS, sortId = 1, type = Common.DashboardSectionType.Order.ToString() });
                }
                else
                {
                    model.lstPie = new List<PieChartModel>();

                    model.title = Resources.PAGE_HEADER_CHART_PANEL_ORDER_DASHBOARD_ORDER;

                    model.lstPie.Add(new PieChartModel { title = Resources.PAGE_HEADER_CHART_PANEL_ORDER_DASHBOARD_ORDER_STATUS, type = Common.DashboardSectionType.Order.ToString() });

                    // Calculate the total count of all items
                    int totalCount = model.lstPie.Sum(item => item.count);

                    
                }


                model.loadedDateFromString = model.loadedDateFrom.ToShortDateString();
                model.loadedDateToString = model.loadedDateTo.ToShortDateString();

                model.lastUpdatedDate = DateTime.Now;
                model.lastUpdatedDateString = DateTime.Now.Date.ToShortDateString();
                model.lastUpdatedTimeString = DateTime.Now.ToShortTimeString();

                if (isBarChart)
                {
                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, MVC.ChartPanelOrderDashboard.Views._BarChart, model)
                    });
                }
                else
                {
                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, MVC.ChartPanelOrderDashboard.Views._PieChart, model)
                    });
                }

            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        [Authorize]
        public virtual JsonResult GetPieChartInfo(string startDate, string endDate, string chartType_Id = null)
        {
            try
            {

                var pie = new ChartPanelInfo();
                pie.typeList = new List<string>();

                if (!string.IsNullOrEmpty(startDate))
                    pie.loadDateFrom = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);

                if (!string.IsNullOrEmpty(endDate))
                    pie.loadDateTo = DateTime.ParseExact(endDate, "dd/MM/yyyy", null).AddDays(1).AddSeconds(-1);

                var lst = new List<PieChartModel>();

                pie.typeList.Add(Common.DashboardSectionType.Order.ToString());

                pie.organizationId = this.SubCostCenter;
                pie.sectionType = Common.GetEnumDescription(Common.DashboardSectionType.Order);
                var barResult = this.svc.GetChartPanelInfo(pie);
                if (barResult != null)
                {
                    foreach (var group in barResult)
                    {
                        var obj = new PieChartModel();
                        obj.type = group.sectionType;
                        obj.listItems = new List<PieChartItemModel>();
                        foreach (var item in group.chartItems)
                        {
                            var subItem = new PieChartItemModel();
                            subItem.type = item.type;
                            subItem.value = item.value;

                            obj.listItems.Add(subItem);
                        }

                        lst.Add(obj);
                    }
                }

                return Json(new
                {
                    list = lst
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        [Authorize]
        public virtual JsonResult GetBarChartInfo(string startDate, string endDate, string chartType_Id = null)
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight == null || _accessRight.canView)
                {
                    return Json(null);
                }


                var pie = new BarChartPanelOrderInfo();
                if (!string.IsNullOrEmpty(startDate))
                    pie.loadDateFrom = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (!string.IsNullOrEmpty(endDate))
                    pie.loadDateTo = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);

                var lst = new List<BarChartDashboardModel>();

                pie.typeList = new List<string>();
                if (string.IsNullOrEmpty(chartType_Id))
                {
                    pie.typeList.Add(Common.DashboardSectionType.Order.ToString());
                }
                else
                {
                    pie.typeList.Add(Common.Decrypt(this.CurrentUserId.ToString(), chartType_Id));
                }

                pie.organizationId = this.SubCostCenter;
                pie.sectionType = Common.GetEnumDescription(Common.DashboardSectionType.Order);
                var barResult = this.svc.GetBarChartPanelOrderInfo(pie);
                if (barResult != null && barResult.barChartPanelOrderList != null)
                {
                    foreach (var obj in barResult.barChartPanelOrderList)
                    {
                        var subItem = new BarChartDashboardModel();
                        subItem.sectionType = obj.sectionType;
                        subItem.lstItem = obj.lstItem.Select(p => new BarChartDashboardSubItemModel { categoryName = p.categoryName, count = p.count }).ToList();

                        lst.Add(subItem);
                    }
                }

                return Json(new
                {
                    list = lst
                });

            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        [HttpPost]
        public virtual JsonResult GetSalesSummary(DateTime dateFrom, DateTime dateTo)
        {
            // Ensure dateFrom starts at 00:00:00 AM
            dateFrom = dateFrom.Date; // This sets the time to 00:00:00

            // Ensure dateTo ends at 11:59:59 PM
            dateTo = dateTo.Date.AddDays(1).AddTicks(-1); // Moves to the next day and subtracts one tick (1 tick = 100 nanoseconds)

            var salesSummary = this.svc.GetSalesSummary(dateFrom, dateTo);

            if (salesSummary != null)
            {
                return Json(new
                {
                    success = true,
                    data = salesSummary
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Sales Summary not found." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public virtual JsonResult GetSalesCount(DateTime dateFrom, DateTime dateTo)
        {
            var salesSummary = this.svcDashboard.GetOrderPaymentTotals(dateFrom, dateTo);

            if (salesSummary != null)
            {
                return Json(new
                {
                    success = true,
                    data = salesSummary
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Sales Summary not found." }, JsonRequestBehavior.AllowGet);
            }
        }


        //[Authorize]
        //public virtual JsonResult GetBarChartInfo(string startDate, string endDate, string chartType_Id = null)
        //{
        //    try
        //    {
        //        var bar = new BarChartPanelOrderInfo();
        //        bar.typeList = new List<string>();
        //        if (!string.IsNullOrEmpty(startDate))
        //            bar.loadDateFrom = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        //        if (!string.IsNullOrEmpty(endDate))
        //            bar.loadDateTo = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);

        //        var lst = new List<BarChartDashboardModel>();

        //        bar.typeList.Add(Common.DashboardSectionType.Order.ToString());

        //        bar.organizationId = this.SubCostCenter;
        //        bar.sectionType = Common.GetEnumDescription(Common.DashboardSectionType.Order);
        //        var barResult = this.svc.GetBarChartPanelOrderInfo(bar);
        //        if (barResult != null && barResult.barChartPanelList != null)
        //        {
        //            foreach (var obj in barResult.barChartPanelList)
        //            {
        //                var subItem = new BarChartDashboardModel();
        //                subItem.type = obj.categoryName;
        //                subItem.count = obj.count;

        //                lst.Add(subItem);
        //            }
        //        }

        //        return Json(new
        //        {
        //            list = lst
        //        });

        //    }
        //    catch (Exception err)
        //    {
        //        throw new ErrorException(err.Message);
        //    }
        //}
    }
}
