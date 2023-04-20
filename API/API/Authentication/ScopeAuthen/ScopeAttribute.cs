using Microsoft.AspNetCore.Authorization;

namespace API.Authentication.ScopeAuthen
{
    public class ScopeAttribute : AuthorizeAttribute
    {
        const string SCOPE_PREFIX = "Scope";
        public string RequiredScope
        {
            get
            {
                return Policy?.Substring(SCOPE_PREFIX.Length);
            }
            set 
            {
                Policy = $"{SCOPE_PREFIX}{value}";
            }
        }

        public ScopeAttribute(string requiredScope) => RequiredScope = requiredScope;
    }
}