namespace AMS.Services.Contracts
{
    public interface IAddressService
    {
        public Task<string> GetId(string country,
            string city,
            string addressText);

        public Task<string> Add(string country,
            string city,
            string addressText);
    }
}
