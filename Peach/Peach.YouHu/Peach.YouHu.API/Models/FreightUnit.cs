namespace Peah.YouHu.API.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Peah.YouHu.API.Models.Enum;

    public class FreightUnit:AuditObj
    {
        [Key]
        public int Id { get; set; }

        public virtual AppUser Driver { get; set; }

        public decimal Height { get; set; }

        public decimal Length { get; set; }

        [MaxLength(10)]
        public string Licence { get; set; }

        [MaxLength(50)]
        public string Location { get; set; }

        public int Rank { get; set; }

        public FreightUnitState State { get; set; }

        public FreightUnitType Type { get; set; }

        public decimal Weight { get; set; }
    }
}