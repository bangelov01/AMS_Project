namespace AMS.Web.Infrastrucutre.Extensions
{
    using Microsoft.EntityFrameworkCore;

    using AMS.Data;
    using AMS.Services.Contracts;
    using AMS.Services;

    public static class WebApplicationExtensions
    {
        public static WebApplication PrepareDatabase(this WebApplication app)
        {
            using var scopedServices = app.Services.CreateScope();

            var serviceProvider = scopedServices.ServiceProvider;
           
            Migrate(serviceProvider);

            Seed(serviceProvider);

            return app;
        }

        public static WebApplicationBuilder AddTransient(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IDataSeederService, DataSeederService>();
            builder.Services.AddTransient<IAuctionService, AuctionService>();
            builder.Services.AddTransient<IAddressService, AddressService>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IListingService, ListingService>();
            builder.Services.AddTransient<IValidatorService, ValidatorService>();
            builder.Services.AddTransient<IBidService, BidService>();

            return builder;
        }

        private static void Migrate(IServiceProvider provider)
        {
            var data = provider.GetRequiredService<AMSDbContext>();

            data.Database.Migrate();
        }

        private static void Seed(IServiceProvider provider)
        {
            var seeder = provider.GetRequiredService<IDataSeederService>();

            seeder.SeedConditions();

            seeder.SeedVehicleTypes();

            seeder.SeedAdministrator();

            seeder.SeedMakes();

            seeder.SeedModels();
        }
    }
}
