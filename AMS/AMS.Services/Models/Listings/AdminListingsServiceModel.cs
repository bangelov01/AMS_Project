namespace AMS.Services.Models.Listings
{
    using AMS.Services.Models.Listings.Base;

    public class AdminListingsServiceModel : ListingsServiceModel
    {
        public string Id { get; init; }

        public int BidsCount { get; init; }

        public bool IsUserSuspended { get; init; }
    }
}
