namespace AMS.Services.Contracts
{
    using AMS.Services.Models.Listings;

    public interface IListingService
    {
        public ICollection<ListingConditionsServiceModel> AllConditions();
        public ICollection<ListingTypesServiceModel> AllTypes();
    }
}
