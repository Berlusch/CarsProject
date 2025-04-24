namespace CarsProject.Extensions
{
    public static class CarsProjectExtensions
    {
        public static void AddCarsProjectCORS(this IServiceCollection Services)
        {
            Services.AddCors(opcije =>
            {
                opcije.AddPolicy("CorsPolicy",
                    builder =>
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                );

            });
        }
    }
}


    
    
