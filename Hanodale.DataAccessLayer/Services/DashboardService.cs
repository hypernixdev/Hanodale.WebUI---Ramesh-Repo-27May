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

namespace Hanodale.DataAccessLayer.Services
{
    public class DashboardService : IDashboardService
    {
        #region Dashboard

        public Dashboards GetDashoardByUser(int userId,int subCostCenter)
        {
            var res = new Dashboards();
            return res;
            /*
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    res.itemList = new List<DashboardItems>();

                    
                    var ticketCount = new System.Data.Objects.ObjectParameter("ticketCount", typeof(int));
                    var lst = model.DB_User_Dashboard_Items(userId, subCostCenter, ticketCount);

                    res.ticketCount = Convert.ToInt32(ticketCount.Value);

                    foreach (var item in lst)
                    {
                        var root = new DashboardItems();
                        root.id = item.id;
                        root.parent_Id = item.parent_Id;
                        root.name = item.name;
                        root.description = item.description;
                        root.backColor = item.backColor;
                        root.fontColor = item.fontColor;
                        root.icon = item.icon;
                        root.ordering = item.ordering;
                        root.pageURL = item.url;
                        root.newCount = item.newRecordCount;
                        
                        res.itemList.Add(root);

                    }


                    //res.ticketCount = model.HelpDesks.Count(a => a.organization_Id == subCostCenter && a.workFollowStatus_Id == 6547);
                    //var lst = model.DashboardCategories.Include("DashboardCategory1").Where(p => p.parent_Id == null && p.visibility && p.MenuItem.UserRights.Any(s => s.UserRole.Users.Any(u => u.id == userId) && s.canView)).OrderBy(p => p.ordering);
                    //foreach (var item in lst)
                    //{
                    //    var root = new DashboardItems();
                    //    root.id = item.id;
                    //    root.parent_Id = item.parent_Id;
                    //    root.name = item.name;
                    //    root.description = item.description;
                    //    root.backColor = item.backColor;
                    //    root.fontColor = item.fontColor;
                    //    root.icon = item.icon;
                    //    root.ordering = item.ordering;
                    //    root.pageURL = item.url;
  

                    //    root.ChildList = new List<DashboardItems>();
                    //    var collection = item.DashboardCategory1.OrderBy(p => p.ordering);
                    //    foreach (var child in collection)
                    //    {
                    //        var obj = new DashboardItems();
                    //        obj.id = child.id;
                    //        obj.parent_Id = child.parent_Id;
                    //        obj.name = child.name;
                    //        obj.description = child.description;
                    //        obj.backColor = child.backColor;
                    //        obj.fontColor = child.fontColor;
                    //        obj.icon = child.icon;
                    //        obj.ordering = child.ordering;
                    //        obj.pageURL = item.url;
                    //        root.ChildList.Add(obj);
                    //    }

                    //    res.itemList.Add(root);

                    //}
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return res; */
        }

        public OrderPaymentTotals GetOrderPaymentTotals(DateTime startDate, DateTime endDate)
        {
            OrderPaymentTotals total = new OrderPaymentTotals();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // Adjust endDate to include the full end date (set to end of the day)
                    endDate = endDate.Date.AddDays(1).AddMilliseconds(-1); // Add one day, then subtract one millisecond

                    total.TotalSales = model.OrderPayment
                        .Where(op => (op.IsRefund == null || op.IsRefund == false)
                                    && op.paymentDate >= startDate.Date
                                    && op.paymentDate <= endDate)
                        .Sum(op => (decimal?)op.amount) ?? 0;

                    total.TotalRefund = model.OrderPayment
                        .Where(op => op.IsRefund == true
                                    && op.paymentDate >= startDate.Date
                                    && op.paymentDate <= endDate)
                        .Sum(op => (decimal?)op.amount) ?? 0;

                    return total;
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }
        }




        public List<HelpDesks> GetNewTickets(int userID, int organizationId)
        {
            List<HelpDesks> _list = new List<HelpDesks>();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.HelpDesks.Where(a => a.organization_Id == organizationId && a.workFollowStatus_Id == 6547).Select(p => new HelpDesks
                    {
                        id = p.id,
                        createdDate = p.createdDate,
                        feedback = p.feedback,
                        name = p.name,
                    }).OrderByDescending(a=>a.createdDate).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        #endregion
    }
}
