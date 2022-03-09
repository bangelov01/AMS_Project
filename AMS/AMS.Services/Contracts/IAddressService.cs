namespace AMS.Services.Contracts
{
    public interface IAddressService
    {
        public string GetId(string country,
            string city,
            string addressText);

        public string Add(string country,
            string city,
            string addressText);
    }
}
