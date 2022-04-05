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
            data.VehicleTypes.Add(new VehicleType { Id = 1, Name = "TestVehicleType" });
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

            data.Users.Add(new User { Id = "TestUserId" });

            data.Vehicles.AddRange(Enumerable.Range(0, 5).Select(i => new Vehicle
            {
                Id = $"TestVehicleId{i}",
                ModelId = 1,
                ConditionId = 1,
                AuctionId = "TestAuctionId0",
                Description = "TestDescription",
                UserId = "TestUserId",
                ImageUrl = "TestUrl",
                IsApproved = true,
                Price = 1000,
            }));

            data.SaveChanges();

            return data;
        }
    }
}
