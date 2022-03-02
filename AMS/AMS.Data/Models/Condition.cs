namespace AMS.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static AMS.Data.Constants.DataConstants;

    public class Condition
    {
        public Condition()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Vehicles = new HashSet<Vehicle>();
        }

        [Key]
        public string Id { get; init; }

        [Required, MaxLength(VehicleConstants.ConditionMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; init; }
    }
}
