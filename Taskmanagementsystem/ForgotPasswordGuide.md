# How to Use the Forgot Password Endpoint

## Overview
The forgot password functionality in your task management system allows users to reset their passwords when they forget them. This process involves two steps:

1. **Request Password Reset** - User requests a password reset link
2. **Reset Password** - User uses the link to set a new password

## Step 1: Request Password Reset

### Endpoint
```
POST /api/account/ForgotPassword
```

### Request Body
```json
{
  "email": "user@example.com"
}
```

### Example Request (cURL)
```bash
curl -X POST "https://localhost:7001/api/account/ForgotPassword" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com"
  }'
```

### Example Request (HTTP)
```http
POST /api/account/ForgotPassword HTTP/1.1
Host: localhost:7001
Content-Type: application/json

{
  "email": "user@example.com"
}
```

### Response
**Success (200 OK):**
```json
{
  "message": "Password reset link has been sent.",
  "resetLink": "https://localhost:7001/api/account/ResetPassword?token=...&email=user@example.com"
}
```

**Note:** The system always returns a success message even if the email doesn't exist (for security reasons to prevent email enumeration).

## Step 2: Reset Password

### Endpoint
```
POST /api/account/ResetPassword
```

### Request Body
```json
{
  "email": "user@example.com",
  "token": "reset_token_from_step_1",
  "newPassword": "NewPassword123!",
  "confirmPassword": "NewPassword123!"
}
```

### Example Request (cURL)
```bash
curl -X POST "https://localhost:7001/api/account/ResetPassword" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "token": "CfDJ8...",
    "newPassword": "NewPassword123!",
    "confirmPassword": "NewPassword123!"
  }'
```

### Response
**Success (200 OK):**
```json
"Password has been reset successfully"
```

**Error (400 Bad Request):**
```json
{
  "Password": ["Passwords do not match"],
  "NewPassword": ["Password must be at least 6 characters long"]
}
```

## Complete Flow Example

### 1. User forgets password and requests reset
```bash
curl -X POST "https://localhost:7001/api/account/ForgotPassword" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john.doe@company.com"
  }'
```

**Response:**
```json
{
  "message": "Password reset link has been sent.",
  "resetLink": "https://localhost:7001/api/account/ResetPassword?token=CfDJ8...&email=john.doe@company.com"
}
```

### 2. User clicks the reset link and sets new password
```bash
curl -X POST "https://localhost:7001/api/account/ResetPassword" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john.doe@company.com",
    "token": "CfDJ8...",
    "newPassword": "MyNewSecurePassword123!",
    "confirmPassword": "MyNewSecurePassword123!"
  }'
```

**Response:**
```json
"Password has been reset successfully"
```

### 3. User can now login with new password
```bash
curl -X POST "https://localhost:7001/api/account/Login" \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "john.doe",
    "password": "MyNewSecurePassword123!"
  }'
```

## Important Notes

### Security Features
1. **Email Enumeration Protection**: The system always returns a success message even if the email doesn't exist
2. **Token Expiration**: Reset tokens have a limited lifespan (typically 24 hours)
3. **One-time Use**: Reset tokens can only be used once
4. **Password Validation**: New passwords must meet complexity requirements

### Current Implementation Status
- ✅ Password reset token generation
- ✅ Reset link generation
- ⚠️ **Email sending is not implemented** (currently returns link in response)
- ✅ Password reset validation
- ✅ Password confirmation matching

### TODO: Implement Email Sending
The current implementation returns the reset link in the response. In production, you should:

1. **Configure SMTP settings** in `appsettings.json`:
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "Username": "your-email@gmail.com",
    "Password": "your-app-password",
    "FromEmail": "noreply@yourcompany.com",
    "FromName": "Task Management System"
  }
}
```

2. **Create an email service** to send the reset link via email instead of returning it in the response.

## Testing the Endpoint

### Using Swagger UI
1. Navigate to `https://localhost:7001/swagger`
2. Find the `POST /api/account/ForgotPassword` endpoint
3. Click "Try it out"
4. Enter the email address
5. Execute the request

### Using Postman
1. Create a new POST request
2. Set URL to `https://localhost:7001/api/account/ForgotPassword`
3. Set Content-Type header to `application/json`
4. Add request body with email
5. Send the request

## Error Handling

### Common Errors
1. **Invalid Email Format**: Returns 400 Bad Request
2. **Passwords Don't Match**: Returns 400 Bad Request
3. **Invalid Token**: Returns 400 Bad Request
4. **User Not Found**: Returns success message (security feature)
5. **Inactive User**: Returns success message (security feature)

### Validation Rules
- Email must be in valid format
- New password must meet complexity requirements
- Confirm password must match new password
- Token must be valid and not expired

## Integration with Frontend

### React/JavaScript Example
```javascript
// Step 1: Request password reset
const requestPasswordReset = async (email) => {
  try {
    const response = await fetch('/api/account/ForgotPassword', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ email })
    });
    
    const data = await response.json();
    console.log('Reset link:', data.resetLink);
    // In production, this link would be sent via email
  } catch (error) {
    console.error('Error requesting password reset:', error);
  }
};

// Step 2: Reset password
const resetPassword = async (email, token, newPassword, confirmPassword) => {
  try {
    const response = await fetch('/api/account/ResetPassword', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        email,
        token,
        newPassword,
        confirmPassword
      })
    });
    
    if (response.ok) {
      console.log('Password reset successful');
    } else {
      const error = await response.json();
      console.error('Password reset failed:', error);
    }
  } catch (error) {
    console.error('Error resetting password:', error);
  }
};
``` 