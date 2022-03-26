namespace AMS.Services.Models.Auctions.Base
{
    using AMS.Services.Models.Contracts;

    public class AuctionServiceModel : IAuctionModel
    {
        public string Id { get; init; }

        public int Number { get; init; }

        public DateTime Start { get; init; }

        public DateTime End { get; init; }

        public string City { get; init; }
    }
}
