using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace CincoAcadimy.Attributes
{
    /// <summary>
    /// Custom authorization attribute for role-based access control
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RequireRoleAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _requiredRoles;

        public RequireRoleAttribute(params string[] roles)
        {
            _requiredRoles = roles ?? throw new ArgumentNullException(nameof(roles));
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User?.Identity?.IsAuthenticated != true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userRoles = context.HttpContext.User.FindAll(ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!_requiredRoles.Any(role => userRoles.Contains(role)))
            {
                context.Result = new ForbidResult();
            }
        }
    }

    /// <summary>
    /// Custom authorization attribute for permission-based access control
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RequirePermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _requiredPermissions;

        public RequirePermissionAttribute(params string[] permissions)
        {
            _requiredPermissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User?.Identity?.IsAuthenticated != true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userPermissions = context.HttpContext.User.FindAll("Permission")
                .Select(c => c.Value)
                .ToList();

            if (!_requiredPermissions.Any(permission => userPermissions.Contains(permission)))
            {
                context.Result = new ForbidResult();
            }
        }
    }

    /// <summary>
    /// Custom authorization attribute for Admin role only
    /// </summary>
    public class RequireAdminAttribute : RequireRoleAttribute
    {
        public RequireAdminAttribute() : base("Admin") { }
    }

    /// <summary>
    /// Custom authorization attribute for Instructor role or higher
    /// </summary>
    public class RequireInstructorAttribute : RequireRoleAttribute
    {
        public RequireInstructorAttribute() : base("Admin", "Instructor") { }
    }

    /// <summary>
    /// Custom authorization attribute for HR role or higher
    /// </summary>
    public class RequireHRAttribute : RequireRoleAttribute
    {
        public RequireHRAttribute() : base("Admin", "HR") { }
    }

    /// <summary>
    /// Custom authorization attribute for Student role or higher
    /// </summary>
    public class RequireStudentAttribute : RequireRoleAttribute
    {
        public RequireStudentAttribute() : base("Admin", "Instructor", "HR", "Student") { }
    }
}
