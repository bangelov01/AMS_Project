namespace AMS.Services.Models.Auctions
{
    using AMS.Services.Models.Auctions.Base;

    public class AllAuctionsServiceModel : AuctionServiceModel
    {
        public string Id { get; init; }

        public string Country { get; init; }

        public string Description { get; init; }

        public int ListingsCount { get; init; }
    }
}
