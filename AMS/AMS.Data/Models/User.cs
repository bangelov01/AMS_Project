namespace AMS.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public User()
        {
            this.Vehicles = new HashSet<Vehicle>();
            this.Bids = new HashSet<Bid>();
            this.Watchlists = new HashSet<Watchlist>();
            this.IsSuspended = false;
        }

        public bool IsSuspended { get; set; }

        [ForeignKey(nameof(Address))]
        public string AddressId { get; set; }

        public virtual Address Address { get; init; }

        public virtual ICollection<Vehicle> Vehicles { get; init; }

        public virtual ICollection<Bid> Bids { get; init; }

        public virtual ICollection<Watchlist> Watchlists { get; init; }
    }
}
