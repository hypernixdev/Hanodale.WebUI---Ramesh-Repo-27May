using System.Security.Principal;
using Hanodale.WebUI.Models;

namespace Hanodale.WebUI
{
    public static class PrincipalExtensions
    {
        public static HanodaleIdentity PMSIdentity(this IPrincipal principal)
        {
            return (HanodaleIdentity)principal.Identity;
        }
    }
}