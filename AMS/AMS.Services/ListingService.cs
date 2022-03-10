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

        public ICollection<ListingMakeServiceModel> MakesByType(int Id)
            => dbContext
                .MakeVehicleTypes
                .Where(t => t.VehicleTypeId == Id)
                .Select(t => new ListingMakeServiceModel
                {
                    Id = t.Make.Id,
                    Name = t.Make.Name
                })
                .ToList();

        public ICollection<ListingModelServiceModel> ModelsByMake(int Id)
            => dbContext
                 .Models
                 .Where(m => m.MakeId == Id)
                 .Select(m => new ListingModelServiceModel
                 {
                     Id = m.Id,
                     Name= m.Name
                 })
                .ToList();
    }
}
