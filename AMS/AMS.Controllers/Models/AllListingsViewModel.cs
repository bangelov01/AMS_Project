namespace AMS.Controllers.Models
{
    using AMS.Services.Models.Auctions.Base;
    using AMS.Services.Models.Listings;

    public class AllListingsViewModel
    {
        public ICollection<AllListingsServiceModel> Listings { get; init; }

        public AuctionServiceModel Auction { get; init; }

        public PaginationViewModel Pagination { get; init; }

    }
}
