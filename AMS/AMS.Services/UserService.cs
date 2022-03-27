namespace AMS.Services
{
    using System.Collections.Generic;

    using Microsoft.Extensions.Options;
    using Microsoft.EntityFrameworkCore;

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


        public async Task<IEnumerable<UsersServiceModel>> All()
            => await dbContext
                .Users
                .Where(u => u.UserName != adminDetails.Username)
                .Select(u => new UsersServiceModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    IsSuspended = u.IsSuspended
                })
                .ToArrayAsync();

        public async Task<bool> Allow(string Id)
        {
            var user = await dbContext
                .Users
                .FindAsync(Id);

            if (user == null)
            {
                return false;
            }

            user.IsSuspended = false;          
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Suspend(string Id)
        {
            var user = await dbContext
                .Users
                .FindAsync(Id);

            if(user == null)
            {
                return false;
            }

            user.IsSuspended = true;
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
