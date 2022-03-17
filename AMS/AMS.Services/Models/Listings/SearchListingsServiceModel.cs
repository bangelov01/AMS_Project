namespace AMS.Services.Models.Listings
{
    public class SearchListingsServiceModel : AllListingsServiceModel
    {
        public string AuctionId { get; init; }
        public int AuctionNumber { get; init; }
    }
}
