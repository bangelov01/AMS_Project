namespace AMS.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using AMS.Data.Models;

    public class AMSDbContext(DbContextOptions<AMSDbContext> options) : IdentityDbContext<User>(options)
    {
        public virtual DbSet<Address> Addresses { get; init; }
        public virtual DbSet<Auction> Auctions { get; init; }
        public virtual DbSet<Bid> Bids { get; init; }
        public virtual DbSet<Condition> Conditions { get; init; }
        public virtual DbSet<Make> Makes { get; init; }
        public virtual DbSet<Model> Models { get; init; }
        public virtual DbSet<VehicleType> VehicleTypes { get; init; }
        public virtual DbSet<Vehicle> Vehicles { get; init; }
        public virtual DbSet<Watchlist> Watchlists { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Watchlist>()
                .HasKey(k => new { k.UserId, k.VehicleId });

            builder.Entity<User>()
                .HasOne<Address>(u => u.Address)
                .WithMany(a => a.Users)
                .HasForeignKey(u => u.AddressId)
                .IsRequired(false);

            base.OnModelCreating(builder);
        }
    }
}