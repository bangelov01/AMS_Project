namespace AMS.Services.Contracts
{
    using AMS.Services.Models.Bids;

    public interface IBidService
    {
        public Task Create(string userId,
            string listingId,
            decimal amount,
            int number);
        public Task<BidServiceModel> HighestForListing(string Id);
    }
}
