﻿namespace AMS.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static AMS.Data.Constants.DataConstants;

    public class VehicleType
    {
        public VehicleType()
        {
            this.MakeVehicleTypes = new HashSet<MakeVehicleType>();
        }

        [Key]
        public int Id { get; init; }

        [Required, MaxLength(VehicleConstants.TypeMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<MakeVehicleType> MakeVehicleTypes { get; init;}
    }
}
