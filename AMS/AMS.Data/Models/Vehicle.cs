namespace AMS.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static AMS.Data.Constants.DataConstants;

    public class Vehicle
    {
        public Vehicle()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Bids = new HashSet<Bid>();
            this.Watchlists = new HashSet<Watchlist>();
        }

        [Key]
        public string Id { get; init; }

        public int Year { get; set; }

        [Required, MaxLength(VehicleConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [ForeignKey(nameof(Auction))]
        public string AuctionId { get; init; }

        public virtual Auction Auction { get; init; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; init; }

        public virtual User User { get; init; }

        [ForeignKey(nameof(Condition))]
        public string ConditionId { get; set; }

        public virtual Condition Condition { get; init; }

        [ForeignKey(nameof(Make))]
        public string MakeId { get; set; }

        public virtual Make Make { get; init; }

        [ForeignKey(nameof(Model))]
        public string ModelId { get; set; }

        public virtual Model Model { get; init; }

        public virtual ICollection<Bid> Bids { get; init; }

        public virtual ICollection<Watchlist> Watchlists { get; init; }

    }
}
