namespace AMS.Services
{
    using AMS.Data;
    using AMS.Data.Models;
    using AMS.Services.Contracts;
    using AMS.Services.Models.Bids;

    public class BidService : IBidService
    {
        private readonly AMSDbContext dbContext;

        public BidService(AMSDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Create(string userId,
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

            dbContext.Bids.Add(bid);
            dbContext.SaveChanges();
        }

        public BidServiceModel HighestForListing(string Id)
            => dbContext
            .Bids
            .Where(b => b.VehicleId == Id)
            .OrderByDescending(b => b.Amount)
            .Take(1)
            .Select(b => new BidServiceModel
            {
                Amount = b.Amount,
                User = b.User.UserName,
            })
            .FirstOrDefault();
    }
}
