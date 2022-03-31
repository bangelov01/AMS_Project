namespace AMS.Services.Contracts
{
    using AMS.Services.Models.Auctions;
    using AMS.Services.Models.Auctions.Base;

    public interface IAuctionService
    {
        public Task Create(int number,
            string Description,
            DateTime start,
            DateTime end,
            string country,
            string city,
            string addressText);

        public Task Edit(string Id,
            int number,
            string Description,
            DateTime start,
            DateTime end,
            string country,
            string city,
            string addressText);

        public Task<IEnumerable<AllAuctionsServiceModel>> All();

        public Task<IEnumerable<AllAuctionsServiceModel>> ActivePerPage(int currentPage, int auctionsPerPage);

        public Task<AuctionServiceModel> DetailsById(string Id);

        public Task<AdminEditServiceModel> AdminDetailsById(string Id);

        public Task<bool> IsCreated(int number);

        public Task<int> ActiveCount();
    }
}
