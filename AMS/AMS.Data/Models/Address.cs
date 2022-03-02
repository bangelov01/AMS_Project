namespace AMS.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static AMS.Data.Constants.DataConstants;

    public class Address
    {
        public Address()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Auctions = new HashSet<Auction>();
            this.Users = new HashSet<User>();
        }

        [Key]
        public string Id { get; init; }

        [Required, MaxLength(AddressConstants.CountryNameMaxLength)]
        public string Country { get; init; }

        [Required, MaxLength(AddressConstants.CityNameMaxLength)]
        public string City { get; init; }

        [Required, MaxLength(AddressConstants.AddressTextMaxLength)]
        public string AddressText { get; init; }

        public virtual ICollection<Auction> Auctions { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
