namespace AMS.Services.Models.Contracts
{
    public interface IListingModel
    {
        public int Year { get; }

        public string Make { get; }

        public string Model { get; }

        public string Description { get; }

        public string CreatorName { get; }
    }
}
