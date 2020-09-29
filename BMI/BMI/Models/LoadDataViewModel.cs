using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMI.Models
{
    public class LoadDataViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Parent { get; set; }
        public string Type { get; set; }

        public string Spend1 { get; set; }
        public string Spend2 { get; set; }
        public string Spend3 { get; set; }
        public string Spend4 { get; set; }

        public string ActivityName { get; set; }

        public string Year { get; set; }

        public string Jan { get; set; }
        public string Feb { get; set; }
        public string Mar { get; set; }
        public string Apr { get; set; }
        public string May { get; set; }
        public string Jun { get; set; }
        public string Jul { get; set; }
        public string Aug { get; set; }
        public string Sep { get; set; }
        public string Oct { get; set; }
        public string Nov { get; set; }
        public string Dec { get; set; }

        public long Spend1Id { get; set; }
        public long Spend2Id { get; set; }
        public long Spend3Id { get; set; }
        public long Spend4Id { get; set; }

        public long EstimateId { get; set; }
    }
}