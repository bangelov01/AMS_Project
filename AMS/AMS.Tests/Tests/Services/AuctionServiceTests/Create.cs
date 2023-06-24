namespace AMS.Tests.Tests.Services.AuctionServiceTests
{
    using System;
    using System.Threading.Tasks;

    using AMS.Data;
    using AMS.Data.Models;
    using AMS.Services;
    using AMS.Services.Contracts;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using Moq;
    using Xunit;

    using static AMS.Tests.Database.DatabaseInitialize;

    public class Create : IDisposable
    {
        private readonly AMSDbContext data;
        private readonly Mock<IAddressService> addressServiceMock;
        private readonly IMapper mapperMock;
        private readonly AuctionService auctionService;

        public Create()
        {
            this.data = Initialize();
            this.addressServiceMock = new Mock<IAddressService>();
            this.mapperMock = Mock.Of<IMapper>();
            this.auctionService = new AuctionService(this.data, this.addressServiceMock.Object, this.mapperMock);
        }

        public void Dispose()
        {
            this.data.Dispose();
        }

        [Fact]
        public async Task When()
        {
            // Arrange
            int number = 1;
            string description = "testDescription";
            DateTime start = new (2023, 01, 01);
            DateTime end = new (2023, 01, 23);

            string country = "testCountry";
            string city = "testCity";
            string addressText = "testAddressText";

            string addressId = "testAddressId";

            addressServiceMock
                .Setup(m => m.GetId(country, city, addressText))
                .ReturnsAsync(addressId)
                .Verifiable();

            // Act
            await auctionService.Create(number, description, start, end, country, city, addressText);

            Auction? result = await data.Auctions.FirstOrDefaultAsync(a => a.AddressId == addressId);

            // Assert
            addressServiceMock.Verify();

            Assert.NotNull(result);

            Assert.Equal(number, result.Number);
            Assert.Equal(description, result.Description);
            Assert.Equal(start, result.Start);
            Assert.Equal(end, result.End);
            Assert.Equal(addressId, result.AddressId);
        }

        [Fact]
        public async Task When_AddressIdIsNull()
        {
            // Arrange
            int number = 1;
            string description = "testDescription";
            DateTime start = new(2023, 01, 01);
            DateTime end = new(2023, 01, 23);

            string country = "testCountry";
            string city = "testCity";
            string addressText = "testAddressText";

            string addressId = "testAddressId";

            addressServiceMock
                .Setup(m => m.GetId(country, city, addressText))
                .ReturnsAsync((string?)null)
                .Verifiable();

            addressServiceMock
                .Setup(m => m.Add(country, city, addressText))
                .ReturnsAsync(addressId)
                .Verifiable();

            // Act
            await auctionService.Create(number, description, start, end, country, city, addressText);

            Auction? result = await data.Auctions.FirstOrDefaultAsync(a => a.AddressId == addressId);

            // Assert
            addressServiceMock.Verify();

            Assert.NotNull(result);

            Assert.Equal(number, result.Number);
            Assert.Equal(description, result.Description);
            Assert.Equal(start, result.Start);
            Assert.Equal(end, result.End);
            Assert.Equal(addressId, result.AddressId);
        }
    }
}
