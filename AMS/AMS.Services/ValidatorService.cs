namespace AMS.Services
{
    using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> AreListingParamsValid(int conditionId, int modelId)
        {
            if (!await ConditionExists(conditionId) ||
                !await ModelExists(modelId))
            {
                return false;
            }

            return true;
        }

        public async Task<bool> IsAuctionValid(string Id)
            => await dbContext
            .Auctions
            .AnyAsync(x => x.Id == Id && x.End > GetCurrentDate());

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

        public async Task<bool> IsListingValidForBid(string listingId, string userId, decimal amount)
            => await dbContext
            .Vehicles
            .AnyAsync(x => x.Id == listingId &&
                 x.UserId != userId &&
                 x.Auction.End > GetCurrentDate() &&
                 x.Price <= amount);


        public async Task<bool> DoesWatchlistExist(string listingId, string userId)
            => await dbContext
            .Watchlists
            .AnyAsync(w => w.VehicleId == listingId && w.UserId == userId);


        private async Task<bool> ConditionExists(int conditionId)
             => await dbContext
             .Conditions
             .AnyAsync(x => x.Id == conditionId);

        private async Task<bool> ModelExists(int modelId)
            => await dbContext
             .Models
             .AnyAsync(x => x.Id == modelId);
    }
}
