﻿namespace AMS.Tests.Mocks
{
    using AMS.Data;
    using Microsoft.EntityFrameworkCore;
    using System;

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
