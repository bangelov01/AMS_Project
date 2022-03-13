namespace AMS.Services.Models.Auctions
{
    using AMS.Services.Models.Base.Auctions;

    public class AllAuctionsServiceModel : AuctionServiceModel
    {
        public string Id { get; init; }

        public int ListingsCount { get; init; }
    }
}
