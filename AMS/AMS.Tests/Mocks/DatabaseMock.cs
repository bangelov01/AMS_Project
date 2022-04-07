namespace AMS.Tests.Mocks
{
    using System;

    using Microsoft.EntityFrameworkCore;

    using AMS.Data;

    public class DatabaseMock
    {
        public static AMSDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<AMSDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString(), b => b.EnableNullChecks(false))
                    .Options;

                return new AMSDbContext(dbContextOptions);
            }
        }
    }
}
