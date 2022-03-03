namespace AMS.Web.Infrastrucutre
{
    using Microsoft.EntityFrameworkCore;

    using AMS.Data;
    using AMS.Services.Contracts;

    public static class WebApplicationExtensions
    {
        public static WebApplication PrepareDatabase(this WebApplication app)
        {
            using var scopedServices = app.Services.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<AMSDbContext>();

            var seeder = scopedServices.ServiceProvider.GetService<IDataSeederService>();

            data.Database.Migrate();

            seeder.SeedConditions();

            return app;
        }
    }
}
