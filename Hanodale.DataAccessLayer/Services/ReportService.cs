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
    public class ReportService : IReportService
    {
        #region Reports

        /// <summary>
        /// This method is to get report by user
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>report category list</returns>
        public List<Reports> GetReportByUser(int userId)
        {
            List<Reports> _lstRpt = new List<Reports>();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var lst = model.ReportCategories.Include("ReportCategory1").Where(p => p.parent_Id == null && p.visibility);
                    foreach (var item in lst)
                    {
                        var root = new Reports();
                        root.id = item.id;
                        root.parent_Id = item.parent_Id;
                        root.name = item.name;
                        root.description = item.description;
                        root.backColor = item.backColor;
                        root.fontColor = item.fontColor;
                        root.icon = item.icon;
                        root.ordering = item.ordering;
                        root.visibility = item.visibility;

                        root.ChildList = new List<Reports>();
                        foreach (var child in item.ReportCategory1)
                        {
                            var obj = new Reports();
                            obj.id = child.id;
                            obj.parent_Id = child.parent_Id;
                            obj.name = child.name;
                            obj.description = child.description;
                            obj.backColor = child.backColor;
                            obj.fontColor = child.fontColor;
                            obj.icon = child.icon;
                            obj.ordering = child.ordering;
                            obj.visibility = child.visibility;

                            root.ChildList.Add(obj);
                        }

                        _lstRpt.Add(root);

                    }

                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _lstRpt;
        }

        #endregion
    }
}
