namespace Peah.YouHu.API.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Peah.YouHu.API.Models.Enum;

    public class Evaluation:AuditObj
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(500)]
        public string Comments { get; set; }

        public EvaluationFrom From { get; set; }

        public virtual Order Order { get; set; }

        public int Rank { get; set; }
    }
}