namespace AMS.Services.Contracts
{
    using AMS.Data.Models;
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

        public ICollection<AuctionServiceModel> AllActive();

        public ICollection<AuctionServiceModel> AllActivePerPage(int currentPage, int auctionsPerPage);

        public int AllActiveCount();

        public AuctionDetailsServiceModel ById(string Id);

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
