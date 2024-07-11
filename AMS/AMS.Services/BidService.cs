namespace AMS.Services
{
    using Microsoft.EntityFrameworkCore;

    using AMS.Data;
    using AMS.Data.Models;

    using AMS.Services.Contracts;
    using AMS.Services.Models.Bids;

    public class BidService(AMSDbContext dbContext) : IBidService
    {
        public async Task Create(string userId,
            string listingId,
            decimal amount,
            int number)
        {
            var bid = new Bid
            {
                UserId = userId,
                Amount = amount,
                VehicleId = listingId,
                Number = number
            };

            await dbContext.Bids.AddAsync(bid);
            await dbContext.SaveChangesAsync();
        }

        public async Task<BidServiceModel> HighestForListing(string Id)
            => await dbContext
            .Bids
            .Where(b => b.VehicleId == Id)
            .OrderByDescending(b => b.Amount)
            .Take(1)
            .Select(b => new BidServiceModel
            {
                Amount = b.Amount,
                User = b.User.UserName,
            })
            .FirstOrDefaultAsync();
    }
}
