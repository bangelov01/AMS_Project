namespace AMS.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static AMS.Data.Constants.DataConstants;

    public class Make
    {
        public Make()
        {
            this.Models = new HashSet<Model>();
            this.MakeVehicleTypes = new HashSet<MakeVehicleType>();
        }

        [Key]
        public int Id { get; init; }

        [Required, MaxLength(VehicleConstants.MakeMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<Model> Models { get; init; }
        public virtual ICollection<MakeVehicleType> MakeVehicleTypes { get; init; }

    }
}
