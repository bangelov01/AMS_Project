﻿namespace AMS.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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

        [Required]
        [ForeignKey(nameof(Make))]
        public int MakeId { get; init; }

        public virtual Make Make { get; init; }

        [Required]
        [ForeignKey(nameof(VehicleType))]
        public int VehicleTypeId { get; init; }

        public virtual VehicleType VehicleType { get; init; }

        public virtual ICollection<Vehicle> Vehicles { get; init; }
    }
}
