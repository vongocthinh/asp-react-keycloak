using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace API.Authentication.ScopeAuthen
{
    public class ScopePolicyProvider : IAuthorizationPolicyProvider
    {
        const string SCOPE_PREFIX = "Scope";
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(
                new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().Build());
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return Task.FromResult<AuthorizationPolicy>(null);
        }
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(SCOPE_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
                policy.AddRequirements(new ScopeRequirement(policyName.Substring(SCOPE_PREFIX.Length)));
                return Task.FromResult((AuthorizationPolicy)policy.Build());
            }
            else
            {
                return Task.FromResult<AuthorizationPolicy>(null);
            }
        }
    }
}