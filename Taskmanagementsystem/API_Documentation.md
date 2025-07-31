# Task Management System API Documentation

## Overview
This document provides comprehensive documentation for all endpoints in the Task Management System API. The API is built with ASP.NET Core and uses JWT authentication with role-based authorization.

**Base URL:** `https://localhost:7001`

**Authentication:** JWT Bearer Token

## Table of Contents
1. [Authentication](#authentication)
2. [Account Management](#account-management)
3. [User Management](#user-management)
4. [Project Management](#project-management)
5. [Task Management](#task-management)
6. [Project Member Management](#project-member-management)
7. [Error Responses](#error-responses)
8. [Authorization Levels](#authorization-levels)

---

## Authentication

### Login
**POST** `/api/account/Login`

**Description:** Authenticate user and receive JWT token

**Authorization:** `[AllowAnonymous]`

**Request Body:**
```json
{
  "userName": "string",
  "password": "string"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2024-01-01T12:00:00",
  "user": {
    "id": "guid",
    "userName": "string",
    "email": "string",
    "fullName": "string",
    "role": "string"
  }
}
```

**Response (400 Bad Request):**
```json
{
  "UserName": ["User name or password is invalid"]
}
```

### Register
**POST** `/api/account/Register`

**Description:** Register a new user account

**Authorization:** `[AllowAnonymous]`

**Request Body:**
```json
{
  "userName": "string",
  "email": "string",
  "password": "string"
}
```

**Response (200 OK):**
```json
"User registered successfully"
```

**Response (400 Bad Request):**
```json
{
  "": ["Error description"]
}
```

### Logout
**POST** `/api/account/Logout`

**Description:** Logout user (client-side token disposal)

**Authorization:** `[Authorize]`

**Response (200 OK):**
```json
"Logout successful"
```

### Forgot Password
**POST** `/api/account/ForgotPassword`

**Description:** Request password reset link

**Authorization:** `[AllowAnonymous]`

**Request Body:**
```json
{
  "email": "string"
}
```

**Response (200 OK):**
```json
{
  "message": "Password reset link has been sent.",
  "resetLink": "https://localhost:7001/api/account/ResetPassword?token=...&email=user@example.com"
}
```

### Reset Password
**POST** `/api/account/ResetPassword`

**Description:** Reset password using token

**Authorization:** `[AllowAnonymous]`

**Request Body:**
```json
{
  "email": "string",
  "token": "string",
  "newPassword": "string",
  "confirmPassword": "string"
}
```

**Response (200 OK):**
```json
"Password has been reset successfully"
```

**Response (400 Bad Request):**
```json
{
  "Password": ["Passwords do not match"],
  "NewPassword": ["Password must be at least 6 characters long"]
}
```

---

## User Management

**Base URL:** `/api/UserManagement`

**Default Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager")]`

### Get All Users
**GET** `/api/UserManagement`

**Description:** Retrieve all users in the system

**Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager")]`

**Response (200 OK):**
```json
[
  {
    "id": "guid",
    "userName": "string",
    "email": "string",
    "fullName": "string",
    "role": "string",
    "isActive": true,
    "createdAt": "2024-01-01T00:00:00Z",
    "lastLoginAt": "2024-01-01T00:00:00Z"
  }
]
```

### Get User by ID
**GET** `/api/UserManagement/{userId}`

**Description:** Retrieve a specific user by ID

**Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager")]`

**Parameters:**
- `userId` (Guid): The ID of the user to retrieve

**Response (200 OK):**
```json
{
  "id": "guid",
  "userName": "string",
  "email": "string",
  "fullName": "string",
  "role": "string",
  "isActive": true,
  "createdAt": "2024-01-01T00:00:00Z",
  "lastLoginAt": "2024-01-01T00:00:00Z"
}
```

**Response (404 Not Found):**
```json
"User not found"
```

### Delete User
**DELETE** `/api/UserManagement/{userId}`

**Description:** Delete a user from the system

**Authorization:** `[Authorize(Roles = "SuperAdmin")]`

**Parameters:**
- `userId` (Guid): The ID of the user to delete

**Response (200 OK):**
```json
"User deleted successfully"
```

**Response (404 Not Found):**
```json
"User not found"
```

### Activate User
**POST** `/api/UserManagement/{userId}/activate`

**Description:** Activate a deactivated user

**Authorization:** `[Authorize(Roles = "SuperAdmin")]`

**Parameters:**
- `userId` (Guid): The ID of the user to activate

**Response (200 OK):**
```json
"User activated successfully"
```

**Response (404 Not Found):**
```json
"User not found"
```

### Deactivate User
**POST** `/api/UserManagement/{userId}/deactivate`

**Description:** Deactivate a user account

**Authorization:** `[Authorize(Roles = "SuperAdmin")]`

**Parameters:**
- `userId` (Guid): The ID of the user to deactivate

**Response (200 OK):**
```json
"User deactivated successfully"
```

**Response (404 Not Found):**
```json
"User not found"
```

---

## Project Management

**Base URL:** `/api/Project`

**Default Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager")]`

### Create Project
**POST** `/api/Project`

**Description:** Create a new project

**Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager")]`

**Request Body:**
```json
{
  "name": "string",
  "description": "string",
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-12-31T00:00:00Z",
  "status": "string"
}
```

**Response (201 Created):**
```json
{
  "id": "guid",
  "name": "string",
  "description": "string",
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-12-31T00:00:00Z",
  "status": "string",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

**Response (400 Bad Request):**
```json
{
  "name": ["Project name is required"]
}
```

### Get All Projects
**GET** `/api/Project`

**Description:** Retrieve all projects

**Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead")]`

**Response (200 OK):**
```json
[
  {
    "id": "guid",
    "name": "string",
    "description": "string",
    "startDate": "2024-01-01T00:00:00Z",
    "endDate": "2024-12-31T00:00:00Z",
    "status": "string",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z"
  }
]
```

### Get Project by ID
**GET** `/api/Project/{id}`

**Description:** Retrieve a specific project by ID

**Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead")]`

**Parameters:**
- `id` (Guid): The ID of the project to retrieve

**Response (200 OK):**
```json
{
  "id": "guid",
  "name": "string",
  "description": "string",
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-12-31T00:00:00Z",
  "status": "string",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

**Response (404 Not Found):**
```json
"Project not found"
```

### Update Project
**PUT** `/api/Project/{id}`

**Description:** Update an existing project

**Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager")]`

**Parameters:**
- `id` (Guid): The ID of the project to update

**Request Body:**
```json
{
  "name": "string",
  "description": "string",
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-12-31T00:00:00Z",
  "status": "string"
}
```

**Response (200 OK):**
```json
{
  "id": "guid",
  "name": "string",
  "description": "string",
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-12-31T00:00:00Z",
  "status": "string",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

**Response (400 Bad Request):**
```json
{
  "name": ["Project name is required"]
}
```

### Delete Project
**DELETE** `/api/Project/{id}`

**Description:** Delete a project

**Authorization:** `[Authorize(Roles = "SuperAdmin")]`

**Parameters:**
- `id` (Guid): The ID of the project to delete

**Response (200 OK):**
```json
"Project deleted successfully"
```

**Response (404 Not Found):**
```json
"Project not found"
```

---

## Task Management

**Base URL:** `/api/Task`

**Default Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead")]`

### Create Task
**POST** `/api/Task`

**Description:** Create a new task

**Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead")]`

**Request Body:**
```json
{
  "title": "string",
  "description": "string",
  "projectId": "guid",
  "assignedUserId": "guid",
  "priority": "string",
  "status": "string",
  "dueDate": "2024-12-31T00:00:00Z"
}
```

**Response (201 Created):**
```json
{
  "id": "guid",
  "title": "string",
  "description": "string",
  "projectId": "guid",
  "assignedUserId": "guid",
  "priority": "string",
  "status": "string",
  "dueDate": "2024-12-31T00:00:00Z",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

**Response (400 Bad Request):**
```json
{
  "title": ["Task title is required"]
}
```

### Get All Tasks
**GET** `/api/Task`

**Description:** Retrieve all tasks

**Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead")]`

**Response (200 OK):**
```json
[
  {
    "id": "guid",
    "title": "string",
    "description": "string",
    "projectId": "guid",
    "assignedUserId": "guid",
    "priority": "string",
    "status": "string",
    "dueDate": "2024-12-31T00:00:00Z",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z"
  }
]
```

### Get Task by ID
**GET** `/api/Task/{id}`

**Description:** Retrieve a specific task by ID

**Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead,Member")]`

**Parameters:**
- `id` (Guid): The ID of the task to retrieve

**Response (200 OK):**
```json
{
  "id": "guid",
  "title": "string",
  "description": "string",
  "projectId": "guid",
  "assignedUserId": "guid",
  "priority": "string",
  "status": "string",
  "dueDate": "2024-12-31T00:00:00Z",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

**Response (404 Not Found):**
```json
"Task not found"
```

### Get Tasks by Project
**GET** `/api/Task/project/{projectId}`

**Description:** Retrieve all tasks for a specific project

**Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead,Member")]`

**Parameters:**
- `projectId` (Guid): The ID of the project

**Response (200 OK):**
```json
[
  {
    "id": "guid",
    "title": "string",
    "description": "string",
    "projectId": "guid",
    "assignedUserId": "guid",
    "priority": "string",
    "status": "string",
    "dueDate": "2024-12-31T00:00:00Z",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z"
  }
]
```

### Get Tasks by User
**GET** `/api/Task/user/{userId}`

**Description:** Retrieve all tasks assigned to a specific user

**Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead,Member")]`

**Parameters:**
- `userId` (Guid): The ID of the user

**Response (200 OK):**
```json
[
  {
    "id": "guid",
    "title": "string",
    "description": "string",
    "projectId": "guid",
    "assignedUserId": "guid",
    "priority": "string",
    "status": "string",
    "dueDate": "2024-12-31T00:00:00Z",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z"
  }
]
```

### Update Task
**PUT** `/api/Task/{id}`

**Description:** Update an existing task

**Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead")]`

**Parameters:**
- `id` (Guid): The ID of the task to update

**Request Body:**
```json
{
  "title": "string",
  "description": "string",
  "projectId": "guid",
  "assignedUserId": "guid",
  "priority": "string",
  "status": "string",
  "dueDate": "2024-12-31T00:00:00Z"
}
```

**Response (200 OK):**
```json
{
  "id": "guid",
  "title": "string",
  "description": "string",
  "projectId": "guid",
  "assignedUserId": "guid",
  "priority": "string",
  "status": "string",
  "dueDate": "2024-12-31T00:00:00Z",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

**Response (400 Bad Request):**
```json
{
  "title": ["Task title is required"]
}
```

### Delete Task
**DELETE** `/api/Task/{id}`

**Description:** Delete a task

**Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager")]`

**Parameters:**
- `id` (Guid): The ID of the task to delete

**Response (200 OK):**
```json
"Task deleted successfully"
```

**Response (404 Not Found):**
```json
"Task not found"
```

### Change Task Status
**PUT** `/api/Task/{id}/status`

**Description:** Update the status of a task

**Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead,Member")]`

**Parameters:**
- `id` (Guid): The ID of the task to update

**Request Body:**
```json
"NewStatus"
```

**Response (200 OK):**
```json
"Task status updated successfully"
```

**Response (404 Not Found):**
```json
"Task not found"
```

---

## Project Member Management

**Base URL:** `/api/ProjectMember`

**Default Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager")]`

### Assign Member to Project
**POST** `/api/ProjectMember/assign`

**Description:** Assign a user to a project

**Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager")]`

**Request Body:**
```json
{
  "userId": "guid",
  "projectId": "guid",
  "role": "string"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Member assigned successfully"
}
```

**Response (400 Bad Request):**
```json
{
  "success": false,
  "message": "User is already assigned to this project"
}
```

### List Project Members
**GET** `/api/ProjectMember/list/{projectId}`

**Description:** Get all members of a specific project

**Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead")]`

**Parameters:**
- `projectId` (Guid): The ID of the project

**Response (200 OK):**
```json
[
  {
    "id": "guid",
    "userId": "guid",
    "projectId": "guid",
    "role": "string",
    "assignedAt": "2024-01-01T00:00:00Z",
    "user": {
      "id": "guid",
      "userName": "string",
      "email": "string",
      "fullName": "string"
    }
  }
]
```

### Search Project Members
**POST** `/api/ProjectMember/search`

**Description:** Search for project members with filters

**Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead")]`

**Request Body:**
```json
{
  "projectId": "guid",
  "searchTerm": "string",
  "role": "string"
}
```

**Response (200 OK):**
```json
[
  {
    "id": "guid",
    "userId": "guid",
    "projectId": "guid",
    "role": "string",
    "assignedAt": "2024-01-01T00:00:00Z",
    "user": {
      "id": "guid",
      "userName": "string",
      "email": "string",
      "fullName": "string"
    }
  }
]
```

### Delete Project Member
**DELETE** `/api/ProjectMember/delete`

**Description:** Remove a user from a project

**Authorization:** `[Authorize(Roles = "SuperAdmin,ProjectManager")]`

**Request Body:**
```json
{
  "userId": "guid",
  "projectId": "guid"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Member removed successfully"
}
```

**Response (400 Bad Request):**
```json
{
  "success": false,
  "message": "User is not assigned to this project"
}
```

---

## Error Responses

### Common HTTP Status Codes

**400 Bad Request**
```json
{
  "fieldName": ["Error description"]
}
```

**401 Unauthorized**
```json
{
  "message": "Unauthorized"
}
```

**403 Forbidden**
```json
{
  "message": "Access denied"
}
```

**404 Not Found**
```json
"Resource not found"
```

**500 Internal Server Error**
```json
{
  "message": "An error occurred while processing your request"
}
```

### Validation Errors
```json
{
  "title": ["Task title is required"],
  "email": ["Invalid email format"],
  "password": ["Password must be at least 6 characters long"]
}
```

---

## Authorization Levels

### Role Hierarchy
1. **SuperAdmin** - Full system access
2. **ProjectManager** - Project and task management
3. **TeamLead** - Team and task management
4. **Member** - Basic task operations

### Authorization Patterns

#### Public Endpoints (No Authentication Required)
- `POST /api/account/Login`
- `POST /api/account/Register`
- `POST /api/account/ForgotPassword`
- `POST /api/account/ResetPassword`

#### Authenticated Endpoints (Any Authenticated User)
- `POST /api/account/Logout`

#### Role-Based Endpoints
- **SuperAdmin Only:**
  - `DELETE /api/Project/{id}`
  - `DELETE /api/UserManagement/{userId}`
  - `POST /api/UserManagement/{userId}/activate`
  - `POST /api/UserManagement/{userId}/deactivate`

- **SuperAdmin, ProjectManager:**
  - `POST /api/Project`
  - `PUT /api/Project/{id}`
  - `DELETE /api/Task/{id}`
  - `POST /api/ProjectMember/assign`
  - `DELETE /api/ProjectMember/delete`

- **SuperAdmin, ProjectManager, TeamLead:**
  - `POST /api/Task`
  - `PUT /api/Task/{id}`
  - `GET /api/Project`
  - `GET /api/Project/{id}`
  - `GET /api/Task`
  - `GET /api/ProjectMember/list/{projectId}`
  - `POST /api/ProjectMember/search`

- **SuperAdmin, ProjectManager, TeamLead, Member:**
  - `GET /api/Task/{id}`
  - `GET /api/Task/project/{projectId}`
  - `GET /api/Task/user/{userId}`
  - `PUT /api/Task/{id}/status`

---

## Testing the API

### Using Swagger UI
1. Navigate to `https://localhost:7001/swagger`
2. Click "Authorize" and enter your JWT token
3. Test endpoints directly from the UI

### Using cURL
```bash
# Login and get token
curl -X POST "https://localhost:7001/api/account/Login" \
  -H "Content-Type: application/json" \
  -d '{"userName": "admin", "password": "password123"}'

# Use token for authenticated requests
curl -X GET "https://localhost:7001/api/Project" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### Using Postman
1. Set base URL to `https://localhost:7001`
2. Add Authorization header: `Bearer YOUR_JWT_TOKEN`
3. Use the endpoints as documented above

---

## Data Models

### Common Fields
- `id` (Guid): Unique identifier
- `createdAt` (DateTime): Creation timestamp
- `updatedAt` (DateTime): Last update timestamp

### Enums
- **UserRole:** SuperAdmin, ProjectManager, TeamLead, Member
- **TaskPriority:** Low, Medium, High, Critical
- **TaskStatus:** Todo, InProgress, Review, Done
- **ProjectStatus:** Active, Completed, OnHold, Cancelled

---

## Rate Limiting
Currently, no rate limiting is implemented. Consider implementing rate limiting for production use.

## CORS
CORS is configured to allow requests from any origin in development. Configure appropriate CORS policies for production.

## Security Notes
- JWT tokens expire after 1 hour
- Passwords are hashed using ASP.NET Core Identity
- Email enumeration protection is implemented
- All sensitive endpoints require authentication
- Role-based authorization is enforced 