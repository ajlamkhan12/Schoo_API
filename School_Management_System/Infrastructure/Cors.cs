namespace School_Management_System.Infrastructure
{
    public static class Cors
    {
        public static void AddCorsService(this IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("cors",
                    p => p.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials()
                          .SetIsOriginAllowed(_ => true));
            });

        }
    }
}
