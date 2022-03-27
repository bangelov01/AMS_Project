namespace AMS.Services.Contracts
{
    using AMS.Services.Models;

    public interface IUserService
    {
        public IEnumerable<UsersServiceModel> All();

        public bool Suspend(string Id);

        public bool Allow(string Id);
    }
}
