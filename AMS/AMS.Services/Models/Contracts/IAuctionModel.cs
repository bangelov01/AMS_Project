namespace AMS.Services.Models.Contracts
{
    public interface IAuctionModel
    {
        public int Number { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Country { get; init; }

        public string City { get; init; }

        public string AddressText { get; init; }
    }
}
