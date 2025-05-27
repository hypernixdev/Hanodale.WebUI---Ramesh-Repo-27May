using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IOrganizationEmailService
    {
        #region OrganizationEmail

        OrganizationEmailDetails GetOrganizationEmail(int currentUserId, int userId, int startIndex, int pageSize, string search);

        OrganizationEmails SaveOrganizationEmail(int currentUserId, OrganizationEmails entity, string pageName);

        bool DeleteOrganizationEmail(int currentUserId, int id, string pageName);

        OrganizationEmails GetOrganizationEmailById(int id);

        List<OrganizationEmails> GetListOrganizationEmail();

        bool IsOrganizationEmailExists(OrganizationEmails entity);

        #endregion
    }
}
