namespace AMS.Services.Contracts
{
    public interface IDataSeederService
    {
        public Task SeedConditions();
        public Task SeedAdministrator();
        public Task SeedVehicleTypes();
        public Task SeedModels();
        public Task SeedMakes();
    }
}
