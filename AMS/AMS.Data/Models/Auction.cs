namespace AMS.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static AMS.Data.Constants.DataConstants;

    public class Auction
    {
        public Auction()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Vehicles = new HashSet<Vehicle>();
        }

        [Key]
        public string Id { get; init; }

        public int Number { get; set; }

        [Required, MaxLength(AuctionConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        [Required]
        [ForeignKey(nameof(Address))]
        public string AddressId { get; set; }

        public virtual Address Address { get; init; }

        public virtual ICollection<Vehicle> Vehicles { get; init; }
    }
}
