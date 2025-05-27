using System;
using System.Security.Principal;
using System.Web.Security;
using Hanodale.Domain.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Hanodale.WebUI.Models
{
    [Serializable]
    public class HanodaleIdentity : IIdentity
    {
        public HanodaleIdentity(string name, string displayName, int userId, string sessionId, int companyId, int subCostCenter)
        {
            this.Name = name;
            this.DisplayName = displayName;
            this.UserId = userId;
            this.SessionId = sessionId;
            this.CompanyId = companyId;
            this.SubCostCenter = subCostCenter;

        }

        public HanodaleIdentity(string name, UserInfo userInfo)
            : this(name, userInfo.DisplayName, userInfo.UserId,userInfo.SessionId,userInfo.CompanyId,userInfo.SubCostCenter)
        {
            if (userInfo == null) throw new ArgumentNullException("userInfo");
            this.UserId = userInfo.UserId;
            this.CompanyId = userInfo.CompanyId;
            this.SubCostCenter = userInfo.SubCostCenter;
        }

        public HanodaleIdentity(FormsAuthenticationTicket ticket)
            : this(ticket.Name, UserInfo.FromString(ticket.UserData))
        {
            if (ticket == null) throw new ArgumentNullException("ticket");
        }

        public string Name { get; private set; }

        public string AuthenticationType
        {
            get { return "WEB"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string DisplayName { get; private set; }

        public int UserId { get; private set; }

        public int CompanyId { get; private set; }

        public int SubCostCenter { get; private set; }

        public string SessionId { get; private set; }
    }
}