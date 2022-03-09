namespace AMS.Services.Models.Auctions
{
    public class AuctionServiceModel
    {
        public string Id { get; init; }

        public int Number { get; init; }

        public string Description { get; init; }

        public DateTime Start { get; init; }

        public DateTime End { get; init; }

        public string City { get; init; }

        public string Country { get; init; }

        public int ListingsCount { get; init; }
    }
}
