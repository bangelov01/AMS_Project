namespace AMS.Services.Contracts
{
    using AMS.Services.Models;

    public interface IUserService
    {
        public Task<IEnumerable<UsersServiceModel>> All();

        public Task<bool> Suspend(string Id);

        public Task<bool> Allow(string Id);
    }
}
