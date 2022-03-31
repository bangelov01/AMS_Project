namespace AMS.Services
{
    using Microsoft.EntityFrameworkCore;

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

        public async Task<string> Add(string country,
            string city,
            string addressText)
        {
            var address = new Address
            {
                Country = country,
                City = city,
                AddressText = addressText
            };

            await dbContext.Addresses.AddAsync(address);
            await dbContext.SaveChangesAsync();

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
