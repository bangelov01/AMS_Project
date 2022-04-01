namespace AMS.Services
{
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using AMS.Data;
    using AMS.Data.Models;

    using AMS.Services.Contracts;
    using AMS.Services.Models.Listings;
    using AMS.Services.Models.Listings.Base;

    using static AMS.Services.Common.CommonFunctions;

    public class ListingService : IListingService
    {
        private readonly AMSDbContext dbContext;
        private readonly IConfigurationProvider mapper;

        public ListingService(AMSDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper.ConfigurationProvider;
        }

        public async Task Create(int year,
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
                ModelId = modelId,
                IsApproved = false
            };

            await dbContext.Vehicles.AddAsync(vehicle);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ListingDetailsServiceModel> Details(string listingId, string auctionId)
             => await dbContext
             .Vehicles
             .Where(v => v.Id == listingId && v.AuctionId == auctionId)
             .ProjectTo<ListingDetailsServiceModel>(mapper)
             .FirstOrDefaultAsync();

        public async Task<IEnumerable<ListingsServiceModel>> ApprovedPerPage(string Id, int currentPage, int listingsPerPage)
             => await dbContext
            .Vehicles
            .Where(v => v.AuctionId == Id
                     && v.IsApproved == true)
            .Skip((currentPage - 1) * listingsPerPage)
            .Take(listingsPerPage)
            .ProjectTo<ListingsServiceModel>(mapper)
            .ToArrayAsync();

        public async Task<IEnumerable<SearchListingsServiceModel>> Search(string searchString)
        {
            var terms = searchString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var listingQuery = dbContext
                .Vehicles
                .Where(v => v.IsApproved == true && v.Auction.End > GetCurrentDate())
                .AsQueryable();

            listingQuery = listingQuery.Where(l => (l.Model.Make.Name + l.Model.Name).ToLower().Contains(string.Join("", terms).ToLower()) ||
                            (l.Model.Name + l.Model.Make.Name).ToLower().Contains(string.Join("", terms).ToLower()));

            var listings = await listingQuery
                .OrderByDescending(l => l.Id)
                .ProjectTo<SearchListingsServiceModel>(mapper)
                .ToArrayAsync();

            return listings;
        }

        public async Task<IEnumerable<AdminListingsServiceModel>> NotApproved()
              => await dbContext
              .Vehicles
              .Where(v => v.IsApproved == false)
              .ProjectTo<AdminListingsServiceModel>(mapper)
              .ToArrayAsync();

        public async Task<ICollection<ListingPropertyServiceModel>> Conditions()
            => await dbContext
            .Conditions
            .Select(x => new ListingPropertyServiceModel
            {
                Id = x.Id,
                Name = x.Name 
            })
            .ToListAsync();

        public async Task<ICollection<ListingPropertyServiceModel>> Types()
            => await dbContext
            .VehicleTypes
            .Select(x => new ListingPropertyServiceModel
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();

        public async Task<ICollection<ListingPropertyServiceModel>> Makes(int typeId)
             => await dbContext
            .Makes
            .Where(m => m.Models.Any(x => x.VehicleTypeId == typeId))
            .Select(x => new ListingPropertyServiceModel
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();

    public async Task<ICollection<ListingPropertyServiceModel>> Models(int typeId, int makeId)
            => await dbContext
            .Models
            .Where(m => m.MakeId == makeId && m.VehicleTypeId == typeId)
            .Select(x => new ListingPropertyServiceModel
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();

        public async Task<bool> IsWatched(string listingId, string userId)
            => await dbContext
            .Watchlists
            .AnyAsync(w => w.VehicleId == listingId && w.UserId == userId);

        public async Task<int> Count(string auctionId)
            => await dbContext
            .Vehicles
            .Where(v => v.IsApproved == true && v.Auction.Id == auctionId)
            .CountAsync();

        public async Task<IEnumerable<ListingsServiceModel>> Preview()
            => await dbContext
            .Vehicles
            .Where(v => v.IsApproved == true && v.Auction.End > GetCurrentDate())
            .Take(3)
            .ProjectTo<ListingsServiceModel>(mapper)
            .ToArrayAsync();

        public async Task<bool> Delete(string Id)
        {
            var listing = await dbContext
                .Vehicles
                .FindAsync(Id);

            if(listing == null)
            {
                return false;
            }

            dbContext.Vehicles.Remove(listing);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Approve(string Id)
        {
            var listing = await dbContext
                .Vehicles
                .FindAsync(Id);

            if (listing == null)
            {
                return false;
            }

            listing.IsApproved = true;
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
