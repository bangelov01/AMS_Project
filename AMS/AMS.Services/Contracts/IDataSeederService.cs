namespace AMS.Services.Contracts
{
    public interface IDataSeederService
    {
        public void SeedConditions();
        public void SeedVehicleTypes();
        public void SeedAdministrator();
        public void SeedMakes();
        public void SeedMakeVehicleTypes();
    }
}
