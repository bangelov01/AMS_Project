namespace AMS.Controllers.Models
{
    using AMS.Services.Models.Auctions;

    public class AllAuctionsViewModel
    {
        public ICollection<AllAuctionsServiceModel> Auctions { get; init; }

        public int CurrentPage { get; init; }

        public int TotalAuctions { get; init; }
    }
}
