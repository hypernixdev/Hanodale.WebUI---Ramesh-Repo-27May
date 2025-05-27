using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.Models
{
    public class User
    {
        /// <summary>
        /// Gets or sets the identifier for the user.
        /// </summary>
        public int UserId { get; set; }


        /// <summary>
        /// Gets or sets the user's display name.
        /// </summary>

        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the authorization identifier for the user.
        /// </summary>

        public string AuthorizationId { get; set; }

        /// <summary>
        /// Gets or sets the country for the user.
        /// </summary>

        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the country for the user.
        /// </summary>

        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has completed or dismissed their profile registration.
        /// </summary>
        public bool HasRegistered { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has completed or dismissed their profile registration.
        /// </summary>
        public int Language { get; set; }


        public int SubCostCenter { get; set; }

        /// <summary>
        /// Gets or sets the country for the user.
        /// </summary>
        public string SessionId { get; set; }

        public string GMT { get; set; }
    }

    public class UserRoles
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
