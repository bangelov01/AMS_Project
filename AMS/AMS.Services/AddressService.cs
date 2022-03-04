namespace AMS.Services
{
    using AMS.Data;
    using AMS.Data.Models;
    using AMS.Services.Contracts;

    public class AddressService : IAddressService
    {
        private readonly AMSDbContext dbContext;

        public AddressService(AMSDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public string AddAddress(string country,
            string city,
            string addressText)
        {
            var address = new Address
            {
                Country = country,
                City = city,
                AddressText = addressText
            };

            dbContext.Addresses.Add(address);
            dbContext.SaveChanges();

            return address.Id;
        }

        public string GetAddressId(string country,
            string city,
            string addressText)
        {
            var address = dbContext
                         .Addresses
                         .Where(a => a.Country == country &&
                                     a.City == city &&
                                     a.AddressText == addressText)
                         .FirstOrDefault();

            if (address == null)
            {
                return null;
            }

            return address.Id;
        }
    }
}
