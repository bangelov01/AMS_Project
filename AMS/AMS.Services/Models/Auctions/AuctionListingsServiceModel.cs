namespace AMS.Services.Models.Auctions
{
    using AMS.Services.Models.Auctions.Base;
    using AMS.Services.Models.Listings;

    public class AuctionListingsServiceModel : AuctionServiceModel
    {
        public ICollection<AllListingsServiceModel> Listings { get; init; }
    }
}
