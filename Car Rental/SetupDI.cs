using CarRental.BL.BL;
using CarRental.BL.Interfaces;

namespace Car_Rental
{
    public static class SetupDI
    {
        public static void RegisterComponents(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddScoped<IBackOffice, BackOfficeBL>();
            services.AddScoped<ICarRental, CarRentalBL>();
        }
    }
}
