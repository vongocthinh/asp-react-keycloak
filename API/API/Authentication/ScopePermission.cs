using Microsoft.AspNetCore.Authorization;

namespace API.Authentication
{
    public class ScopePermission : AuthorizationHandler<ScopeRequirement>
    {
        protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, ScopeRequirement requirement)
        {
            var userClaims = context.User.Claims;
            var userScopes = userClaims.FirstOrDefault(c => c.Type == "scope")?.Value?.Split(" ");
            if (userScopes.Contains(requirement.Scope)) {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}