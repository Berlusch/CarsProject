namespace CarsProject.Extensions
{
    public static class CarsProjectExtensions
    {
        public static void AddCarsProjectCORS(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                );
            });
        }
    }
}


    
    
