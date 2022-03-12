namespace AMS.Controllers.Models
{
    using System.ComponentModel.DataAnnotations;

    using static AMS.Data.Constants.DataConstants.VehicleConstants;

    public class VehicleFormModel
    {
        public int MyProperty { get; set; }

        [Range(YearMinValue, YearMaxValue)]
        public int Year { get; init; }

        [Required]
        [StringLength(DescriptionMaxLength,
            MinimumLength = DescriptionMinLength,
            ErrorMessage = "The {0} must be between {2} and {1} characters long!")]
        public string Description { get; init; }

        [Range(PriceMinValue, PriceMaxValue)]
        public decimal Price { get; init; }

        [Required, Url, Display(Name = "Image URL")]
        public string ImageUrl { get; init; }
    }
}
