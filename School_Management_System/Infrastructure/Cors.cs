namespace School_Management_System.Infrastructure
{
    public static class Cors
    {
        public static void AddCorsService(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsApp", builder =>
                {
                    builder
                        .WithOrigins("*")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }

    }
}
