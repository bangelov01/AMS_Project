namespace AMS.Tests.Tests.Services.AddressServiceTests
{
    using System;
    using System.Threading.Tasks;

    using AMS.Data;
    using AMS.Services;
    using AMS.Data.Models;

    using Xunit;

    using static AMS.Tests.Database.DatabaseInitialize;

    public class Add : IDisposable
    {
        private readonly AMSDbContext data;
        private readonly AddressService addressService;

        public Add()
        {
            this.data = Initialize();
            this.addressService = new AddressService(data);
        }

        public void Dispose()
        {
            this.data.Dispose();
        }

        [Fact]
        public async Task When()
        {
            // Arrange
            string expectedCountry = "awesomeCountry";
            string expectedCity = "awesomeCity";
            string expectedAddressText = "awesomeAddressText";

            // Act
            string addressId = await addressService.Add(expectedCountry, expectedCity, expectedAddressText);

            // Assert
            Assert.NotNull(addressId);

            Address? result = await data.Addresses.FindAsync(addressId);

            Assert.NotNull(result);

            Assert.Equal(result.Id, addressId);
            Assert.Equal(result.Country, expectedCountry);
            Assert.Equal(result.City, expectedCity);
            Assert.Equal(result.AddressText, expectedAddressText);
        }
    }
}
