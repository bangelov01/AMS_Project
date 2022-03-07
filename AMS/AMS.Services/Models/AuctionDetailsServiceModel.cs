namespace AMS.Services.Models
{
    public class AuctionDetailsServiceModel
    {

        public int Number { get; init; }

        public string Description { get; init; }

        public DateTime Start { get; init; }

        public DateTime End { get; init; }

        public string Country { get; init; }

        public string City { get; init; }

        public string AddressText { get; init; }
    }
}
