using API.Authentication;
using API.Authentication.ScopeAuthen;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _currentEnvironment;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _currentEnvironment = env;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            // IdentityModelEventSource.ShowPII = true;
            services.AddCors(o => o.AddPolicy("allow-spa", builder =>
            {
                builder.WithOrigins("http://localhost:3006")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.ConfigureJWT(_currentEnvironment.IsDevelopment(), "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAic9wSgfGhvqyEbba79FGacBil4dbAefdRtLHateWPEPxD78BJb5JaITEkV8EHH/05oCP5ObrS4TdCPgUWuGW2IPMb55q75EnqbCQU5bHNzvc474wfUfxe+Bz/N4oqT/TvcBne5ZbG3NduYYu2a8w398BbNfDcqF5dsyIJFGfgl8jkodXeYlvcbnxSKF7qnYXWVkod1hhXtdQUdQ2t++EfSDeDHIbc51UXV845wZ8Ewvg5Ft32GdLXlS4Aj0GrWDmK4EP8NB+AIPPWnUTUY0kyVegFrcHtd6wz06F72BpMN31vacv/hM+dbK1eUDg3J6d02ZFhyHa5nc0ss5MzXQHFwIDAQAB");

            services.AddSingleton<IAuthorizationPolicyProvider, ScopePolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, ScopePermission>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyWebApi", Version = "v1" });
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Type = SecuritySchemeType.Http,
                        Scheme = JwtBearerDefaults.AuthenticationScheme
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = JwtBearerDefaults.AuthenticationScheme,
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                });
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("allow-spa");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyWebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}