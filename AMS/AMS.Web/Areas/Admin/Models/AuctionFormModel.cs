namespace AMS.Web.Areas.Admin.Models
{
    using System.ComponentModel.DataAnnotations;

    using static AMS.Data.Constants.DataConstants.AddressConstants;
    using static AMS.Data.Constants.DataConstants.AuctionConstants;
    using static AMS.Data.Constants.DataConstants.ErrorMessages;

    public class AuctionFormModel
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "{0} must be positive!")]
        [Display(Name = "Auction Number")]
        public int Number { get; init; }

        [Required]
        [StringLength(DescriptionMaxLength,
            MinimumLength = DescriptionMinLength,
            ErrorMessage = StringLengthValidationError)]
        public string Description { get; init; }

        [Required, Display(Name = "Auction Start Date")]
        public DateTime Start { get; init; }

        [Required, Display(Name = "Auction End Date")]
        public DateTime End { get; init; }

        [Required]
        [StringLength(CountryNameMaxLength,
            MinimumLength = CountryNameMinLength,
            ErrorMessage = StringLengthValidationError)]
        public string Country { get; init; }

        [Required]
        [StringLength(CityNameMaxLength,
            MinimumLength = CityNameMinLength,
            ErrorMessage = StringLengthValidationError)]
        public string City { get; init; }

        [Required]
        [StringLength(AddressTextMaxLength,
            MinimumLength = AddressTextMinLength,
            ErrorMessage = StringLengthValidationError)]
        [Display(Name = "Street")]
        public string AddressText { get; init; }
    }
}
