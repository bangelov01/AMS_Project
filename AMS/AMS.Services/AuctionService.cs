namespace AMS.Services
{
    using System;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using AMS.Data;
    using AMS.Data.Models;

    using AMS.Services.Contracts;
    using AMS.Services.Models.Auctions;
    using AMS.Services.Models.Auctions.Base;

    using static AMS.Services.Common.CommonFunctions;

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

        public IEnumerable<AllAuctionsServiceModel> All()
            => dbContext
                .Auctions
                .Select(a => new AllAuctionsServiceModel
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
                .ToArray();

        public IEnumerable<AllAuctionsServiceModel> ActivePerPage(int currentPage, int auctionsPerPage)
            => dbContext
                .Auctions
                .Where(a => a.End > GetCurrentDate())
                .Skip((currentPage - 1) * auctionsPerPage)
                .Take(auctionsPerPage)
                .Select(a => new AllAuctionsServiceModel
                {
                    Id = a.Id,
                    Number = a.Number,
                    Description = a.Description,
                    Start = a.Start,
                    End = a.End,
                    City = a.Address.City,
                    Country = a.Address.Country,
                    ListingsCount = a.Vehicles.Count(v => v.IsApproved == true)
                })
                .ToArray();

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

        public AdminEditServiceModel AdminDetailsById(string Id)
            => dbContext
                .Auctions
                .Where(a => a.Id == Id)
                .Select(a => new AdminEditServiceModel
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

        public int ActiveCount()
            => dbContext
                .Auctions
                .Where(a => a.End > GetCurrentDate())
                .Count();

        public AuctionServiceModel DetailsById(string Id)
            => dbContext
            .Auctions
            .Where(a => a.Id == Id && a.End > GetCurrentDate())
            .Select(a => new AuctionServiceModel
            {
                Id = a.Id,
                City = a.Address.City,
                Number = a.Number,
                Start = a.Start,
                End = a.End
            })
            .FirstOrDefault();
    }
}
