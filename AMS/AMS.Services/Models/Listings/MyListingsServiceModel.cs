namespace AMS.Services.Models.Listings
{
    using AMS.Services.Models.Listings.Base;

    public class MyListingsServiceModel : ListingsServiceModel
    {
        public decimal Price { get; init; }

        public bool IsApproved { get; init; }

        public decimal HighestBid { get; init; }

        public DateTime Start { get; init; }

        public DateTime End { get; init; }
    }
}
