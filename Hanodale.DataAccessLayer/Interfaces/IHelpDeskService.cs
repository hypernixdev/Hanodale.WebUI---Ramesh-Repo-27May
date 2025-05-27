using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.Domain.DTOs;
using Hanodale.Entity;
namespace Hanodale.DataAccessLayer.Interfaces
{
   public interface IHelpDeskService
    {
        #region HelpDesk

       HelpDeskDetails GetHelpDeskBySearch(int currentUserId, int organizationId, int startIndex, int pageSize, string search, object filterEntity);

       HelpDeskDetails GetHelpDesk(int currentUserId, int organizationId, int startIndex, int pageSize, string search, object filterEntity);

        HelpDesks CreateHelpDesk(int currentUserId, HelpDesks helpDeskEn, string pageName);

        HelpDesks UpdateHelpDesk(int currentUserId, HelpDesks helpDeskEn, string pageName);

        bool DeleteHelpDesk(int currentUserId, int helpDeskId, string pageName);

        HelpDesks GetHelpDeskById(int id);

        bool IsHelpDeskExists(HelpDesks helpDesk);

        List<HelpDesks> GetListHelpDesk();

        List<Users> GetListUserByStatus();

        List<HelpDesks> GetListHelpDeskByOrganizationAndApproved(int subCostCenter);

        List<HelpDesks> UpdatedHelpDeskApproval(int currentUserId, List<HelpDesks> helpDeskEn, string pageName);

        ChartPanelInfo GetChartPanelInfo(ChartPanelInfo entity);

        List<StackingBarChartPanelTicketInfo> GetStackingBarChartPanelTicketInfo(StackingBarChartPanelTicketInfo entity);

        ChartPanelTicketDetails GetChartPanelDetails(int currentUserId, int userId, int organization_Id, int startIndex, int pageSize, string search, object filterEntity);

        
        #endregion
    }
}
