﻿namespace AMS.Services
{
    using AMS.Data;
    using AMS.Data.Models;
    using AMS.Services.Contracts;
    using AMS.Services.Models.Bids;
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

        public ICollection<Models.Listings.ListingsServiceModel> AllForAuction(string auctionId)
        {
            var listings = dbContext
                .Vehicles
                .Where(v => v.AuctionId == auctionId)
                .Select(v => new ListingsServiceModel
                {
                    Id = v.Id,
                    Year = v.Year,
                    Description = v.Description,
                    ImageUrl = v.ImageUrl,
                    Price = v.Price,
                    Make = v.Model.Make.Name,
                    Model = v.Model.Name,
                    CreatorId = v.UserId,
                    CreatorName = v.User.UserName,
                    Bids = v.Bids.Select(b => new BidServiceModel {
                        Number = b.Number,
                        Amount = b.Amount,
                        User = v.User.UserName
                    })
                    .ToArray()
                })
                .ToList();

            return listings;
        }

        public IEnumerable<ListingPropertyServiceModel> Conditions()
            => dbContext
            .Conditions
            .Select(x => new ListingPropertyServiceModel
            {
                Id = x.Id,
                Name = x.Name 
            })
            .ToList();

        public IEnumerable<ListingPropertyServiceModel> Makes()
            => dbContext
            .Makes
            .Select(x => new ListingPropertyServiceModel
            {
                Id = x.Id,
                Name= x.Name
            })
            .ToList();

        public IEnumerable<ListingPropertyServiceModel> Models()
            => dbContext
            .Models
            .Select(x => new ListingPropertyServiceModel
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToList();

        public IEnumerable<ListingPropertyServiceModel> Types()
            => dbContext
            .VehicleTypes
            .Select(x => new ListingPropertyServiceModel
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToList();

        public int Count()
            => dbContext.Vehicles.Count();
    }
}
