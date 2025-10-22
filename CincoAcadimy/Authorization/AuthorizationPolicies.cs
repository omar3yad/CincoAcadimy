using Microsoft.AspNetCore.Authorization;
using CincoAcadimy.Models;
using CincoAcadimy.Models.Authorization;
using CincoAcadimy.Authorization;

namespace CincoAcadimy.Authorization
{
    /// <summary>
    /// Configuration class for authorization policies
    /// </summary>
    public static class AuthorizationPolicies
    {
        /// <summary>
        /// Configures all authorization policies for the application
        /// </summary>
        public static void ConfigurePolicies(AuthorizationOptions options)
        {
            // Role-based policies
            ConfigureRolePolicies(options);
            
            // Permission-based policies
            ConfigurePermissionPolicies(options);
            
            // Combined policies
            ConfigureCombinedPolicies(options);
        }

        private static void ConfigureRolePolicies(AuthorizationOptions options)
        {
            // Admin only policy
            options.AddPolicy("AdminOnly", policy =>
                policy.Requirements.Add(new RoleRequirement("Admin")));

            // Instructor or higher policy
            options.AddPolicy("InstructorOrHigher", policy =>
                policy.Requirements.Add(new RoleRequirement("Admin", "Instructor")));

            // HR or higher policy
            options.AddPolicy("HROrHigher", policy =>
                policy.Requirements.Add(new RoleRequirement("Admin", "HR")));

            // Student or higher policy (essentially authenticated users)
            options.AddPolicy("StudentOrHigher", policy =>
                policy.Requirements.Add(new RoleRequirement("Admin", "Instructor", "HR", "Student")));

            // Management roles (Admin and HR)
            options.AddPolicy("ManagementRoles", policy =>
                policy.Requirements.Add(new RoleRequirement("Admin", "HR")));
        }

        private static void ConfigurePermissionPolicies(AuthorizationOptions options)
        {
            // User Management Policies
            options.AddPolicy("CreateUser", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.CreateUser)));
            
            options.AddPolicy("ReadUser", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.ReadUser)));
            
            options.AddPolicy("UpdateUser", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.UpdateUser)));
            
            options.AddPolicy("DeleteUser", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.DeleteUser)));
            
            options.AddPolicy("ChangeUserRole", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.ChangeUserRole)));

            // Course Management Policies
            options.AddPolicy("CreateCourse", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.CreateCourse)));
            
            options.AddPolicy("ReadCourse", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.ReadCourse)));
            
            options.AddPolicy("UpdateCourse", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.UpdateCourse)));
            
            options.AddPolicy("DeleteCourse", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.DeleteCourse)));
            
            options.AddPolicy("EnrollInCourse", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.EnrollInCourse)));

            // Session Management Policies
            options.AddPolicy("CreateSession", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.CreateSession)));
            
            options.AddPolicy("ReadSession", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.ReadSession)));
            
            options.AddPolicy("UpdateSession", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.UpdateSession)));
            
            options.AddPolicy("DeleteSession", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.DeleteSession)));
            
            options.AddPolicy("TakeAttendance", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.TakeAttendance)));

            // Assessment Policies
            options.AddPolicy("CreateAssessment", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.CreateAssessment)));
            
            options.AddPolicy("ReadAssessment", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.ReadAssessment)));
            
            options.AddPolicy("UpdateAssessment", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.UpdateAssessment)));
            
            options.AddPolicy("DeleteAssessment", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.DeleteAssessment)));
            
            options.AddPolicy("GradeAssessment", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.GradeAssessment)));

            // Payment Policies
            options.AddPolicy("CreatePayment", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.CreatePayment)));
            
            options.AddPolicy("ReadPayment", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.ReadPayment)));
            
            options.AddPolicy("UpdatePayment", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.UpdatePayment)));
            
            options.AddPolicy("DeletePayment", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.DeletePayment)));

            // Resource Policies
            options.AddPolicy("CreateResource", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.CreateResource)));
            
            options.AddPolicy("ReadResource", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.ReadResource)));
            
            options.AddPolicy("UpdateResource", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.UpdateResource)));
            
            options.AddPolicy("DeleteResource", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.DeleteResource)));

            // Dashboard Policies
            options.AddPolicy("ViewAdminDashboard", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.ViewAdminDashboard)));
            
            options.AddPolicy("ViewInstructorDashboard", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.ViewInstructorDashboard)));
            
            options.AddPolicy("ViewHRDashboard", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.ViewHRDashboard)));
            
            options.AddPolicy("ViewStudentDashboard", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.ViewStudentDashboard)));

            // Report Policies
            options.AddPolicy("ViewReports", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.ViewReports)));
            
            options.AddPolicy("ExportReports", policy =>
                policy.Requirements.Add(new PermissionRequirement(Permissions.ExportReports)));
        }

        private static void ConfigureCombinedPolicies(AuthorizationOptions options)
        {
            // Multiple permission policies
            options.AddPolicy("UserManagement", policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    Permissions.CreateUser, 
                    Permissions.ReadUser, 
                    Permissions.UpdateUser, 
                    Permissions.DeleteUser, 
                    Permissions.ChangeUserRole)));

            options.AddPolicy("CourseManagement", policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    Permissions.CreateCourse, 
                    Permissions.ReadCourse, 
                    Permissions.UpdateCourse, 
                    Permissions.DeleteCourse)));

            options.AddPolicy("SessionManagement", policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    Permissions.CreateSession, 
                    Permissions.ReadSession, 
                    Permissions.UpdateSession, 
                    Permissions.DeleteSession, 
                    Permissions.TakeAttendance)));

            options.AddPolicy("AssessmentManagement", policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    Permissions.CreateAssessment, 
                    Permissions.ReadAssessment, 
                    Permissions.UpdateAssessment, 
                    Permissions.DeleteAssessment, 
                    Permissions.GradeAssessment)));

            options.AddPolicy("PaymentManagement", policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    Permissions.CreatePayment, 
                    Permissions.ReadPayment, 
                    Permissions.UpdatePayment, 
                    Permissions.DeletePayment)));

            options.AddPolicy("ResourceManagement", policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    Permissions.CreateResource, 
                    Permissions.ReadResource, 
                    Permissions.UpdateResource, 
                    Permissions.DeleteResource)));
        }
    }
}
