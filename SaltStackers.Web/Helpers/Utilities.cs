using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Membership;
using SaltStackers.Common.Helper;
using SaltStackers.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SaltStackers.Web.Helpers
{
    public class Utilities : IUtilities
    {
        private readonly IMembershipService _membershipService;
        private readonly IApplicationService _applicationService;
        private readonly IMemoryCache _memoryCache;

        public Utilities(IMembershipService membershipService,
            IApplicationService applicationService, IMemoryCache memoryCache)
        {
            _membershipService = membershipService;
            _applicationService = applicationService;
            _memoryCache = memoryCache;
        }

        private bool NeedPermission(MethodInfo method)
        {
            var authorizeAttribute = method.CustomAttributes.FirstOrDefault(p => p.AttributeType.Name == "AuthorizeAttribute");
            if (authorizeAttribute != null)
            {
                var policy = authorizeAttribute.NamedArguments.FirstOrDefault(p => p.MemberName == "Policy");
                if (policy.TypedValue.Value != null && policy.TypedValue.Value.ToString() == "DynamicPermission")
                {
                    return true;
                }
            }

            return false;
        }

        public List<ApplicationPage> GetApplicationPages()
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            var methods = asm.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)));

            var list = new List<ApplicationPage>();
            var projectName = Assembly.GetExecutingAssembly().FullName?.Split(',')[0];

            foreach (var method in methods)
            {

                if (NeedPermission(method))
                {
                    var areaName = method.DeclaringType?.Namespace?
                        .Replace(projectName + ".", "")
                            .Replace("Areas.", "")
                            .Replace(".Controllers", "")
                            .Replace("Controllers", "");

                    list.Add(new ApplicationPage()
                    {
                        ActionName = method.Name,
                        ControllerName = method.DeclaringType?.Name.Replace("Controller", ""),
                        AreaName = string.IsNullOrEmpty(areaName) ? "NoArea" : areaName
                    });
                }
            }

            return list.Distinct().ToList();
        }

        public string DataBaseRoleValidationGuid()
        {
            return _applicationService.GetSetting("RoleValidationKey");
        }

        private Permission GetPermissionsByMethod(ApplicationPage item)
        {
            var permission = new Permission
            {
                Group = $"{item.AreaName}.{item.ControllerName}",
                Name = $"{item.AreaName}|{item.ControllerName}|{item.ActionName}",
                Title = $"{item.AreaName} - {item.ControllerName} - {item.ActionName}"
            };

            var projectName = Assembly.GetExecutingAssembly().FullName?.Split(',')[0];
            var controllerType = Type.GetType($"{projectName}.Areas.{item.AreaName}.Controllers.{item.ControllerName}Controller");
            var method = controllerType?.GetMethods().FirstOrDefault(p => p.Name == item.ActionName);
            var breadcrumbAttribute = method?.CustomAttributes.FirstOrDefault(p => p.AttributeType.Name == "BreadCrumbAttribute");
            if (breadcrumbAttribute != null)
            {
                var resourceName = "";
                var titleResourceName = breadcrumbAttribute.NamedArguments.FirstOrDefault(p => p.MemberName == "TitleResourceName");
                if (titleResourceName.TypedValue.Value != null)
                {
                    resourceName = titleResourceName.TypedValue.Value.ToString();
                }

                var titleResourceType = breadcrumbAttribute.NamedArguments.FirstOrDefault(p => p.MemberName == "TitleResourceType");
                if (titleResourceType.TypedValue.Value != null)
                {
                    var resourceType = titleResourceType.TypedValue.Value as Type;
                    permission.Title = resourceName.GetResource(resourceType, true);
                }

                var order = breadcrumbAttribute.NamedArguments.FirstOrDefault(p => p.MemberName == "Order");
                if (order.TypedValue.Value != null)
                {
                    permission.Order = Convert.ToInt32(order.TypedValue.Value);
                }
            }

            return permission;
        }

        public List<Permission> GetPermissionsByMethods(List<ApplicationPage> applicationPages)
        {
            return applicationPages.Select(GetPermissionsByMethod).ToList();
        }

        public void InitialAdminClaims()
        {
            var permissions = GetPermissionsByMethods(_memoryCache.GetOrCreate("ApplicationPermissions", p =>
            {
                p.AbsoluteExpiration = DateTimeOffset.MaxValue;
                return GetApplicationPages();
            }));

            foreach (var permission in permissions)
            {
                permission.Selected = true;
            }

            var role = _membershipService.FindRoleByNameAsync("Administrator").Result;

            _membershipService.ManageRoleClaimsAsync(role.Id, permissions).Wait();
        }
    }
}
