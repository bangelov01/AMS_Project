namespace AMS.Services.Contracts
{
    using AMS.Services.Models;

    public interface IUserService
    {
        public ICollection<UsersServiceModel> All();

        public void Suspend(string Id);

        public void Allow(string Id);
    }
}
