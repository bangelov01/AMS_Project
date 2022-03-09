namespace AMS.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static AMS.Data.Constants.DataConstants;

    public class Model
    {
        public Model()
        {
            this.Vehicles = new HashSet<Vehicle>();
        }

        [Key]
        public int Id { get; init; }

        [Required, MaxLength(VehicleConstants.ModelMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; init; }
    }
}
