namespace AMS.Services.Models
{
    public class AuctionServiceModel
    {
        public string Id { get; init; }

        public int Number { get; init; }

        public DateTime Start { get; init; }

        public DateTime End { get; init; }

        public string City { get; init; }

        public int ListingsCount { get; init; }
    }
}
