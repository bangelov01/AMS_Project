namespace AMS.Web.Infrastrucutre
{
    using AMS.Data;
    using Microsoft.EntityFrameworkCore;

    public static class WebApplicationExtensions
    {
        public static WebApplication PrepareDatabase(this WebApplication app)
        {
            using var scopedServices = app.Services.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<AMSDbContext>();

            data.Database.Migrate();

            return app;
        }
    }
}
