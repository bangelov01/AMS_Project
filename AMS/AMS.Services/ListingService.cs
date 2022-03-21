namespace AMS.Services
{
    using AMS.Data;
    using AMS.Data.Models;
    using AMS.Services.Contracts;
    using AMS.Services.Models.Listings;
    using AMS.Services.Models.Listings.Base;
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
                ModelId = modelId,
                IsApproved = false
            };

            dbContext.Vehicles.Add(vehicle);
            dbContext.SaveChanges();
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

        public int Count(string auctionId)
            => dbContext
            .Vehicles
            .Where(v => v.IsApproved == true && v.Auction.Id == auctionId)
            .Count();


        public IEnumerable<ListingsServiceModel> ApprovedPerPage(string Id, int currentPage, int listingsPerPage)
            => dbContext
                    .Vehicles
                    .Where(v => v.AuctionId == Id && v.IsApproved == true)
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
                    .ToList();

        public IEnumerable<AdminListingsServiceModel> NotApproved()
            => dbContext
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
            .ToList();

        public bool Delete(string Id)
        {
            var listing = dbContext
                .Vehicles
                .Find(Id);

            if(listing == null)
            {
                return false;
            }

            dbContext.Vehicles.Remove(listing);
            dbContext.SaveChanges();

            return true;
        }

        public bool Approve(string Id)
        {
            var listing = dbContext
                .Vehicles
                .Find(Id);

            if (listing == null)
            {
                return false;
            }

            listing.IsApproved = true;
            dbContext.SaveChanges();

            return true;
        }

        public IEnumerable<SearchListingsServiceModel> Search(string searchString)
        {
            var terms = searchString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var listingQuery = dbContext
                .Vehicles
                .Where(v => v.IsApproved == true)
                .AsQueryable();

            listingQuery = listingQuery.Where(l => (l.Model.Make.Name + l.Model.Name).ToLower().Contains(string.Join("", terms).ToLower()) ||
                            (l.Model.Name + l.Model.Make.Name).ToLower().Contains(string.Join("", terms).ToLower()));

            var listings = listingQuery
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
                .ToList();

            return listings;
        }

        public ListingDetailsServiceModel Details(string id)
            => dbContext
            .Vehicles
            .Where(v => v.Id == id)
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
                Price = v.Price
            })
            .FirstOrDefault();
    }
}
