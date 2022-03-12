namespace AMS.Services
{
    using AMS.Data;
    using AMS.Data.Models;
    using AMS.Services.Contracts;
    using AMS.Services.Models.Listings;
    using System.Collections.Generic;

    public class ListingService : IListingService
    {
        private readonly AMSDbContext dbContext;

        public ListingService(AMSDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Create(int year,
            decimal price,
            string description,
            string imageUrl,
            int conditionId,
            string auctionId,
            int modelId,
            string userId)
        {
            var vehicle = new Vehicle
            {
                Year = year,
                Description = description,
                ImageUrl = imageUrl,
                Price = price,
                AuctionId = auctionId,
                UserId = userId,
                ConditionId = conditionId,
                ModelId = modelId
            };

            dbContext.Vehicles.Add(vehicle);
            dbContext.SaveChanges();
        }

        public IEnumerable<ListingsConditionsServiceModel> Conditions()
            => dbContext
            .Conditions
            .Select(x => new ListingsConditionsServiceModel
            {
                Id = x.Id,
                Name = x.Name 
            })
            .ToList();

        public IEnumerable<ListingsMakesServiceModel> Makes()
            => dbContext
            .Makes
            .Select(x => new ListingsMakesServiceModel
            {
                Id = x.Id,
                Name= x.Name
            })
            .ToList();

        public IEnumerable<ListingsModelsServiceModel> Models()
            => dbContext
            .Models
            .Select(x => new ListingsModelsServiceModel
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToList();

        public IEnumerable<ListingsTypesServiceModel> Types()
            => dbContext
            .VehicleTypes
            .Select(x => new ListingsTypesServiceModel
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToList();
    }
}
