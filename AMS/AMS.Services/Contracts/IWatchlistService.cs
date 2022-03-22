namespace AMS.Services.Contracts
{
    using AMS.Services.Models.Listings;

    public interface IWatchlistService
    {
        public void Create(string listingId, string userId);

        public IEnumerable<SearchListingsServiceModel> ListingsForUser(string Id);
    }
}
