namespace AMS.Web.Infrastrucutre.Extensions
{
    using Microsoft.EntityFrameworkCore;

    using AMS.Data;

    using AMS.Services.Contracts;
    using AMS.Services;

    public static class WebApplicationExtensions
    {
        public static async Task PrepareDatabase(this WebApplication app)
        {
            await using var scopedServices = app.Services.CreateAsyncScope();

            var serviceProvider = scopedServices.ServiceProvider;
           
            await Migrate(serviceProvider);

            await Seed(serviceProvider);
        }

        public static void AddTransient(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IDataSeederService, DataSeederService>();
            builder.Services.AddTransient<IAuctionService, AuctionService>();
            builder.Services.AddTransient<IAddressService, AddressService>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IListingService, ListingService>();
            builder.Services.AddTransient<IValidatorService, ValidatorService>();
            builder.Services.AddTransient<IBidService, BidService>();
            builder.Services.AddTransient<IWatchlistService, WatchlistService>();
            builder.Services.AddTransient<IStatisticService, StatisticsService>();
        }

        private static async Task Migrate(IServiceProvider provider)
        {
            var data = provider.GetRequiredService<AMSDbContext>();

            await data.Database.MigrateAsync();
        }

        private static async Task Seed(IServiceProvider provider)
        {
            var seeder = provider.GetRequiredService<IDataSeederService>();

            await seeder.SeedConditions();

            await seeder.SeedVehicleTypes();

            await seeder.SeedAdministrator();

            await seeder.SeedMakes();

            await seeder.SeedModels();
        }
    }
}
