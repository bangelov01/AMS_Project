namespace AMS.Controllers.Models.Listings
{
    using AMS.Services.Models.Auctions.Base;
    using AMS.Services.Models.Bids;
    using AMS.Services.Models.Listings;

    public class ListingViewModel
    {
        public AuctionServiceModel Auction { get; init; }

        public string Id { get; init; }

        public string UserId { get; init; }

        public string ImageUrl { get; init; }

        public string Type { get; init; }

        public string Condition { get; init; }

        public string Make { get; init; }

        public string Model { get; init; }

        public string Price { get; init; }

        public int Year { get; init; }

        public string Description { get; init; }

        public string BidUser { get; init; }

        public string BidAmount { get; init; }

        public bool IsWatched { get; init; }
    }
}
