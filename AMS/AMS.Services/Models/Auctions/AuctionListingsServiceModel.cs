namespace AMS.Services.Models.Auctions
{
    using AMS.Services.Models.Listings;

    public class AuctionListingsServiceModel
    {
        public int Number { get; init; }

        public DateTime Start { get; init; }

        public DateTime End { get; init; }

        public string City { get; init; }

        public ICollection<ListingsServiceModel> Listings { get; init; }
    }
}
