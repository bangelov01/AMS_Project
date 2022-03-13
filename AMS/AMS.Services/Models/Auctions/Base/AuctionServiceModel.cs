namespace AMS.Services.Models.Base.Auctions
{
    using AMS.Services.Models.Auctions.Contracts;

    public class AuctionServiceModel : IAuctionModel
    {
        public int Number { get; init; }

        public DateTime Start { get; init; }

        public DateTime End { get; init; }

        public string City { get; init; }

        public string Description { get; init; }

        public string Country { get; init; }
    }
}
