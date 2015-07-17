﻿namespace Peah.YouHu.API.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Peah.YouHu.API.Models.Enum;

    public class Evaluation
    {
        [Key]
        public int Id { get; set; }

        public string Comments { get; set; }

        public EvaluationFrom From { get; set; }

        public Order Order { get; set; }

        public int Rank { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }
    }
}