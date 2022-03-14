namespace AMS.Services.Models.Base.Auctions
{
    using AMS.Services.Models.Contracts;

    public class AuctionServiceModel : IAuctionModel
    {
        public int Number { get; init; }

        public DateTime Start { get; init; }

        public DateTime End { get; init; }

        public string City { get; init; }
    }
}
