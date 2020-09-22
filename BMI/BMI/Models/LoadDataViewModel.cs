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

    }
}