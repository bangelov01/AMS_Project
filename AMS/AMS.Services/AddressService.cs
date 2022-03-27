namespace AMS.Services
{
    using AMS.Data;
    using AMS.Data.Models;
    using AMS.Services.Contracts;
    using Microsoft.EntityFrameworkCore;

    public class AddressService : IAddressService
    {
        private readonly AMSDbContext dbContext;

        public AddressService(AMSDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public string Add(string country,
            string city,
            string addressText)
        {
            var address = new Address
            {
                Country = country,
                City = city,
                AddressText = addressText
            };

            dbContext.Addresses.AddAsync(address);
            dbContext.SaveChangesAsync();

            return address.Id;
        }

        public async Task<string> GetId(string country,
            string city,
            string addressText)
        {
            var address = await dbContext
                         .Addresses
                         .Where(a => a.Country == country &&
                                     a.City == city &&
                                     a.AddressText == addressText)
                         .FirstOrDefaultAsync();

            if (address == null)
            {
                return null;
            }

            return address.Id;
        }
    }
}
