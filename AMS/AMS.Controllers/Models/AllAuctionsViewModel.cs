namespace AMS.Controllers.Models
{
    using AMS.Services.Models.Auctions;

    public class AllAuctionsViewModel
    {
        public ICollection<AllAuctionsServiceModel> Auctions { get; init; }

        public PaginationViewModel Pagination { get; init; }

        public int Total { get; init; }
    }
}
