namespace AMS.Services.Models.Listings
{
    using AMS.Services.Models.Bids;
    using AMS.Services.Models.Listings.Base;

    public class MyListingsServiceModel : ListingsServiceModel
    {
        public decimal Price { get; init; }

        public BidServiceModel Bid { get; init; }

        public int BidsCount { get; init; }

        public DateTime Start { get; init; }

        public DateTime End { get; init; }
    }
}
