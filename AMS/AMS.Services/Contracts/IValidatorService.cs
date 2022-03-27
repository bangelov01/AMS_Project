namespace AMS.Services.Contracts
{
    public interface IValidatorService
    {
        public bool AreListingParamsValid(int conditionId, int modelId);

        public bool IsAuctionValid(string Id);

        public bool IsListingValidForBid(string listingId, string userId);

        public bool IsOrderParamValid(string orderParam);

        public bool DoesWatchlistExist(string listingId, string userId);
    }
}
