using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace School_Management_System.Infrastructure
{
    public static class Configuration
    {
        public static void ConfigurationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //var appSettingsSection = configuration.GetSection("AppSettings");
            //services.Configure<AppSettings>(appSettingsSection);

            //var appSettings = appSettingsSection.Get<AppSettings>();

            services.AddCorsService();
            //services.ConfigureJwtToken(appSettings);
            services.AddDIContainerService(configuration);
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            object value = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuer = true,
                           ValidateAudience = true,
                           ValidateLifetime = true,
                           ValidateIssuerSigningKey = true,
                   
                           ValidIssuer = configuration["Jwt:Issuer"],
                           ValidAudience = configuration["Jwt:Audience"],
                           IssuerSigningKey = new SymmetricSecurityKey(
                               Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
                           ),
                           ClockSkew = TimeSpan.Zero
                       };
                   });
                           services.AddControllers();
                           services.AddAuthorization();
                   
        }
    }
}
