namespace AMS.Services.Contracts
{
    using AMS.Services.Models;

    public interface IUserService
    {
        public ICollection<UsersServiceModel> AllUsers();

        public void SuspendUser(string Id);

        public void AllowUser(string Id);
    }
}
