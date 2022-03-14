namespace AMS.Services.Models.Listings
{
    using AMS.Services.Models.Bids;
    using AMS.Services.Models.Listings.Base;

    public class AllListingsServiceModel : ListingsServiceModel
    {
        public string Id { get; init; }

        public decimal Price { get; init; }

        public ICollection<BidServiceModel> Bids { get; init; }
    }
}
