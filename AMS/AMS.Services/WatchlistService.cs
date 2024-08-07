﻿namespace AMS.Services
{
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using AMS.Data;
    using AMS.Data.Models;

    using AMS.Services.Contracts;
    using AMS.Services.Models.Listings;

    using static AMS.Services.Common.CommonFunctions;

    public class WatchlistService(
        AMSDbContext dbContext,
        IMapper mapper) : IWatchlistService
    {
        private readonly IConfigurationProvider mapper = mapper.ConfigurationProvider;

        public async Task Create(string listingId, string userId)
        {
            var watchlist = new Watchlist
            {
                VehicleId = listingId,
                UserId = userId,
            };

            await dbContext.Watchlists.AddAsync(watchlist);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> Delete(string listingId, string userId)
        {
            var watch = await dbContext
                .Watchlists
                .Where(w => w.VehicleId == listingId && w.UserId == userId)
                .FirstOrDefaultAsync();

            if (watch == null)
            {
                return false;
            }

            dbContext.Watchlists.Remove(watch);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<SearchListingsServiceModel>> ListingsForUser(string Id)
            => await dbContext
                .Watchlists
                .Where(w => w.UserId == Id && w.Vehicle.Auction.End > GetCurrentDate())
                .ProjectTo<SearchListingsServiceModel>(mapper)
                .ToArrayAsync();
    }
}
