using SaltStackers.Application.ViewModels.Membership;
using SaltStackers.Web.Models;
using System.Collections.Generic;

namespace SaltStackers.Web.Helpers
{
    public interface IUtilities
    {
        List<ApplicationPage> GetApplicationPages();

        string DataBaseRoleValidationGuid();

        List<Permission> GetPermissionsByMethods(List<ApplicationPage> applicationPages);

        void InitialAdminClaims();
    }
}
