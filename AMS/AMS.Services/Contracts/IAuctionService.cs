namespace AMS.Services.Contracts
{
    using AMS.Data.Models;

    public interface IAuctionService
    {
        public void CreateAuction(int number,
            string Description,
            DateTime start,
            DateTime end,
            string country,
            string city,
            string addressText);

        public bool IsAuctionCreated(int number);

    }
}
