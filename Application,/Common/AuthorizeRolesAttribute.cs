using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Application_.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params UserRole[] roles)
        {
            Roles = string.Join(",", roles.Select(r => r.ToString()));
        }
    }
} 