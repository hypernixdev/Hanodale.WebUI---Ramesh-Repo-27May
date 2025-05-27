using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;
using Hanodale.Domain.DTOs;
namespace Hanodale.BusinessLogic
{
   public class HelpDeskService:IHelpDeskService
   {
       #region HelpDeskService

       public Hanodale.DataAccessLayer.Interfaces.IHelpDeskService DataProvider;

       public HelpDeskService()
       {
           this.DataProvider = new Hanodale.DataAccessLayer.Services.HelpDeskService();
       }

       public HelpDeskDetails GetHelpDesk(int currentUserId, int organizationId, int startIndex, int pageSize, string search, object filterEntity)
       {
           //if (string.IsNullOrEmpty(search))
           //    return this.DataProvider.GetHelpDesk(currentUserId,organizationId,  startIndex, pageSize, search, filterEntity);
           //else
           //    return this.DataProvider.GetHelpDeskBySearch(currentUserId, organizationId,  startIndex, pageSize, search, filterEntity);
           return this.DataProvider.GetHelpDesk(currentUserId, organizationId, startIndex, pageSize, search, filterEntity);
       }

       public HelpDesks SaveHelpDesk(int currentUserId, HelpDesks helpDesksEn, string pageName)
       {
           if (helpDesksEn.id > 0)
               return this.DataProvider.UpdateHelpDesk(currentUserId, helpDesksEn, pageName);
           else
               return this.DataProvider.CreateHelpDesk(currentUserId, helpDesksEn, pageName);
       }

       public bool DeleteHelpDesk(int currentUserId, int helpDesksId, string pageName)
       {
           return this.DataProvider.DeleteHelpDesk(currentUserId, helpDesksId, pageName);
       }

       public HelpDesks GetHelpDeskById(int id)
       {
           return this.DataProvider.GetHelpDeskById(id);
       }

       public bool IsHelpDeskExists(HelpDesks helpDesks)
       {
           return this.DataProvider.IsHelpDeskExists(helpDesks);
       }
       public List<HelpDesks> GetListHelpDesk()
       {
           return this.DataProvider.GetListHelpDesk();
       }

       public List<Users> GetListUserByStatus()
       {
           return this.DataProvider.GetListUserByStatus();

       }

       public List<HelpDesks> GetListHelpDeskByOrganizationAndApproved(int subCostCenter)
       {
           return this.DataProvider.GetListHelpDeskByOrganizationAndApproved(subCostCenter);
       }

       public List<HelpDesks> UpdatedHelpDeskApproval(int currentUserId, List<HelpDesks> helpDeskEn, string pageName)
       {
           return this.DataProvider.UpdatedHelpDeskApproval(currentUserId, helpDeskEn, pageName);
       }

       public ChartPanelInfo GetChartPanelInfo(ChartPanelInfo entity)
       {
           return this.DataProvider.GetChartPanelInfo(entity);
       }

       public List<StackingBarChartPanelTicketInfo> GetStackingBarChartPanelTicketInfo(StackingBarChartPanelTicketInfo entity)
       {
           return this.DataProvider.GetStackingBarChartPanelTicketInfo(entity);
       }

       public ChartPanelTicketDetails GetChartPanelDetails(int currentUserId, int userId, int organization_Id, int startIndex, int pageSize, string search, object filterEntity)
       {
           return this.DataProvider.GetChartPanelDetails(currentUserId, userId, organization_Id, startIndex, pageSize, search, filterEntity);
       }

   

       #endregion
   }
}
