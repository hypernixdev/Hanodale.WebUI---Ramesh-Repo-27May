using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
   public interface IHelpDeskService
    {
        #region HelpDesk

       HelpDeskDetails GetHelpDesk(int currentUserId, int organizationId, int startIndex, int pageSize, string search, object filterEntity);

        HelpDesks SaveHelpDesk(int currentUserId, HelpDesks helpDesksEn, string pageName);

        bool DeleteHelpDesk(int currentUserId, int helpDesksId, string pageName);

        HelpDesks GetHelpDeskById(int id);

        bool IsHelpDeskExists(HelpDesks helpDesks);

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
