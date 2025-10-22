# Role-Based Access Control (RBAC) System Documentation

## Overview

This document describes the comprehensive Role-Based Access Control (RBAC) system implemented for the CincoAcadimy application. The system provides fine-grained access control using both roles and permissions.

## User Roles

The system defines four main user roles:

### 1. Admin
- **Description**: Full system access with all permissions
- **Permissions**: All available permissions in the system
- **Use Cases**: System administration, user management, course management, etc.

### 2. Instructor
- **Description**: Teaching staff with course and student management capabilities
- **Permissions**: 
  - Read users and courses
  - Create, read, update sessions
  - Create, read, update assessments
  - Grade assessments
  - Take attendance
  - Create and manage resources
  - View instructor dashboard

### 3. HR (Human Resources)
- **Description**: Human resources staff with user and payment management capabilities
- **Permissions**:
  - Create, read, update users
  - Change user roles
  - Read courses, sessions, and assessments
  - Create, read, update payments
  - View HR dashboard
  - View and export reports

### 4. Student
- **Description**: Students with limited access to their own data and enrolled courses
- **Permissions**:
  - Read courses and enroll in courses
  - Read sessions and assessments
  - Read payments and resources
  - View student dashboard

## Permissions System

The system uses a permission-based approach where each action requires specific permissions:

### User Management Permissions
- `CreateUser`: Create new users
- `ReadUser`: View user information
- `UpdateUser`: Modify user information
- `DeleteUser`: Remove users
- `ChangeUserRole`: Assign roles to users

### Course Management Permissions
- `CreateCourse`: Create new courses
- `ReadCourse`: View course information
- `UpdateCourse`: Modify course information
- `DeleteCourse`: Remove courses
- `EnrollInCourse`: Enroll in courses

### Session Management Permissions
- `CreateSession`: Create new sessions
- `ReadSession`: View session information
- `UpdateSession`: Modify session information
- `DeleteSession`: Remove sessions
- `TakeAttendance`: Record attendance

### Assessment Permissions
- `CreateAssessment`: Create new assessments
- `ReadAssessment`: View assessment information
- `UpdateAssessment`: Modify assessment information
- `DeleteAssessment`: Remove assessments
- `GradeAssessment`: Grade student assessments

### Payment Permissions
- `CreatePayment`: Create new payments
- `ReadPayment`: View payment information
- `UpdatePayment`: Modify payment information
- `DeletePayment`: Remove payments

### Resource Permissions
- `CreateResource`: Create new resources
- `ReadResource`: View resource information
- `UpdateResource`: Modify resource information
- `DeleteResource`: Remove resources

### Dashboard Permissions
- `ViewAdminDashboard`: Access admin dashboard
- `ViewInstructorDashboard`: Access instructor dashboard
- `ViewHRDashboard`: Access HR dashboard
- `ViewStudentDashboard`: Access student dashboard

### Report Permissions
- `ViewReports`: View system reports
- `ExportReports`: Export reports

## Implementation

### 1. Authorization Attributes

The system provides several custom authorization attributes:

```csharp
[RequireRole("Admin")]                    // Admin only
[RequireInstructor]                       // Instructor or Admin
[RequireHR]                              // HR or Admin
[RequireStudent]                         // Any authenticated user
[RequirePermission("CreateUser")]         // Specific permission
[RequirePermission("ReadUser", "UpdateUser")] // Multiple permissions
```

### 2. Authorization Policies

Policies are configured in `Program.cs` and can be used with the `[Authorize]` attribute:

```csharp
[Authorize(Policy = "AdminOnly")]
[Authorize(Policy = "InstructorOrHigher")]
[Authorize(Policy = "HROrHigher")]
[Authorize(Policy = "StudentOrHigher")]
[Authorize(Policy = "CreateUser")]
[Authorize(Policy = "ReadUser")]
[Authorize(Policy = "UserManagement")]
```

### 3. Controller Implementation

Controllers should be decorated with authorization attributes:

```csharp
[Route("api/[controller]")]
[ApiController]
[Authorize] // Require authentication for all endpoints
public class CourseController : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "ReadCourse")]
    public async Task<IActionResult> GetAll()
    {
        // Implementation
    }

    [HttpPost]
    [Authorize(Policy = "CreateCourse")]
    public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
    {
        // Implementation
    }
}
```

## API Endpoints

### Authentication Endpoints
- `POST /api/account/register` - Register new user (Anonymous)
- `POST /api/account/login` - User login (Anonymous)

### User Management Endpoints
- `GET /api/account/profile` - Get current user profile (Authenticated)
- `GET /api/account/users` - Get all users (ReadUser permission)
- `POST /api/account/create-user` - Create new user (CreateUser permission)
- `PUT /api/account/update-user/{userId}` - Update user (UpdateUser permission)
- `DELETE /api/account/delete-user/{userId}` - Delete user (DeleteUser permission)
- `POST /api/account/ChangeRole` - Change user role (ChangeUserRole permission)
- `GET /api/account/by-role/{role}` - Get users by role (HROrHigher policy)

### System Information Endpoints
- `GET /api/account/roles` - Get all available roles (ReadUser permission)
- `GET /api/account/permissions` - Get all available permissions (ReadUser permission)
- `GET /api/account/user-permissions/{userId}` - Get user permissions (ReadUser permission)

## Usage Examples

### 1. Protecting Controller Actions

```csharp
[HttpGet("admin-only")]
[Authorize(Policy = "AdminOnly")]
public IActionResult AdminOnlyAction()
{
    return Ok("Only admins can see this");
}

[HttpPost("create-course")]
[Authorize(Policy = "CreateCourse")]
public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto dto)
{
    // Only users with CreateCourse permission can access this
    await _courseService.CreateCourseAsync(dto);
    return Ok("Course created successfully");
}
```

### 2. Using Custom Attributes

```csharp
[HttpGet("instructor-dashboard")]
[RequireInstructor]
public IActionResult GetInstructorDashboard()
{
    // Only instructors and admins can access this
    return Ok("Instructor dashboard data");
}

[HttpPost("grade-assessment")]
[RequirePermission("GradeAssessment")]
public async Task<IActionResult> GradeAssessment([FromBody] GradeDto dto)
{
    // Only users with GradeAssessment permission can access this
    await _assessmentService.GradeAssessmentAsync(dto);
    return Ok("Assessment graded successfully");
}
```

### 3. Checking User Roles in Code

```csharp
public async Task<IActionResult> SomeAction()
{
    var user = await _userManager.GetUserAsync(User);
    var userRoles = await _userManager.GetRolesAsync(user);
    
    if (userRoles.Contains("Admin"))
    {
        // Admin-specific logic
    }
    else if (userRoles.Contains("Instructor"))
    {
        // Instructor-specific logic
    }
    
    return Ok();
}
```

## Security Considerations

1. **Authentication Required**: All endpoints require authentication except login and register
2. **Permission-Based Access**: Fine-grained control using permissions
3. **Role Hierarchy**: Admin has all permissions, other roles have specific permissions
4. **JWT Token Validation**: All requests must include valid JWT tokens
5. **Authorization Policies**: Centralized policy configuration for consistency

## Best Practices

1. **Use Policies Over Roles**: Prefer permission-based policies over role-based attributes
2. **Consistent Naming**: Use consistent naming conventions for permissions and policies
3. **Document Permissions**: Document which permissions are required for each endpoint
4. **Test Authorization**: Ensure all endpoints are properly protected
5. **Regular Audits**: Regularly review and audit user permissions

## Migration and Setup

1. **Database Migration**: The system uses Entity Framework migrations for role and permission data
2. **Role Seeding**: Roles are automatically seeded on application startup
3. **Permission Assignment**: Permissions are automatically assigned based on user roles
4. **JWT Configuration**: Ensure JWT settings are properly configured in `appsettings.json`

## Troubleshooting

### Common Issues

1. **401 Unauthorized**: User is not authenticated or token is invalid
2. **403 Forbidden**: User is authenticated but lacks required permissions
3. **Policy Not Found**: Authorization policy is not properly configured

### Debugging

1. Check user authentication status
2. Verify user roles and permissions
3. Ensure policies are properly configured
4. Check JWT token validity and claims

## Conclusion

This RBAC system provides a robust, scalable, and maintainable approach to access control in the CincoAcadimy application. It supports both role-based and permission-based authorization, allowing for fine-grained control over user access to system resources.
