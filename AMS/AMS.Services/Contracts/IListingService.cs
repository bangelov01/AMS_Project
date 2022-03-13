namespace AMS.Services.Contracts
{
    using AMS.Services.Models.Listings;

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

        public int Count();
        public ICollection<ListingsServiceModel> AllForAuction(string auctionId);
        public IEnumerable<ListingsConditionsServiceModel> Conditions();
        public IEnumerable<ListingsTypesServiceModel> Types();
        public IEnumerable<ListingsMakesServiceModel> Makes();
        public IEnumerable<ListingsModelsServiceModel> Models();
    }
}
