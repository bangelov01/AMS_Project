namespace AMS.Services
{
    using Microsoft.EntityFrameworkCore;

    using AMS.Data;
    using AMS.Data.Models;

    using AMS.Services.Contracts;
    using static AMS.Services.Common.CommonFunctions;

    public class ValidatorService(AMSDbContext dbContext) : IValidatorService
    {
        public async Task<bool> AreListingParamsValid(int conditionId,
            int modelId,
            int makeId,
            int typeId)
        {
            return await ConditionExists(conditionId) &&
                   await ModelExists(modelId) &&
                   await MakeExists(makeId) &&
                   await TypeExists(typeId);
        }

        public async Task<bool> IsAuctionValid(string Id)
            => await dbContext
            .Auctions
            .AnyAsync(x => x.Id == Id && x.End > GetCurrentDate());

        public bool IsOrderParamValid(string orderParam)
        {
            return orderParam is nameof(Make) or nameof(Model) or nameof(Vehicle.Year);
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

        private async Task<bool> MakeExists(int makeId)
           => await dbContext
            .Makes
            .AnyAsync(x => x.Id == makeId);

        private async Task<bool> TypeExists(int typeId)
           => await dbContext
            .VehicleTypes
            .AnyAsync(x => x.Id == typeId);

    }
}
