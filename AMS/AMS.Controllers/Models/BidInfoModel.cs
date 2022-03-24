namespace AMS.Controllers.Models
{
    using System.ComponentModel.DataAnnotations;

    using static AMS.Data.Constants.DataConstants.VehicleConstants;

    public class BidInfoModel
    {
        [Range(PriceMinValue, PriceMaxValue)]
        [DataType(DataType.Currency)]
        public decimal Amount { get; init; }

        [Required]
        public string ListingId { get; init; }
    }
}
