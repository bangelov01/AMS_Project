namespace AMS.Controllers.Models
{
    using System.ComponentModel.DataAnnotations;

    using static AMS.Data.Constants.DataConstants.VehicleConstants;

    public class BidInfoModel
    {
        [Range(PriceMinValue, PriceMaxValue)]
        public decimal Amount { get; init; }

        public string ListingId { get; init; }
    }
}
