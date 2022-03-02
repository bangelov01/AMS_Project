namespace AMS.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Watchlist
    {
        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; init; }

        public virtual User User { get; init; }

        [Required]
        [ForeignKey(nameof(Vehicle))]
        public string VehicleId { get; init; }

        public virtual Vehicle Vehicle { get; init; }
    }
}
