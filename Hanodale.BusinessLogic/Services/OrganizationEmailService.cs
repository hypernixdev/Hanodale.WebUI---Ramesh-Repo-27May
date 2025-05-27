using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic; 

namespace Hanodale.BusinessLogic
{
    public class OrganizationEmailService : IOrganizationEmailService
    {
        #region OrganizationEmail

        public Hanodale.DataAccessLayer.Interfaces.IOrganizationEmailService DataProvider;

        public OrganizationEmailService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.OrganizationEmailService();
        }

        public OrganizationEmailDetails GetOrganizationEmail(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            if (string.IsNullOrEmpty(search))
                return this.DataProvider.GetOrganizationEmail(currentUserId, userId, startIndex, pageSize);
            else
                return this.DataProvider.GetOrganizationEmailBySearch(currentUserId, userId, startIndex, pageSize, search);
        }

        public OrganizationEmails SaveOrganizationEmail(int currentUserId, OrganizationEmails entity, string pageName)
        {
            if (entity.id > 0)
                return this.DataProvider.UpdateOrganizationEmail(currentUserId, entity, pageName);
            else
                return this.DataProvider.CreateOrganizationEmail(currentUserId, entity, pageName);
        }

        public bool DeleteOrganizationEmail(int currentUserId, int id, string pageName)
        {
            return this.DataProvider.DeleteOrganizationEmail(currentUserId, id, pageName);
        }

        public OrganizationEmails GetOrganizationEmailById(int id)
        {
            return this.DataProvider.GetOrganizationEmailById(id);
        }

        public bool IsOrganizationEmailExists(OrganizationEmails entity)
        {
            return this.DataProvider.IsOrganizationEmailExists(entity);
        }

        public List<OrganizationEmails> GetListOrganizationEmail()
        {
            return this.DataProvider.GetListOrganizationEmail();
        }
        #endregion

       
    }
}
