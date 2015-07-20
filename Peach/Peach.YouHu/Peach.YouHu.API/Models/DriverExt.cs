namespace Peah.YouHu.API.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DriverExt:AuditObj
    {
        [Key]
        public int Id { get; set; }
        
        public AppUser Driver { get; set; }

        public decimal CurrentIncome { get; set; }

        public decimal TotalIncome { get; set; }
    }
}