namespace CarsProject.WebApi.Extensions
{
    public static class CarsProjectExtensions
    {
        public static void AddCarsProjectCORS(this IServiceCollection Services)
        {
            Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                );

            });
        }
    }
}


    
    
