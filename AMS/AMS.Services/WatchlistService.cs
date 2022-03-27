namespace AMS.Services
{
    using Microsoft.EntityFrameworkCore;

    using System.Collections.Generic;

    using AMS.Data;
    using AMS.Data.Models;

    using AMS.Services.Contracts;
    using AMS.Services.Models.Listings;

    using static AMS.Services.Common.CommonFunctions;

    public class WatchlistService : IWatchlistService
    {
        private readonly AMSDbContext dbContext;

        public WatchlistService(AMSDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Create(string listingId, string userId)
        {
            var watchlist = new Watchlist
            {
                VehicleId = listingId,
                UserId = userId,
            };

            await dbContext.Watchlists.AddAsync(watchlist);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> Delete(string listingId, string userId)
        {
            var watch = await dbContext
                .Watchlists
                .Where(w => w.VehicleId == listingId && w.UserId == userId)
                .FirstOrDefaultAsync();

            if (watch == null)
            {
                return false;
            }

            dbContext.Watchlists.Remove(watch);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<SearchListingsServiceModel>> ListingsForUser(string Id)
            => await dbContext
                .Watchlists
                .Where(w => w.UserId == Id && w.Vehicle.Auction.End > GetCurrentDate())
                .Select(w => new SearchListingsServiceModel
                {
                    AuctionId = w.Vehicle.AuctionId,
                    AuctionNumber = w.Vehicle.Auction.Number,
                    Id = w.Vehicle.Id,
                    Condition = w.Vehicle.Condition.Name,
                    Make = w.Vehicle.Model.Make.Name,
                    Model = w.Vehicle.Model.Name,
                    ImageUrl = w.Vehicle.ImageUrl,
                    Year = w.Vehicle.Year,
                })
                .ToArrayAsync();
    }
}
