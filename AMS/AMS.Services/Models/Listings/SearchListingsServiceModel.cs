namespace AMS.Services.Models.Listings
{
    using AMS.Services.Models.Listings.Base;

    public class SearchListingsServiceModel : ListingsServiceModel
    {
        public string AuctionId { get; init; }
        public int AuctionNumber { get; init; }
        public DateTime End { get; init; }
    }
}
