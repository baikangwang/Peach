using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peah.YouHu.API.Models
{
    using System.ComponentModel.DataAnnotations;

    using Peah.YouHu.API.Models.Enum;

    public class PublishFreightUnitBindingModel
    {
        [Required]
        [Display(Name = "Freight Unit Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Modified User")]
        public int ModifiedBy { get; set; }
    }

    public class RegisterFreightUnitBindingModel
    {
        [Required]
        [Display(Name = "Driver Id")]
        public int DriverId { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Height of the freight unit")]
        public float Height { get; set; }

        [Required]
        [Display(Name = "Length of the freight unit")]
        public float Length { get; set; }

        [Required]
        [Display(Name = "Licence of the freight unit")]
        public string Licence { get; set; }

        [Required]
        [Display(Name = "the freight unit Type")]
        public FreightUnitType Type { get; set; }

        [Required]
        [Display(Name = "Weight of the freight unit")]
        public float Weight { get; set; }
    }
}