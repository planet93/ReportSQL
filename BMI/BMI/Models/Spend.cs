using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMI.Models
{
    public class Spend
    {
        public string Name { get; set; }

        public List<Spend> SubSpend { get; set; }
        public bool IsOpen { get; set; }
        public bool IsFolder { get; set; }
        public string ColorClass { get; set; }
        public int Level { get; set; }
    }
}