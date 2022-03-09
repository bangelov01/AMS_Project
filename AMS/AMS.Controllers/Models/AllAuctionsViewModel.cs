namespace AMS.Controllers.Models
{
    using AMS.Services.Models;

    public class AllAuctionsViewModel
    {
        public ICollection<AuctionServiceModel> Auctions { get; init; }

        public int CurrentPage { get; init; }

        public int TotalAuctions { get; init; }
    }
}
