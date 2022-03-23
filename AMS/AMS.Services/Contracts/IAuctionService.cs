namespace AMS.Services.Contracts
{
    using AMS.Services.Models.Auctions;
    using AMS.Services.Models.Auctions.Base;

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

        public AuctionServiceModel DetailsById(string Id);

        public ICollection<AllAuctionsServiceModel> All();

        public ICollection<AllAuctionsServiceModel> ActivePerPage(int currentPage, int auctionsPerPage);

        public int ActiveCount();

        public AdminEditServiceModel AdminDetailsById(string Id);

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
