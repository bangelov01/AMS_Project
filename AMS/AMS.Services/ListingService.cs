namespace AMS.Services
{
    using System.Collections.Generic;

    using AMS.Data;
    using AMS.Services.Contracts;
    using AMS.Services.Models.Listings;

    public class ListingService : IListingService
    {
        private readonly AMSDbContext dbContext;

        public ListingService(AMSDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ICollection<ListingConditionsServiceModel> AllConditions()
            => dbContext
                .Conditions
                .Select(c => new ListingConditionsServiceModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();

        public ICollection<ListingTypesServiceModel> AllTypes()
            => dbContext
                .VehicleTypes
                .Select(c => new ListingTypesServiceModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
    }
}
