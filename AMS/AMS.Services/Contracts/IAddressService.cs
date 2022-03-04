namespace AMS.Services.Contracts
{
    public interface IAddressService
    {
        public string GetAddressId(string country,
            string city,
            string addressText);

        public string AddAddress(string country,
            string city,
            string addressText);
    }
}
