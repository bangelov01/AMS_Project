namespace AMS.Tests.Database
{
    using System;

    using AMS.Data;

    using static AMS.Tests.Database.DatabaseInitialize;

    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            this.data = Initialize();
        }

        public void Dispose()
        {
            data.Dispose();
        }

        public AMSDbContext data { get; private set; }
    }
}
