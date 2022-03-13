namespace AMS.Services.Models.Auctions.Contracts
{
    public interface IAuctionModel
    {
        public int Number { get; }

        public DateTime Start { get; }

        public DateTime End { get; }

        public string City { get; }

        public string Description { get; }

        public string Country { get; }
    }
}
