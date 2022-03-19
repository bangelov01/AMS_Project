namespace AMS.Controllers.Models
{
    using AMS.Services.Models.Auctions.Base;
    using AMS.Services.Models.Listings.Base;

    public class AllListingsViewModel
    {
        public IEnumerable<ListingsServiceModel> Listings { get; init; }

        public AuctionServiceModel Auction { get; init; }

        public PaginationViewModel Pagination { get; init; }

    }
}
