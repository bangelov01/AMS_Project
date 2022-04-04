namespace AMS.Tests.Database
{
    using System;
    using System.Linq;

    using AMS.Data;
    using AMS.Data.Models;

    using AMS.Tests.Mocks;

    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            this.data = Initialize();
        }

        public void Dispose()
        {
            data.Dispose();
        }

        public AMSDbContext data { get; private set; }

        private AMSDbContext Initialize()
        {
            var data = DatabaseMock.Instance;

            data.Makes.Add(new Make { Id = 1, Name = "TestMake" });
            data.VehicleTypes.Add(new VehicleType { Id = 1, Name = "TestVehicleType" });
            data.Models.Add(new Model { Id = 1, Name = "TestModel", MakeId = 1, VehicleTypeId = 1 });
            data.Conditions.Add(new Condition { Id = 1, Name = "TestCondition" });

            data.Auctions.Add(new Auction
            {
                Id = "TestId",
                AddressId = "TestAddressId",
                Description = "TestDescrioption",
                End = DateTime.Now.AddDays(2000)
            });

            data.Vehicles.AddRange(Enumerable.Range(0,5).Select(i => new Vehicle
            {
                Id = $"TestId{i}",
                ModelId = 1,
                ConditionId = 1,
                AuctionId = "TestId",
                Description = "TestDescription",
                UserId = "TestId",
                ImageUrl = "TestUrl",
                IsApproved = true,
            }));

            data.Users.Add(new User());

            data.SaveChanges();

            return data;
        }
    }
}
