namespace AMS.Data
{
    using AMS.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class AMSDbContext : IdentityDbContext<User>
    {
        public AMSDbContext(DbContextOptions<AMSDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; init; }
        public virtual DbSet<Auction> Auctions { get; init; }
        public virtual DbSet<Bid> Bids { get; init; }
        public virtual DbSet<Condition> Conditions { get; init; }
        public virtual DbSet<Make> Makes { get; init; }
        public virtual DbSet<Model> Models { get; init; }
        public virtual DbSet<Vehicle> Vehicles { get; init; }
        public virtual DbSet<Watchlist> Watchlists { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Watchlist>()
                .HasKey(k => new { k.UserId, k.VehicleId });

            base.OnModelCreating(builder);
        }
    }
}