namespace AMS.Services
{
    using Microsoft.EntityFrameworkCore;

    using AMS.Data;

    using AMS.Services.Contracts;
    using AMS.Services.Models;

    public class StatisticsService(AMSDbContext dbContext) : IStatisticService
    {
        public async Task<StatisticsServiceModel> Total()
            => new StatisticsServiceModel
            {
                TotalAuctions = await dbContext.Auctions.CountAsync(),
                TotalListings = await dbContext.Vehicles.CountAsync(),
                TotalUsers = await dbContext.Users.Where(u => u.UserName != "admin").CountAsync()
            };
    }
}
