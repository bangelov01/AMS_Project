namespace AMS.Services
{
    using System.Linq;

    using AMS.Data;
    using AMS.Data.Models;
    using AMS.Services.Contracts;

    public class DataSeederService : IDataSeederService
    {
        private readonly AMSDbContext dbContext;

        public DataSeederService(AMSDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void SeedConditions()
        {
            if (dbContext.Conditions.Any())
            {
                return;
            }

            dbContext.Conditions.AddRange(new[]
            {
                new Condition { Name = "Used" },
                new Condition { Name = "Salvage" },
                new Condition { Name = "Damaged" }
            });

            dbContext.SaveChanges();
        }
    }
}
