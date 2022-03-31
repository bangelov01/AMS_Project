namespace AMS.Controllers.Models.Listings
{
    using AMS.Services.Models.Listings;

    public class SearchListingsViewModel
    {
        public int ModelId { get; init; }

        public IEnumerable<ListingPropertyServiceModel> Models { get; init; }

        public IEnumerable<SearchListingsServiceModel> Listings { get; init; }

        public string SearchTerm { get; init; }
    }
}
