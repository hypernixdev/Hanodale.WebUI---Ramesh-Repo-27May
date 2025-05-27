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
    //[Authorize]
    public partial class CommonController : AuthorizedController
    {
        #region Declaration
        const string PAGE_URL = "StockMaster/StockMaster";
        #endregion

        #region Constructor

        private readonly ICommonService svcCommon;

        public CommonController(ICommonService _commonService)
            
        {
            this.svcCommon = _commonService;
        }
        #endregion

        
        public virtual ActionResult GetCustomSearchPanel(int searchType)
        {
            var assetTypeId = 0;
            var obj = new SearchPanelModel();
            obj.searchType = searchType;
            if (searchType == 2)
            {
                obj.createdDateFrom = DateTime.Now.AddDays(-7);
                obj.createdDateTo = DateTime.Now;
            }
            else if (searchType == 10)
            {
                var _statusList = new List<ModuleItems>();
                _statusList.Add(new ModuleItems { id=0, name = "Active" });
                _statusList.Add(new ModuleItems { name = "InActive" });
                obj.lstStatus = _statusList.Select(p => new SelectListItem
                {
                    Text = p.name,
                    Value = p.name,
                    Selected = p.name == "Active",
                }).ToList();
            }
            else
            {
               
            }

            return PartialView(MVC.CustomSearchPanel.Views._SearchPanel1, obj);
        }

    }
}
