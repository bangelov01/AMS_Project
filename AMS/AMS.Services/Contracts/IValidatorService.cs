namespace AMS.Services.Contracts
{
    public interface IValidatorService
    {
        public bool AreListingParamsValid(int typeId,
                 int conditionId,
                 int makeId,
                 int modelId);

        public bool IsAuctionValid(string Id);

        public bool IsListingValid(string Id);

        public bool IsOrderParamValid(string orderParam);
    }
}
