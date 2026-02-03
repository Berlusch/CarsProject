namespace CarsProject.WebApi.Extensions
{
    public static class CarsProjectExtensions
    {
        public static void AddCarsProjectCORS(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowViteDev", builder =>
                {
                    builder
                        .WithOrigins("http://localhost:5173") 
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }
    }
}




