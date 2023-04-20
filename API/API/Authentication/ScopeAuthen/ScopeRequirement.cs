using Microsoft.AspNetCore.Authorization;

namespace API.Authentication.ScopeAuthen
{
    public class ScopeRequirement : IAuthorizationRequirement
    {
        public string Scope { get; }

        public ScopeRequirement(string scope) => Scope = scope;
    }
}