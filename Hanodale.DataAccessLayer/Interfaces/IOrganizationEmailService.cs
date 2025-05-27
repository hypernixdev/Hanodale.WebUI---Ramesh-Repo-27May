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

namespace Hanodale.DataAccessLayer.Interfaces
{
    public interface IOrganizationEmailService
    {
        #region OrganizationEmail

        OrganizationEmailDetails GetOrganizationEmailBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search);

        OrganizationEmailDetails GetOrganizationEmail(int currentUserId, int userId, int startIndex, int pageSize);

        OrganizationEmails CreateOrganizationEmail(int currentUserId, OrganizationEmails entity, string pageName);

        OrganizationEmails UpdateOrganizationEmail(int currentUserId, OrganizationEmails entity, string pageName);

        bool DeleteOrganizationEmail(int currentUserId, int id, string pageName);

        OrganizationEmails GetOrganizationEmailById(int id);

        List<OrganizationEmails> GetListOrganizationEmail();

        bool IsOrganizationEmailExists(OrganizationEmails entity);

        #endregion
    }
}
