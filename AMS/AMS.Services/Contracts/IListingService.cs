namespace AMS.Services.Contracts
{
    using AMS.Services.Models.Listings;
    using AMS.Services.Models.Listings.Base;

    public interface IListingService
    {
        public Task Create(int year,
            decimal price,
            string description,
            string imageUrl,
            int conditionId,
            string auctionId,
            int modelId,
            string userId);
        public Task<IEnumerable<ListingsServiceModel>> Preview();
        public Task<ListingDetailsServiceModel> Details(string listingId, string userId);
        public Task<IEnumerable<ListingsServiceModel>> ApprovedPerPage(string Id, int currentPage, int listingsPerPage);
        public Task<IEnumerable<SearchListingsServiceModel>> Search(string searchString);
        public Task<IEnumerable<AdminListingsServiceModel>> NotApproved();
        public Task<ICollection<ListingPropertyServiceModel>> Conditions();
        public Task<ICollection<ListingPropertyServiceModel>> Types();
        public Task<ICollection<ListingPropertyServiceModel>> Makes();
        public Task<ICollection<ListingPropertyServiceModel>> Models();
        public Task<bool> IsWatched(string listingId, string userId);
        public Task<int> Count(string auctionId);
        public Task<bool> Delete(string Id);
        public Task<bool> Approve(string Id);
    }
}
