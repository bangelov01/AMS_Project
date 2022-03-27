namespace AMS.Services.Contracts
{
    using AMS.Services.Models.Listings;

    public interface IWatchlistService
    {
        public Task Create(string listingId, string userId);

        public Task<bool> Delete(string listingId, string userId);

        public Task<IEnumerable<SearchListingsServiceModel>> ListingsForUser(string Id);
    }
}
