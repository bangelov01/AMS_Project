namespace AMS.Services
{
    using System.Linq;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;

    using AMS.Data;
    using AMS.Data.Models;
    using AMS.Services.Contracts;
    using AMS.Services.Models;

    using static AMS.Services.Constants.ServicesConstants;

    public class DataSeederService : IDataSeederService
    {
        private readonly AMSDbContext dbContext;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AppSettingsServiceModel adminDetails;

        public DataSeederService(AMSDbContext dbContext,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<AppSettingsServiceModel> adminDetails)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.adminDetails = adminDetails.Value;
        }

        public void SeedAdministrator()
        {
            var test1 = adminDetails.Email;
            var test2 = adminDetails.Username;

            Task.Run(async () => 
            {
                if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                {
                    return;
                }

                var role = new IdentityRole
                {
                    Name = AdministratorRoleName
                };

                await roleManager.CreateAsync(role);

                var user = new User
                {
                    Email = adminDetails.Email,
                    UserName = adminDetails.Username,
                };

                await userManager.CreateAsync(user, adminDetails.Password);

                await userManager.AddToRoleAsync(user, role.Name);

            })
            .GetAwaiter()
            .GetResult();
        }

        public void SeedConditions()
        {
            if (dbContext.Conditions.Any())
            {
                return;
            }

            dbContext.Conditions.AddRange(new[]
            {
                new Condition { Name = "Used" },
                new Condition { Name = "Salvage" },
                new Condition { Name = "Damaged" }
            });

            dbContext.SaveChanges();
        }


        public void SeedVehicleTypes()
        {
            if (dbContext.VehicleTypes.Any())
            {
                return;
            }

            dbContext.VehicleTypes.AddRange(new[]
            {
                new VehicleType { Name = "Car" },
                new VehicleType { Name = "Motorbike" },
                new VehicleType { Name = "Truck" },
                new VehicleType { Name = "ATV" }
            });

            dbContext.SaveChanges();
        }

        public void SeedMakes()
        {
            if (dbContext.Makes.Any())
            {
                return;
            }

            dbContext.Makes.AddRange(new[]
            {
                new Make { Name = "Audi"},
                new Make { Name = "BMW"},
                new Make { Name = "Toyota"},
                new Make { Name = "Mazda"},
                new Make { Name = "Honda"},
                new Make { Name = "Kawazaki"},
                new Make { Name = "MAN"},
                new Make { Name = "Mercedes"},
            });

            dbContext.SaveChanges();
        }

        public void SeedMakeVehicleTypes()
        {
            if (dbContext.MakeVehicleTypes.Any())
            {
                return;
            }

            dbContext.MakeVehicleTypes.AddRange(new[]
            {
                new MakeVehicleType { MakeId = 1, VehicleTypeId = 1 },
                new MakeVehicleType { MakeId = 2, VehicleTypeId = 1 },
                new MakeVehicleType { MakeId = 2, VehicleTypeId = 2 },
                new MakeVehicleType { MakeId = 3, VehicleTypeId = 1 },
                new MakeVehicleType { MakeId = 4, VehicleTypeId = 1 },
                new MakeVehicleType { MakeId = 5, VehicleTypeId = 1 },
                new MakeVehicleType { MakeId = 5, VehicleTypeId = 2 },
                new MakeVehicleType { MakeId = 5, VehicleTypeId = 4 },
                new MakeVehicleType { MakeId = 6, VehicleTypeId = 2 },
                new MakeVehicleType { MakeId = 6, VehicleTypeId = 4 },
                new MakeVehicleType { MakeId = 7, VehicleTypeId = 3 },
                new MakeVehicleType { MakeId = 8, VehicleTypeId = 1 },
                new MakeVehicleType { MakeId = 8, VehicleTypeId = 3 },
            });

            dbContext.SaveChanges();
        }
    }
}
