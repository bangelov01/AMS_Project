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

        public void SeedModels()
        {
            if (dbContext.Models.Any())
            {
                return;
            }

            dbContext.Models.AddRange(new[]
            {
                new Model { VehicleTypeId = 1, MakeId = 1, Name = "A3"},
                new Model { VehicleTypeId = 1, MakeId = 1, Name = "A4"},
                new Model { VehicleTypeId = 1, MakeId = 2, Name = "330d"},
                new Model { VehicleTypeId = 1, MakeId = 2, Name = "630d"},
                new Model { VehicleTypeId = 1, MakeId = 3, Name = "Avensis"},
                new Model { VehicleTypeId = 1, MakeId = 3, Name = "Supra"},
                new Model { VehicleTypeId = 1, MakeId = 4, Name = "3"},
                new Model { VehicleTypeId = 1, MakeId = 4, Name = "Miata"},
                new Model { VehicleTypeId = 1, MakeId = 5, Name = "Accord"},
                new Model { VehicleTypeId = 1, MakeId = 5, Name = "Civic"},
                new Model { VehicleTypeId = 1, MakeId = 8, Name = "S Class"},
                new Model { VehicleTypeId = 1, MakeId = 8, Name = "C Class"},

                new Model { VehicleTypeId = 2, MakeId = 2, Name = "R 1250 RS"},
                new Model { VehicleTypeId = 2, MakeId = 2, Name = "R 2000 RS"},

                new Model { VehicleTypeId = 2, MakeId = 5, Name = "X-ADV"},
                new Model { VehicleTypeId = 2, MakeId = 5, Name = "CB1000R"},

                new Model { VehicleTypeId = 3, MakeId = 7, Name = "TGS"},
                new Model { VehicleTypeId = 3, MakeId = 7, Name = "TGM"},

                new Model { VehicleTypeId = 3, MakeId = 8, Name = "Actros L"},
                new Model { VehicleTypeId = 3, MakeId = 8, Name = "Unimog"},
            });

            dbContext.SaveChanges();
        }
    }
}
