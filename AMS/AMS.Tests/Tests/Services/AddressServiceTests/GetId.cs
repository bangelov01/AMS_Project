namespace AMS.Tests.Tests.Services.AddressServiceTests
{
    using System;
    using System.Threading.Tasks;

    using AMS.Data;
    using AMS.Data.Models;
    using AMS.Services;

    using Xunit;

    using static AMS.Tests.Database.DatabaseInitialize;

    public class GetId : IDisposable
    {
        private readonly AMSDbContext data;
        private readonly AddressService addressService;

        public GetId()
        {
            this.data = Initialize();
            this.addressService = new AddressService(data);
        }

        public void Dispose()
        {
            this.data.Dispose();
        }

        [Theory]
        [InlineData("invalid", "invalid", "invalid")]
        [InlineData("TestCountry", "invalid", "invalid")]
        [InlineData("TestCountry", "TestCity", "invalid")]
        public async Task When_Parameters_Are_Invalid(string country, string city, string addressText)
        {
            // Act
            string addressId = await addressService.GetId(country, city, addressText);

            // Assert
            Assert.Null(addressId);
        }

        [Fact]
        public async Task When_AllParametersAreValid()
        {
            // Arrange
            string expectedCountry = "TestCountry";
            string expectedCity = "TestCity";
            string expectedAddressText = "TestStreet";

            // Act
            string addressId = await addressService.GetId(expectedCountry, expectedCity, expectedAddressText);

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
