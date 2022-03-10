namespace AMS.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class MakeVehicleType
    {
        [Required]
        [ForeignKey(nameof(Make))]
        public int MakeId { get; init; }

        public virtual Make Make { get; init; }

        [Required]
        [ForeignKey(nameof(VehicleType))]

        public int VehicleTypeId { get; init; }

        public virtual VehicleType VehicleType { get; init; }
    }
}
