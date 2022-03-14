namespace AMS.Services.Models.Listings.Base
{
    using AMS.Services.Models.Contracts;

    public class ListingsServiceModel : IListingModel
    {
        public int Year { get; init; }

        public string Make { get; init; }

        public string Model { get; init; }

        public string Description { get; init; }

        public string CreatorName { get; init; }

        public string ImageUrl { get; init; }
    }
}
