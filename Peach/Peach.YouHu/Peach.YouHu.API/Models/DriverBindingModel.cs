namespace Peah.YouHu.API.Models
{
    using System.ComponentModel.DataAnnotations;

    public class WithDrawBindingModel
    {
        //[Required]
        //[Display(Name = "Driver Id")]
        //public int DriverId { get; set; }

        [Required]
        [Display(Name = "WithDraw Amount")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Payment Code")]
        public string PaymentCode { get; set; }
    }
}