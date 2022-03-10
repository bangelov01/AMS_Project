namespace AMS.Controllers.Models
{
    using AMS.Services.Models.Listings;
    using System.ComponentModel.DataAnnotations;

    using static AMS.Data.Constants.DataConstants.VehicleConstants;

    public class ListingFormModel
    {

        [Range(YearMinValue,
            YearMaxValue,
            ErrorMessage = "{0} cannot be lower than {1} or higher than {2}!")]
        public int Year { get; init; }

        [Required]
        [StringLength(DescriptionMaxLength,
            MinimumLength = DescriptionMinLength,
            ErrorMessage = "{0} must be at least {2} characters long!")]
        public string Description { get; init; }

        [Required, Url, Display(Name = "Image URL")]
        public string ImageUrl { get; init; }

        [Range(typeof(decimal),
            PriceDecimalRangeMinValue,
            PriceDecimalRangeMaxValue,
            ErrorMessage = "Starting price cannot be negative!")]
        [Display(Name = "Starting Price")]
        public decimal Price { get; init; }

        [Display(Name = "Vehicle Type")]
        public string VehicleTypeId { get; set; }

        public IEnumerable<ListingTypesServiceModel> VehicleTypes { get; set; }

        [Display(Name = "Make")]
        public int MakeId { get; set; }

        public IEnumerable<ListingMakeServiceModel> Makes { get; set; }

        [Display(Name = "Model")]
        public int ModelId { get; set; }

        public IEnumerable<ListingModelServiceModel> Models { get; set; }

        [Display(Name = "Condition")]
        public string ConditionId { get; set; }

        public IEnumerable<ListingConditionsServiceModel> Conditions { get; set; }
    }
}
