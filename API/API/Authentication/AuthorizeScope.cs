using Microsoft.AspNetCore.Authorization;

namespace API.Authentication
{
    public class AuthorizeScope
    {
        private readonly RequestDelegate _next;
        // private readonly PathString _path;
        // private readonly String[] _scopes;
        public AuthorizeScope(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext, IAuthorizationService authorizationService)
        {
            var roles = httpContext.User.Claims.FirstOrDefault(c => c.Type == "scope")?.Value.Split(" ");
            if(roles.Contains("test"))
            {
                // httpContext
            }

            await _next(httpContext);
        }
    }
}