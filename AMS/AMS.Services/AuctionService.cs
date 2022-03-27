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

        public async Task Create(int number,
            string description,
            DateTime start,
            DateTime end,
            string country,
            string city,
            string addressText)
        {
            var addressId = await addressService.GetId(country, city, addressText);

            if (addressId == null)
            {
                var newAddress = await addressService.Add(country, city, addressText);

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

            await dbContext.Auctions.AddAsync(auction);
            await dbContext.SaveChangesAsync();
        }

        public async Task Edit(string Id,
            int number,
            string description,
            DateTime start,
            DateTime end,
            string country,
            string city,
            string addressText)
        {
            var auction = await dbContext
                .Auctions
                .Include(a => a.Address)
                .FirstOrDefaultAsync(x => x.Id == Id);

            auction.Number = number;
            auction.Description = description;
            auction.Start = start;
            auction.End = end;
            auction.Address.City = city;
            auction.Address.Country = country;
            auction.Address.AddressText = addressText;

            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AllAuctionsServiceModel>> All()
            => await dbContext
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
                .ToArrayAsync();

        public async Task<IEnumerable<AllAuctionsServiceModel>> ActivePerPage(int currentPage, int auctionsPerPage)
            => await dbContext
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
                .ToArrayAsync();

        public async Task<bool> IsCreated(int number)
        {
            var auction = await dbContext
                .Auctions
                .Where(a => a.Number == number)
                .FirstOrDefaultAsync();

            if (auction == null)
            {
                return false;
            }

            return true;
        }

        public async Task<AuctionServiceModel> DetailsById(string Id)
            => await dbContext
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
            .FirstOrDefaultAsync();

        public async Task<AdminEditServiceModel> AdminDetailsById(string Id)
            => await dbContext
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
                .FirstOrDefaultAsync();

        public async Task<int> ActiveCount()
            => await dbContext
                .Auctions
                .Where(a => a.End > GetCurrentDate())
                .CountAsync();
    }
}
