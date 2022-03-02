namespace AMS.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Bid
    {
        public Bid()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; init; }

        public int Number { get; set; }

        [Column(TypeName = "decimal(19, 4)")]
        public decimal Amount { get; set; }

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
