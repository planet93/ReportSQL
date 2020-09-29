using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMI.Models
{
    public class LoadEstimation
    {
        public long EstimateId { get; set; }
        public string ActivityName { get; set; }

        public decimal EstimSum { get; set; }
        public decimal TargetSum { get; set; }

        public long MonthId { get; set; }
        public long Spend1Id { get; set; }
        public long Spend2Id { get; set; }
        public long Spend3Id { get; set; }
        public long Spend4Id { get; set; }
    }
}