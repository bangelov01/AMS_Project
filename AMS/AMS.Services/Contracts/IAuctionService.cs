namespace AMS.Services.Contracts
{
    using AMS.Data.Models;
    using AMS.Services.Models;

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

        public ICollection<AuctionServiceModel> ActiveAuctions();

        public ICollection<AuctionServiceModel> ActiveAuctionsPerPage(int currentPage, int auctionsPerPage);

        public int ActiveAuctionsCount();

        public AuctionDetailsServiceModel AuctionById(string Id);

        public void Edit(string Id,
            int number,
            string Description,
            DateTime start,
            DateTime end,
            string country,
            string city,
            string addressText);
    }
}
