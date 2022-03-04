namespace AMS.Services
{
    using System;

    using AMS.Data;
    using AMS.Data.Models;
    using AMS.Services.Contracts;

    public class AuctionService : IAuctionService
    {
        private readonly AMSDbContext dbContext;
        private readonly IAddressService addressService;

        public AuctionService(AMSDbContext dbContext,
            IAddressService addressService)
        {
            this.dbContext = dbContext;
            this.addressService = addressService;
        }

        public void CreateAuction(int number,
            string Description,
            DateTime start,
            DateTime end,
            string country,
            string city,
            string addressText)
        {
            var addressId = addressService.GetAddressId(country, city, addressText);

            if (addressId == null)
            {
                var newAddress = addressService.AddAddress(country, city, addressText);

                addressId = newAddress;
            }

            var auction = new Auction
            {
                Number = number,
                Description = Description,
                Start = start,
                End = end,
                AddressId = addressId
            };

            dbContext.Auctions.Add(auction);
            dbContext.SaveChanges();
        }

        public bool IsAuctionCreated(int number)
        {
            var auction = dbContext
                .Auctions
                .Where(a => a.Number == number)
                .FirstOrDefault();

            if (auction == null)
            {
                return false;
            }

            return true;
        }
    }
}
