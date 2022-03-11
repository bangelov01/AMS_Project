namespace AMS.Services.Contracts
{
    public interface IDataSeederService
    {
        public void SeedConditions();
        public void SeedAdministrator();
        public void SeedVehicleTypes();
        public void SeedModels();
        public void SeedMakes();
    }
}
