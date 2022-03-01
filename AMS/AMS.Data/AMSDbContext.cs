namespace AMS.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class AMSDbContext : IdentityDbContext
    {
        public AMSDbContext(DbContextOptions<AMSDbContext> options)
            : base(options)
        {
        }
    }
}