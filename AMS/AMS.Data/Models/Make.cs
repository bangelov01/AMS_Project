namespace AMS.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static AMS.Data.Constants.DataConstants;

    public class Make
    {
        public Make()
        {
            this.Vehicles = new HashSet<Vehicle>();
        }

        [Key]
        public int Id { get; init; }

        [Required, MaxLength(VehicleConstants.MakeMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; init; }
    }
}
