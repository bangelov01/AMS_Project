namespace AMS.Controllers.Models
{
    using AMS.Services.Models.Listings;

    public class AllListingsViewModel
    {
        public ICollection<ListingsServiceModel> Listings { get; init; }

        public int CurrentPage { get; init; }

        public int TotalListings { get; init; }
    }
}
