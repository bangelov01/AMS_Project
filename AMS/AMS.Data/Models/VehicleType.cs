namespace AMS.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static AMS.Data.Constants.DataConstants;

    public class VehicleType
    {
        public VehicleType()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Vehicles = new HashSet<Vehicle>();
        }

        [Key]
        public string Id { get; init; }

        [Required, MaxLength(VehicleConstants.TypeMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; init; }
    }
}
