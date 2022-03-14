namespace AMS.Services.Models.Contracts
{
    public interface IAuctionModel
    {
        public int Number { get; }

        public DateTime Start { get; }

        public DateTime End { get; }

        public string City { get; }

    }
}
