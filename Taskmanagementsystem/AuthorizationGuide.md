# Authorization Guide for Task Management System

## Overview
This guide explains how to select the appropriate authorization type for every endpoint in your ASP.NET Core task management system.

## Authorization Types Available

### 1. **No Authorization (Allow Anonymous)**
```csharp
[AllowAnonymous]
```
- **Use for**: Public endpoints like login, register, forgot password
- **Example**: `/api/account/login`, `/api/account/register`

### 2. **Basic Authentication**
```csharp
[Authorize]
```
- **Use for**: Endpoints that require any authenticated user
- **Example**: User profile endpoints, basic CRUD operations

### 3. **Role-Based Authorization**
```csharp
[Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead")]
```
- **Use for**: Endpoints that require specific roles
- **Available Roles**: SuperAdmin, ProjectManager, TeamLead, Member

### 4. **Policy-Based Authorization**
```csharp
[Authorize(Policy = "ProjectManager")]
[Authorize(Policy = "SuperAdmin")]
[Authorize(Policy = "Authenticated")]
```
- **Use for**: More complex authorization rules
- **Available Policies**: SuperAdmin, ProjectManager, TeamLead, Authenticated

## Endpoint-Specific Authorization Recommendations

### Account Controller
```csharp
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    [HttpPost("Register")]
    [AllowAnonymous]  // Public registration
    public async Task<IActionResult> Register(ResgiterUserDTO dto) { }

    [HttpPost("Login")]
    [AllowAnonymous]  // Public login
    public async Task<IActionResult> Login(LoginDTO dto) { }

    [HttpPost("Logout")]
    [Authorize]  // Any authenticated user can logout
    public IActionResult Logout() { }

    [HttpPost("ForgotPassword")]
    [AllowAnonymous]  // Public password reset
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto) { }

    [HttpPost("ResetPassword")]
    [AllowAnonymous]  // Public password reset
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto) { }
}
```

### Project Controller
```csharp
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "SuperAdmin,ProjectManager")]  // Default for all project operations
public class ProjectController : ControllerBase
{
    [HttpPost]
    // Inherits controller-level authorization
    public async Task<IActionResult> CreateProject(CreateProjectDTO dto) { }

    [HttpGet("{id}")]
    [Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead")]  // Override for read access
    public async Task<IActionResult> GetProject(Guid id) { }

    [HttpGet]
    [Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead")]  // Override for read access
    public async Task<IActionResult> GetAllProjects() { }

    [HttpPut("{id}")]
    // Inherits controller-level authorization
    public async Task<IActionResult> UpdateProject(Guid id, UpdateProjectDTO dto) { }

    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin")]  // Only SuperAdmin can delete projects
    public async Task<IActionResult> DeleteProject(Guid id) { }
}
```

### Task Controller
```csharp
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead")]  // Default for task operations
public class TaskController : ControllerBase
{
    [HttpPost]
    // Inherits controller-level authorization
    public async Task<IActionResult> CreateTask(CreateTaskDTO dto) { }

    [HttpGet("{id}")]
    [Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead,Member")]  // Allow members to view tasks
    public async Task<IActionResult> GetTask(Guid id) { }

    [HttpGet]
    [Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead")]  // Override for list access
    public async Task<IActionResult> GetAllTasks() { }

    [HttpGet("project/{projectId}")]
    [Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead,Member")]  // Allow members to view project tasks
    public async Task<IActionResult> GetTasksByProject(Guid projectId) { }

    [HttpGet("user/{userId}")]
    [Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead,Member")]  // Allow members to view their tasks
    public async Task<IActionResult> GetTasksByUser(Guid userId) { }

    [HttpPut("{id}")]
    // Inherits controller-level authorization
    public async Task<IActionResult> UpdateTask(Guid id, UpdateTaskDTO dto) { }

    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin,ProjectManager")]  // Only managers can delete tasks
    public async Task<IActionResult> DeleteTask(Guid id) { }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead,Member")]  // Allow members to update task status
    public async Task<IActionResult> ChangeTaskStatus(Guid id, [FromBody] string newStatus) { }
}
```

### Project Member Controller
```csharp
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "SuperAdmin,ProjectManager")]  // Default for member management
public class ProjectMemberController : ControllerBase
{
    [HttpPost]
    // Inherits controller-level authorization
    public async Task<IActionResult> AddMember(AddProjectMemberDTO dto) { }

    [HttpGet("project/{projectId}")]
    [Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead")]  // Allow team leads to view members
    public async Task<IActionResult> GetProjectMembers(Guid projectId) { }

    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin,ProjectManager")]  // Only managers can remove members
    public async Task<IActionResult> RemoveMember(Guid id) { }
}
```

### User Management Controller
```csharp
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "SuperAdmin")]  // Only SuperAdmin can manage users
public class UserManagementController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllUsers() { }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id) { }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDTO dto) { }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id) { }

    [HttpPut("{id}/role")]
    public async Task<IActionResult> UpdateUserRole(Guid id, [FromBody] string newRole) { }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateUserStatus(Guid id, [FromBody] bool isActive) { }
}
```

## Authorization Hierarchy

### Role Hierarchy (from highest to lowest privileges):
1. **SuperAdmin** - Full system access
2. **ProjectManager** - Project and task management
3. **TeamLead** - Team and task management
4. **Member** - Basic task operations

### Common Authorization Patterns:

#### Read Operations
- **Public**: Login, Register, ForgotPassword
- **Authenticated Users**: User profile, own tasks
- **Team Members**: Project tasks, team information
- **Managers**: All project and task data

#### Write Operations
- **Members**: Update own task status
- **Team Leads**: Create/update tasks, manage team
- **Project Managers**: Create/update projects, manage members
- **SuperAdmin**: Full system management

#### Delete Operations
- **Project Managers**: Delete tasks, remove members
- **SuperAdmin**: Delete projects, delete users

## Best Practices

1. **Principle of Least Privilege**: Grant minimum required permissions
2. **Controller-Level Defaults**: Set appropriate default authorization at controller level
3. **Action-Level Overrides**: Override specific actions when needed
4. **Consistent Patterns**: Use similar authorization patterns across similar endpoints
5. **Security by Design**: Always consider security implications when designing endpoints

## Testing Authorization

### Test with Different Roles:
```bash
# SuperAdmin token
curl -H "Authorization: Bearer {superadmin-token}" https://localhost:7001/api/projects

# ProjectManager token
curl -H "Authorization: Bearer {projectmanager-token}" https://localhost:7001/api/projects

# Member token
curl -H "Authorization: Bearer {member-token}" https://localhost:7001/api/projects
```

### Expected Results:
- SuperAdmin: Full access to all endpoints
- ProjectManager: Access to project and task management
- TeamLead: Access to team and task management
- Member: Limited access to own tasks and basic operations 