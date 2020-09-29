using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMI.Models
{
    public class EstimateViewModel
    {
        public string Name { get; set; }
        public List<EstimateSum> MonthSum { get; set; } = new List<EstimateSum>();

        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class EstimateSum
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal EstimSum { get; set; }
        public decimal TargetSum { get; set; }
        public decimal Deviation { get; set; }
        public long MonthId { get; set; }
        public long ChildrenSpendId { get; set; }
        public bool AllowEdit { get; set; }
    }
    public class Result
    {
        public List<EstimateSum> Header { get; set; } = new List<EstimateSum>();
        public List<EstimateViewModel> ResultList { get; set; } = new List<EstimateViewModel>();
    }
}