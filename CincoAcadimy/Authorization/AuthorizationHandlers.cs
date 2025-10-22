using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using CincoAcadimy.Models;
using CincoAcadimy.Models.Authorization;

namespace CincoAcadimy.Authorization
{
    /// <summary>
    /// Authorization handler for role-based access control
    /// </summary>
    public class RoleAuthorizationHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleAuthorizationHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RoleRequirement requirement)
        {
            if (context.User?.Identity?.IsAuthenticated != true)
            {
                context.Fail();
                return;
            }

            var user = await _userManager.GetUserAsync(context.User);
            if (user == null)
            {
                context.Fail();
                return;
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            
            if (requirement.RequiredRoles.Any(role => userRoles.Contains(role)))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }

    /// <summary>
    /// Authorization requirement for roles
    /// </summary>
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string[] RequiredRoles { get; }

        public RoleRequirement(params string[] roles)
        {
            RequiredRoles = roles ?? throw new ArgumentNullException(nameof(roles));
        }
    }

    /// <summary>
    /// Authorization handler for permission-based access control
    /// </summary>
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PermissionAuthorizationHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (context.User?.Identity?.IsAuthenticated != true)
            {
                context.Fail();
                return;
            }

            var user = await _userManager.GetUserAsync(context.User);
            if (user == null)
            {
                context.Fail();
                return;
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var userPermissions = GetPermissionsForRoles(userRoles.ToArray());

            if (requirement.RequiredPermissions.Any(permission => userPermissions.Contains(permission)))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }

        private static string[] GetPermissionsForRoles(string[] roles)
        {
            var permissions = new List<string>();

            foreach (var role in roles)
            {
                switch (role)
                {
                    case "Admin":
                        permissions.AddRange(Permissions.AdminPermissions);
                        break;
                    case "Instructor":
                        permissions.AddRange(Permissions.InstructorPermissions);
                        break;
                    case "HR":
                        permissions.AddRange(Permissions.HRPermissions);
                        break;
                    case "Student":
                        permissions.AddRange(Permissions.StudentPermissions);
                        break;
                }
            }

            return permissions.Distinct().ToArray();
        }
    }

    /// <summary>
    /// Authorization requirement for permissions
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string[] RequiredPermissions { get; }

        public PermissionRequirement(params string[] permissions)
        {
            RequiredPermissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
        }
    }
}
