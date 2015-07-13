using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peah.YouHu.API.Models
{
    using System.ComponentModel.DataAnnotations;
    public class PublishBindingModel
    {
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Destination")]
        public string Destination { get; set; }

        [Required]
        [Display(Name = "Source")]
        public string Source { get; set; }

        [Required]
        [Display(Name = "Size")]
        public float Size { get; set; }

        [Required]
        [Display(Name = "Weight")]
        public float Weight { get; set; }

    }

    public class MakeDealBindingModel
    {
        public int OrderId { get; set; }

        public int FreightUnitId { get; set; }
    }
}