namespace AMS.Services.Contracts
{
    using AMS.Services.Models.Auctions;
    using AMS.Services.Models.Listings;
    using AMS.Services.Models.Listings.Base;

    public interface IListingService
    {
        public void Create(int year,
            decimal price,
            string description,
            string imageUrl,
            int conditionId,
            string auctionId,
            int modelId,
            string userId);

        public int Count(string auctionId);
        public IEnumerable<ListingsServiceModel> ApprovedPerPage(string Id, int currentPage, int listingsPerPage);
        public ListingDetailsServiceModel Details(string id);
        public IEnumerable<ListingPropertyServiceModel> Conditions();
        public IEnumerable<ListingPropertyServiceModel> Types();
        public IEnumerable<ListingPropertyServiceModel> Makes();
        public IEnumerable<ListingPropertyServiceModel> Models();
        public IEnumerable<AdminListingsServiceModel> NotApproved();
        public IEnumerable<SearchListingsServiceModel> Search(string searchString);
        public bool Delete(string Id);
        public bool Approve(string Id);
    }
}
