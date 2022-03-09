namespace AMS.Services
{
    using System;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using AMS.Data;
    using AMS.Data.Models;
    using AMS.Services.Contracts;
    using AMS.Services.Models.Auctions;

    public class AuctionService : IAuctionService
    {
        private readonly AMSDbContext dbContext;
        private readonly IAddressService addressService;

        public AuctionService(AMSDbContext dbContext,
            IAddressService addressService)
        {
            this.dbContext = dbContext;
            this.addressService = addressService;
        }

        public void Create(int number,
            string description,
            DateTime start,
            DateTime end,
            string country,
            string city,
            string addressText)
        {
            var addressId = addressService.GetId(country, city, addressText);

            if (addressId == null)
            {
                var newAddress = addressService.Add(country, city, addressText);

                addressId = newAddress;
            }

            var auction = new Auction
            {
                Number = number,
                Description = description,
                Start = start,
                End = end,
                AddressId = addressId
            };

            dbContext.Auctions.Add(auction);
            dbContext.SaveChanges();
        }

        public ICollection<AuctionServiceModel> AllActive()
            => dbContext
                .Auctions
                .Where(a => a.End > DateTime.UtcNow)
                .Select(a => new AuctionServiceModel
                {
                    Id = a.Id,
                    Number = a.Number,
                    Description = a.Description,
                    Start = a.Start,
                    End = a.End,
                    City = a.Address.City,
                    Country = a.Address.Country,
                    ListingsCount = a.Vehicles.Count
                })
                .ToList();

        public ICollection<AuctionServiceModel> AllActivePerPage(int currentPage, int auctionsPerPage)
            => dbContext
                .Auctions
                .Where(a => a.End > DateTime.UtcNow)
                .Skip((currentPage - 1) * auctionsPerPage)
                .Take(auctionsPerPage)
                .Select(a => new AuctionServiceModel
                {
                    Id = a.Id,
                    Number = a.Number,
                    Description = a.Description,
                    Start = a.Start,
                    End = a.End,
                    City = a.Address.City,
                    Country = a.Address.Country,
                    ListingsCount = a.Vehicles.Count
                })
                .ToList();

        public bool IsCreated(int number)
        {
            var auction = dbContext
                .Auctions
                .Where(a => a.Number == number)
                .FirstOrDefault();

            if (auction == null)
            {
                return false;
            }

            return true;
        }

        public AuctionDetailsServiceModel ById(string Id)
        {
            var auction = dbContext
                .Auctions
                .Where(a => a.Id == Id)
                .Select(a => new AuctionDetailsServiceModel
                {
                    Number = a.Number,
                    Description = a.Description,
                    Start = a.Start,
                    End = a.End,
                    Country = a.Address.Country,
                    City = a.Address.City,
                    AddressText = a.Address.AddressText
                })
                .FirstOrDefault();

            return auction;
        }

        public void Edit(string Id,
            int number,
            string description,
            DateTime start,
            DateTime end,
            string country,
            string city,
            string addressText)
        {
            var auction = dbContext
                .Auctions
                .Include(a => a.Address)
                .FirstOrDefault(x => x.Id == Id);

            auction.Number = number;
            auction.Description = description;
            auction.Start = start;
            auction.End = end;
            auction.Address.City = city;
            auction.Address.Country = country;
            auction.Address.AddressText = addressText;

            dbContext.SaveChanges();
        }

        public int AllActiveCount()
            => dbContext
                .Auctions
                .Where(a => a.End > DateTime.UtcNow)
                .Count();
    }
}
