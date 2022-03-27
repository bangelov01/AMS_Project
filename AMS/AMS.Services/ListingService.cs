namespace AMS.Services
{
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using AMS.Data;
    using AMS.Data.Models;

    using AMS.Services.Contracts;
    using AMS.Services.Models.Listings;
    using AMS.Services.Models.Listings.Base;
    using static AMS.Services.Common.CommonFunctions;

    public class ListingService : IListingService
    {
        private readonly AMSDbContext dbContext;

        public ListingService(AMSDbContext dbContext)
        {
            this.dbContext = dbContext;
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

        public async Task<ListingDetailsServiceModel> Details(string listingId, string userId)
             => await dbContext
             .Vehicles
             .Where(v => v.Id == listingId)
             .Select(v => new ListingDetailsServiceModel
             {
                 Id = v.Id,
                 UserId = v.UserId,
                 Type = v.Model.VehicleType.Name,
                 Condition = v.Condition.Name,
                 ImageUrl = v.ImageUrl,
                 Make = v.Model.Make.Name,
                 Model = v.Model.Name,
                 Description = v.Description,
                 Year = v.Year,
                 Price = v.Price,
                 IsWatched = v.Watchlists.Any(w => w.VehicleId == listingId && w.UserId == userId),
             })
             .FirstOrDefaultAsync();

        public async Task<IEnumerable<ListingsServiceModel>> ApprovedPerPage(string Id, int currentPage, int listingsPerPage)
             => await dbContext
            .Vehicles
            .Where(v => v.AuctionId == Id
                     && v.IsApproved == true)
            .Skip((currentPage - 1) * listingsPerPage)
            .Take(listingsPerPage)
            .Select(v => new ListingsServiceModel
            {
                Id = v.Id,
                Condition = v.Condition.Name,
                Make = v.Model.Make.Name,
                Model = v.Model.Name,
                ImageUrl = v.ImageUrl,
                Year = v.Year
            })
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
                .Select(l => new SearchListingsServiceModel
                {
                    AuctionId = l.AuctionId,
                    AuctionNumber = l.Auction.Number,
                    Id = l.Id,
                    Make = l.Model.Make.Name,
                    Model = l.Model.Name,
                    Condition = l.Condition.Name,
                    ImageUrl = l.ImageUrl,
                    Year = l.Year,
                })
                .ToArrayAsync();

            return listings;
        }

        public async Task<IEnumerable<AdminListingsServiceModel>> NotApproved()
              => await dbContext
              .Vehicles
              .Where(v => v.IsApproved == false)
              .Select(v => new AdminListingsServiceModel
              {
                  Id = v.Id,
                  CreatorName = v.User.UserName,
                  ImageUrl = v.ImageUrl,
                  Year = v.Year,
                  Make = v.Model.Make.Name,
                  Model = v.Model.Name,
                  Description = v.Description,
                  IsUserSuspended = v.User.IsSuspended,
                  BidsCount = v.Bids.Count,
              })
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

        public async Task<ICollection<ListingPropertyServiceModel>> Makes()
            => await dbContext
            .Makes
            .Select(x => new ListingPropertyServiceModel
            {
                Id = x.Id,
                Name= x.Name
            })
            .ToListAsync();

        public async Task<ICollection<ListingPropertyServiceModel>> Models()
            => await dbContext
            .Models
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

        public async Task<int> Count(string auctionId)
            => await dbContext
            .Vehicles
            .Where(v => v.IsApproved == true && v.Auction.Id == auctionId)
            .CountAsync();

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
