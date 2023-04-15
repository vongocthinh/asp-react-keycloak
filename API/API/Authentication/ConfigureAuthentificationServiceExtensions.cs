using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;

namespace API.Authentication
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity != null) {
                ClaimsIdentity claimsIdentity = (ClaimsIdentity) principal.Identity;

                if (claimsIdentity.IsAuthenticated && claimsIdentity.HasClaim((claim) => claim.Type == "resource_access"))
                {
                    var userRole = claimsIdentity.FindFirst((claim) => claim.Type == "resource_access");
                    if (userRole != null)
                    {

                        var content = Newtonsoft.Json.Linq.JObject.Parse(userRole.Value);
                        foreach (var role in content["my-app"]["roles"])
                        {
                            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
                        }
                    }
                }
            }

            return Task.FromResult(principal);
        }
    }
    public static class ConfigureAuthentificationServiceExtensions
    {
        private static RsaSecurityKey BuildRSAKey(string publicKeyJWT)
        {
            RSA rsa = RSA.Create();

            rsa.ImportSubjectPublicKeyInfo(

                source: Convert.FromBase64String(publicKeyJWT),
                bytesRead: out _
            );

            var IssuerSigningKey = new RsaSecurityKey(rsa);

            return IssuerSigningKey;
        }

        public static void ConfigureJWT(this IServiceCollection services, bool IsDevelopment, string publicKeyJWT)
        {
            services.AddTransient<IClaimsTransformation, ClaimsTransformer>();
            var AuthenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            AuthenticationBuilder.AddJwtBearer(options =>
                {
                    #region == JWT Token Validation ===
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = true,
                        ValidIssuers = new string[] {"http://localhost:8082/auth/realms/myrealm"},
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = BuildRSAKey(publicKeyJWT),
                        ValidateLifetime = true,
                    };
                    #endregion

                    #region === Event Authentification Handlers ===
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = c =>
                        {
                            Console.WriteLine("User successfully authenticated");
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            if (IsDevelopment) return c.Response.WriteAsync(c.Exception.ToString());
                            return c.Response.WriteAsync("An error occured processing your authentication.");
                        }
                    };
                    #endregion
                }
            );
        }
    }
}