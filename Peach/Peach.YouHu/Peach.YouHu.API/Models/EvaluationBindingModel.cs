using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peah.YouHu.API.Models
{
    using System.ComponentModel.DataAnnotations;

    public class EvaluateBindingModel
    {
        [Required]
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Owner/Driver Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Rank")]
        public int Rank { get; set; }

        [Required]
        [Display(Name = "Comments")]
        public string Comments { get; set; }
    }
}