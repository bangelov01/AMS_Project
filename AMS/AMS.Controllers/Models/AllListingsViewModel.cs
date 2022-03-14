namespace AMS.Controllers.Models
{
    using AMS.Services.Models;
    using AMS.Services.Models.Listings;

    public class AllListingsViewModel
    {
        public ICollection<AllListingsServiceModel> Listings { get; init; }

        public string Id { get; init; }

        public int CurrentPage { get; init; }

        public int TotalListings { get; init; }

        public int Number { get; init; }

        public DateTime Start { get; init; }

        public DateTime End { get; init; }

        public string City { get; init; }
    }
}
