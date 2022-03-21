namespace AMS.Controllers.Models
{
    using AMS.Services.Models.Auctions.Base;
    using AMS.Services.Models.Bids;
    using AMS.Services.Models.Listings;

    public class ListingViewModel
    {
        public string AuctionId { get; init; }
        public AuctionServiceModel Auction { get; init; }
        public ListingDetailsServiceModel Listing { get; init; }
    }
}
