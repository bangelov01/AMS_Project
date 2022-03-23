namespace AMS.Services
{
    using AMS.Data;
    using AMS.Data.Models;
    using AMS.Services.Contracts;
    using AMS.Services.Models.Listings;
    using System.Collections.Generic;

    public class WatchlistService : IWatchlistService
    {
        private readonly AMSDbContext dbContext;

        public WatchlistService(AMSDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Create(string listingId, string userId)
        {
            var watchlist = new Watchlist
            {
                VehicleId = listingId,
                UserId = userId,
            };

            dbContext.Watchlists.Add(watchlist);
            dbContext.SaveChanges();
        }

        public bool Delete(string listingId, string userId)
        {
            var watch = dbContext
                .Watchlists
                .Where(w => w.VehicleId == listingId && w.UserId == userId)
                .FirstOrDefault();

            if (watch == null)
            {
                return false;
            }

            dbContext.Watchlists.Remove(watch);
            dbContext.SaveChanges();

            return true;
        }

        public IEnumerable<SearchListingsServiceModel> ListingsForUser(string Id)
        {
            var listings = dbContext
                .Watchlists
                .Where(w => w.UserId == Id && w.Vehicle.Auction.End > DateTime.UtcNow)

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
                .ToArray();

            return listings;
        }
    }
}
