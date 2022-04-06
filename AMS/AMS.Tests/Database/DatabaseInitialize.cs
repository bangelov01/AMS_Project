namespace AMS.Tests.Database
{
    using AMS.Data;
    using AMS.Data.Models;
    using AMS.Tests.Mocks;
    using System;
    using System.Linq;

    public static class DatabaseInitialize
    {
        public static AMSDbContext Initialize()
        {
            var data = DatabaseMock.Instance;

            data.Makes.Add(new Make { Id = 1, Name = "TestMake" });
            data.VehicleTypes.AddRange(Enumerable.Range(1,4).Select(i => new VehicleType { Id = i, Name = "TestVehicleType" }));
            data.Models.Add(new Model { Id = 1, Name = "TestModel", MakeId = 1, VehicleTypeId = 1 });
            data.Conditions.Add(new Condition { Id = 1, Name = "TestCondition" });

            data.Addresses.AddRange(Enumerable.Range(0, 2).Select(i => new Address
            {
                Id = $"TestAddressId{i}",
                Country = "TestCountry",
                City = "TestCity",
                AddressText = "TestStreet"
            }));

            data.Auctions.AddRange(Enumerable.Range(0,2).Select(i => new Auction
            {
                Id = $"TestAuctionId{i}",
                AddressId = $"TestAddressId{i}",
                Description = "TestDescrioption",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(2000),
                Number = i
            }));

            data.Users.AddRange(Enumerable.Range(0,2).Select(i => new User { Id = $"TestUser{i}", UserName = $"TestUsername{i}" }));

            data.Vehicles.AddRange(Enumerable.Range(0, 5).Select(i => new Vehicle
            {
                Id = $"TestVehicleId{i}",
                ModelId = 1,
                ConditionId = 1,
                AuctionId = "TestAuctionId0",
                Description = "TestDescription",
                UserId = "TestUser0",
                ImageUrl = "TestUrl",
                IsApproved = true,
                Price = 1000,
            }));

            data.Bids.AddRange(Enumerable.Range(0, 5).Select(i => new Bid
            {
                Id = $"TestBid{i}",
                Amount = 100 + i,
                Number = i,
                VehicleId = $"TestVehicleId0",
                UserId = "TestUser1",
            }));

            data.SaveChanges();

            return data;
        }
    }
}
