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

        public bool Allow(string Id)
        {
            var user = dbContext
                .Users
                .Find(Id);

            if (user == null)
            {
                return false;
            }

            user.IsSuspended = false;          
            dbContext.SaveChanges();

            return true;
        }

        public IEnumerable<UsersServiceModel> All()
            => dbContext
                .Users
                .Where(u => u.UserName != adminDetails.Username)
                .Select(u => new UsersServiceModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    IsSuspended = u.IsSuspended
                })
                .ToArray();

        public bool Suspend(string Id)
        {
            var user = dbContext
                .Users
                .Find(Id);

            if(user == null)
            {
                return false;
            }

            user.IsSuspended = true;
            dbContext.SaveChanges();

            return true;
        }
    }
}
