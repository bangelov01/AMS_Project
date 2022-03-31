namespace AMS.Controllers.Models
{
    using AMS.Services.Models;
    using AMS.Services.Models.Listings.Base;

    public class HomeViewModel
    {
        public StatisticsServiceModel Statistics { get; init; }
        public IEnumerable<ListingsServiceModel> Preview { get; init; }
    }
}
