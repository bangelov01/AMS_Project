namespace AMS.Controllers.Models
{
    using AMS.Services.Models.Listings;
    using System.ComponentModel.DataAnnotations;

    using static AMS.Data.Constants.DataConstants.VehicleConstants;

    public class ListingsFormModel
    {
        [Display(Name = "Condition")]
        public int ConditionId { get; init; }
        public IEnumerable<ListingsConditionsServiceModel> Conditions { get; set; }

        [Display(Name = "Vehicle Type")]
        public int TypeId { get; init; }
        public IEnumerable<ListingsTypesServiceModel> Types { get; set; }

        [Display(Name = "Make")]
        public int MakeId { get; init; }
        public IEnumerable<ListingsMakesServiceModel> Makes { get; set; }

        [Display(Name = "Model")]
        public int ModelId { get; init; }
        public IEnumerable<ListingsModelsServiceModel> Models { get; set; }

        [Range(YearMinValue, YearMaxValue)]
        public int Year { get; init; }

        [Required]
        [StringLength(DescriptionMaxLength,
            MinimumLength = DescriptionMinLength,
            ErrorMessage = "The {0} must be between {2} and {1} characters long!")]
        public string Description { get; init; }

        [Display(Name = "Starting Price")]
        [Range(PriceMinValue, PriceMaxValue)]
        public decimal Price { get; init; }

        [Required, Url, Display(Name = "Image URL")]
        public string ImageUrl { get; init; }
    }
}
