namespace AMS.Services
{
    using System.Collections.Generic;

    using Microsoft.Extensions.Options;
    using Microsoft.EntityFrameworkCore;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using AMS.Data;

    using AMS.Services.Contracts;
    using AMS.Services.Models;

    public class UserService(
        AMSDbContext dbContext,
        IOptions<AppSettingsServiceModel> adminDetails,
        IMapper mapper)
        : IUserService
    {
        private readonly AppSettingsServiceModel adminDetails = adminDetails.Value;
        private readonly IConfigurationProvider mapper = mapper.ConfigurationProvider;
        
        public async Task<IEnumerable<UsersServiceModel>> All()
            => await dbContext
                .Users
                .Where(u => u.UserName != adminDetails.Username)
                .ProjectTo<UsersServiceModel>(mapper)
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
