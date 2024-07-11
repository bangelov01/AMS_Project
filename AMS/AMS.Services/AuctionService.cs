namespace AMS.Services
{
    using System;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using AMS.Data;
    using AMS.Data.Models;

    using AMS.Services.Contracts;
    using AMS.Services.Models.Auctions;
    using AMS.Services.Models.Auctions.Base;

    using static AMS.Services.Common.CommonFunctions;

    public class AuctionService(
        AMSDbContext dbContext,
        IAddressService addressService,
        IMapper mapper)
        : IAuctionService
    {
        private readonly IConfigurationProvider mapper = mapper.ConfigurationProvider;

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
                .ProjectTo<AllAuctionsServiceModel>(mapper)
                .ToArrayAsync();

        public async Task<IEnumerable<AllAuctionsServiceModel>> ActivePerPage(int currentPage, int auctionsPerPage)
            => await dbContext
                .Auctions
                .Where(a => a.End > GetCurrentDate())
                .Skip((currentPage - 1) * auctionsPerPage)
                .Take(auctionsPerPage)
                .ProjectTo<AllAuctionsServiceModel>(mapper)
                .ToArrayAsync();

        public async Task<bool> IsCreated(int number)
        {
            var auction = await dbContext
                .Auctions
                .Where(a => a.Number == number)
                .FirstOrDefaultAsync();

            return auction != null;
        }

        public async Task<AuctionServiceModel> DetailsById(string Id)
            => await dbContext
            .Auctions
            .Where(a => a.Id == Id && a.End > GetCurrentDate())
            .ProjectTo<AuctionServiceModel>(mapper)
            .FirstOrDefaultAsync();

        public async Task<AdminEditServiceModel> AdminDetailsById(string Id)
            => await dbContext
                .Auctions
                .Where(a => a.Id == Id)
                .ProjectTo<AdminEditServiceModel>(mapper)
                .FirstOrDefaultAsync();

        public async Task<int> ActiveCount()
            => await dbContext
                .Auctions
                .Where(a => a.End > GetCurrentDate())
                .CountAsync();
    }
}
