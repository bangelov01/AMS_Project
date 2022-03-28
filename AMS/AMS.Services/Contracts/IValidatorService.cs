namespace AMS.Services.Contracts
{
    public interface IValidatorService
    {
        public Task<bool> AreListingParamsValid(int conditionId, int modelId);

        public Task<bool> IsAuctionValid(string Id);

        public Task<bool> IsListingValidForBid(string listingId, string userId, decimal amount);

        public bool IsOrderParamValid(string orderParam);

        public Task<bool> DoesWatchlistExist(string listingId, string userId);
    }
}
