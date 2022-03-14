namespace AMS.Services.Models.Auctions
{
    using AMS.Services.Models.Base.Auctions;

    public class AdminEditServiceModel : AuctionServiceModel
    {
        public string Country { get; init; }

        public string Description { get; init; }

        public string AddressText { get; init; }
    }
}
