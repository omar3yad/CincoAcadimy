namespace CincoAcadimy.Models.Authorization
{
    /// <summary>
    /// Defines all available permissions in the system
    /// </summary>
    public static class Permissions
    {
        // User Management Permissions
        public const string CreateUser = "CreateUser";
        public const string ReadUser = "ReadUser";
        public const string UpdateUser = "UpdateUser";
        public const string DeleteUser = "DeleteUser";
        public const string ChangeUserRole = "ChangeUserRole";
        
        // Course Management Permissions
        public const string CreateCourse = "CreateCourse";
        public const string ReadCourse = "ReadCourse";
        public const string UpdateCourse = "UpdateCourse";
        public const string DeleteCourse = "DeleteCourse";
        public const string EnrollInCourse = "EnrollInCourse";
        
        // Session Management Permissions
        public const string CreateSession = "CreateSession";
        public const string ReadSession = "ReadSession";
        public const string UpdateSession = "UpdateSession";
        public const string DeleteSession = "DeleteSession";
        public const string TakeAttendance = "TakeAttendance";
        
        // Assessment Permissions
        public const string CreateAssessment = "CreateAssessment";
        public const string ReadAssessment = "ReadAssessment";
        public const string UpdateAssessment = "UpdateAssessment";
        public const string DeleteAssessment = "DeleteAssessment";
        public const string GradeAssessment = "GradeAssessment";
        
        // Payment Permissions
        public const string CreatePayment = "CreatePayment";
        public const string ReadPayment = "ReadPayment";
        public const string UpdatePayment = "UpdatePayment";
        public const string DeletePayment = "DeletePayment";
        
        // Resource Permissions
        public const string CreateResource = "CreateResource";
        public const string ReadResource = "ReadResource";
        public const string UpdateResource = "UpdateResource";
        public const string DeleteResource = "DeleteResource";
        
        // Dashboard Permissions
        public const string ViewAdminDashboard = "ViewAdminDashboard";
        public const string ViewInstructorDashboard = "ViewInstructorDashboard";
        public const string ViewHRDashboard = "ViewHRDashboard";
        public const string ViewStudentDashboard = "ViewStudentDashboard";
        
        // Report Permissions
        public const string ViewReports = "ViewReports";
        public const string ExportReports = "ExportReports";
        
        /// <summary>
        /// Gets all available permissions
        /// </summary>
        public static readonly string[] AllPermissions = {
            CreateUser, ReadUser, UpdateUser, DeleteUser, ChangeUserRole,
            CreateCourse, ReadCourse, UpdateCourse, DeleteCourse, EnrollInCourse,
            CreateSession, ReadSession, UpdateSession, DeleteSession, TakeAttendance,
            CreateAssessment, ReadAssessment, UpdateAssessment, DeleteAssessment, GradeAssessment,
            CreatePayment, ReadPayment, UpdatePayment, DeletePayment,
            CreateResource, ReadResource, UpdateResource, DeleteResource,
            ViewAdminDashboard, ViewInstructorDashboard, ViewHRDashboard, ViewStudentDashboard,
            ViewReports, ExportReports
        };
        
        /// <summary>
        /// Gets permissions for Admin role
        /// </summary>
        public static readonly string[] AdminPermissions = AllPermissions;
        
        /// <summary>
        /// Gets permissions for Instructor role
        /// </summary>
        public static readonly string[] InstructorPermissions = {
            ReadUser, ReadCourse, CreateSession, ReadSession, UpdateSession,
            CreateAssessment, ReadAssessment, UpdateAssessment, GradeAssessment,
            ReadPayment, CreateResource, ReadResource, UpdateResource,
            ViewInstructorDashboard, TakeAttendance
        };
        
        /// <summary>
        /// Gets permissions for HR role
        /// </summary>
        public static readonly string[] HRPermissions = {
            CreateUser, ReadUser, UpdateUser, ChangeUserRole,
            ReadCourse, ReadSession, ReadAssessment, ReadPayment,
            CreatePayment, UpdatePayment, ViewHRDashboard, ViewReports, ExportReports
        };
        
        /// <summary>
        /// Gets permissions for Student role
        /// </summary>
        public static readonly string[] StudentPermissions = {
            ReadCourse, EnrollInCourse, ReadSession, ReadAssessment,
            ReadPayment, ReadResource, ViewStudentDashboard
        };
    }
}
