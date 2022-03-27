﻿namespace AMS.Services
{
    using AMS.Data;
    using AMS.Data.Models;
    using AMS.Services.Contracts;

    using static AMS.Services.Common.CommonFunctions;

    public class ValidatorService : IValidatorService
    {
        private readonly AMSDbContext dbContext;

        public ValidatorService(AMSDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool AreListingParamsValid(int conditionId, int modelId)
        {
            if (!ConditionExists(conditionId) ||
                !ModelExists(modelId))
            {
                return false;
            }

            return true;
        }

        public bool IsAuctionValid(string Id)
            => dbContext
            .Auctions
            .Any(x => x.Id == Id && x.End > GetCurrentDate());

        private bool ConditionExists(int conditionId)
            => dbContext
            .Conditions
            .Any(x => x.Id == conditionId);

        private bool ModelExists(int modelId)
            => dbContext
             .Models
             .Any(x => x.Id == modelId);

        public bool IsOrderParamValid(string orderParam)
        {
            if (orderParam == nameof(Make) ||
                orderParam == nameof(Model) ||
                orderParam == nameof(Vehicle.Year))
            {
                return true;
            }

            return false;
        }

        public bool IsListingValidForBid(string listingId, string userId)
            => dbContext
            .Vehicles
            .Any(x => x.Id == listingId &&
                 x.UserId != userId &&
                 x.Auction.End > GetCurrentDate());

        public bool DoesWatchlistExist(string listingId, string userId)
            => dbContext
            .Watchlists
            .Any(w => w.VehicleId == listingId && w.UserId == userId);
    }
}
