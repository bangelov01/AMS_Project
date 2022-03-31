namespace AMS.Controllers.Models.Listings
{
    using AMS.Services.Models.Auctions.Base;
    using AMS.Services.Models.Bids;
    using AMS.Services.Models.Listings;

    public class ListingViewModel
    {
        public AuctionServiceModel Auction { get; init; }
        public ListingDetailsServiceModel Listing { get; init; }
        public BidServiceModel Bid { get; init; }
        public bool IsWatched { get; init; }
    }
}
