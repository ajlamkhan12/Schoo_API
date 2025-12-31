using School_IServices;
using School_Services;

namespace School_Management_System.Infrastructure
{
    public static class DependancyInjectionConfiguration
    {
        public static void AddDIContainerService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRazorPages();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICommunicationService, CommunicationService>();
            services.AddScoped<IChatService, ChatService>();

        }
    }
}
