namespace AMS.Services.Models.Listings
{
    using AMS.Services.Models.Bids;

    public class ListingsServiceModel
    {
        public string Id { get; init; }

        public int Year { get; init; }

        public string Make { get; init; }

        public string Model { get; init; }

        public string Description { get; init; }

        public string ImageUrl { get; init; }

        public decimal Price { get; init; }

        public string CreatorId { get; init; }

        public string CreatorName { get; init; }

        public ICollection<BidServiceModel> Bids { get; init; }
    }
}
