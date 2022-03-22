namespace AMS.Controllers.Models
{
    using AMS.Services.Models.Listings;
    using System.ComponentModel.DataAnnotations;

    using static AMS.Data.Constants.DataConstants.VehicleConstants;

    public class ListingsFormModel
    {
        public ListingsFormModel()
        {
            Conditions = new List<ListingPropertyServiceModel>();
            Types = new List<ListingPropertyServiceModel>();
            Makes = new List<ListingPropertyServiceModel>();
            Models = new List<ListingPropertyServiceModel>();
        }

        [Display(Name = "Condition")]
        public int ConditionId { get; init; }
        public ICollection<ListingPropertyServiceModel> Conditions { get; set; }

        [Display(Name = "Vehicle Type")]
        public int TypeId { get; init; }
        public ICollection<ListingPropertyServiceModel> Types { get; set; }

        [Display(Name = "Make")]
        public int MakeId { get; init; }
        public ICollection<ListingPropertyServiceModel> Makes { get; set; }

        [Display(Name = "Model")]
        public int ModelId { get; init; }
        public ICollection<ListingPropertyServiceModel> Models { get; set; }

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
