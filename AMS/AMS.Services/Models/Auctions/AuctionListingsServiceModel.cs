namespace AMS.Services.Models.Auctions
{
    using AMS.Services.Models.Base.Auctions;
    using AMS.Services.Models.Listings;

    public class AuctionListingsServiceModel : AuctionServiceModel
    {
        public ICollection<AllListingsServiceModel> Listings { get; init; }
    }
}
