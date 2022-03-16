namespace AMS.Controllers.Models
{
    using AMS.Services.Models;
    using AMS.Services.Models.Listings;

    public class AllListingsViewModel
    {
        public ICollection<AllListingsServiceModel> Listings { get; init; }

        public PaginationViewModel Pagination { get; init; }

        public int Number { get; init; }

        public DateTime Start { get; init; }

        public DateTime End { get; init; }

        public string City { get; init; }
    }
}
