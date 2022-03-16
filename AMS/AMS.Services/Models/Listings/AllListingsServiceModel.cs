namespace AMS.Services.Models.Listings
{
    using AMS.Services.Models.Listings.Base;

    public class AllListingsServiceModel : ListingsServiceModel
    {
        public string Id { get; init; }

        public string Description { get; init; }
    }
}
