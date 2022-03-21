namespace AMS.Services.Models.Listings
{
    using AMS.Services.Models.Listings.Base;

    public class ListingDetailsServiceModel : ListingsServiceModel
    {
        public string UserId { get; init; }

        public string Type { get; init; }

        public string Description { get; init; }

        public decimal Price { get; init; }
    }
}
