﻿using System;
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
        [Required]
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Freight Unit Id")]
        public int FreightUnitId { get; set; }

        [Required]
        [Display(Name = "Modified Uesr")]
        public int ModifiedBy { get; set; }
    }


    public class ValidatePaymentCodeBindingModel
    {
        [Required]
        [Display(Name = "Driver Id")]
        public int DriverId { get; set; }

        [Required]
        [Display(Name = "Payment Code")]
        public string PaymentCode { get; set; }
    }

    public class PayBdingModel
    {
        [Required]
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Payment Code")]
        public string PaymentCode { get; set; }

        [Required]
        [Display(Name = "Freight Cost")]
        public decimal FreightCost { get; set; }

        [Required]
        [Display(Name = "Modified Uesr")]
        public int ModifiedBy { get; set; }

        [Required]
        [Display(Name = "Payment Amount")]
        public decimal Paid { get; set; }
    }

    public class ConsignBindingModel
    {
        [Required]
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Payment Code")]
        public string PaymentCode { get; set; }

        [Required]
        [Display(Name = "Modified Uesr")]
        public int ModifiedBy { get; set; }
    }
}