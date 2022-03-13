namespace AMS.Services.Contracts
{
    using AMS.Services.Models.Auctions;

    public interface IAuctionService
    {
        public void Create(int number,
            string Description,
            DateTime start,
            DateTime end,
            string country,
            string city,
            string addressText);

        public bool IsCreated(int number);

        public ICollection<AllAuctionsServiceModel> AllActive();

        public ICollection<AllAuctionsServiceModel> AllActivePerPage(int currentPage, int auctionsPerPage);

        public int AllActiveCount();

        public AdminEditServiceModel DetailsById(string Id);

        public AuctionListingsServiceModel DetailsListingsPerPage(string Id, int currentPage, int listingsPerPage);

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
