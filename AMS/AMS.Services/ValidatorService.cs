namespace AMS.Services
{
    using AMS.Data;
    using AMS.Data.Models;
    using AMS.Services.Contracts;

    public class ValidatorService : IValidatorService
    {
        private readonly AMSDbContext dbContext;

        public ValidatorService(AMSDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool IsListingValid(int typeId,
               int conditionId,
               int makeId,
               int modelId)
        {
            if (!TypeExists(typeId) ||
                !ConditionExists(conditionId) ||
                !MakeExists(makeId) ||
                !ModelExists(modelId))
            {
                return false;
            }

            return true;
        }

        public bool IsAuctionValid(string auctionId)
            => dbContext
            .Auctions
            .Any(x => x.Id == auctionId);

        private bool TypeExists(int typeId)
            => dbContext
            .VehicleTypes
            .Any(c => c.Id == typeId);

        private bool ConditionExists(int conditionId)
            => dbContext
            .Conditions
            .Any(x => x.Id == conditionId);

        private bool MakeExists(int makeId)
            => dbContext
            .Makes
            .Any(x => x.Id == makeId);

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
    }
}
