namespace AMS.Services.Contracts
{
    public interface IValidatorService
    {
        public bool IsListingValid(int typeId,
                 int conditionId,
                 int makeId,
                 int modelId);

        public bool IsAuctionValid(string auctionId);
    }
}
