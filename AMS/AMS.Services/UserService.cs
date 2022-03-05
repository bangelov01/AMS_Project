namespace AMS.Services
{
    using System.Collections.Generic;

    using Microsoft.Extensions.Options;

    using AMS.Data;
    using AMS.Services.Contracts;
    using AMS.Services.Models;

    public class UserService : IUserService
    {
        public readonly AMSDbContext dbContext;
        private readonly AppSettingsServiceModel adminDetails;

        public UserService(AMSDbContext dbContext,
            IOptions<AppSettingsServiceModel> adminDetails)
        {
            this.dbContext = dbContext;
            this.adminDetails = adminDetails.Value;
        }

        public void AllowUser(string Id)
        {
            var user = dbContext
                .Users
                .Find(Id);

            user.IsSuspended = false;
            
            dbContext.SaveChanges();
        }

        public ICollection<UsersServiceModel> AllUsers()
        {
            var users = dbContext
                .Users
                .Where(u => u.UserName != adminDetails.Username)
                .Select(u => new UsersServiceModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    IsSuspended = u.IsSuspended
                })
                .ToList();

            return users;
        }

        public void SuspendUser(string Id)
        {
            var user = dbContext
                .Users
                .Find(Id);

            user.IsSuspended = true;

            dbContext.SaveChanges();
        }
    }
}
