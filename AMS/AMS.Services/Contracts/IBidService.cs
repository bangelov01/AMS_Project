namespace AMS.Services.Contracts
{
    using AMS.Services.Models.Bids;

    public interface IBidService
    {
        public void Create(string userId,
            string listingId,
            decimal amount,
            int number);
        public BidServiceModel HighestForListing(string Id);
    }
}
