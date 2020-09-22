using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMI.Models
{
    public class Spend
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public List<Spend> SubSpend { get; set; }
        public bool IsOpen { get; set; }
        public bool IsFolder { get; set; }
        public string ColorClass { get; set; }
        public int Level { get; set; }
    }

    public class SpendDDM
    {
        public long Spend1_Id { get; set; }
        public string Spend1_Name { get; set; }
        public long Spend2_Id { get; set; }
        public string Spend2_Name { get; set; }
        public long Spend3_Id { get; set; }
        public string Spend3_Name { get; set; }
        public long Spend4_Id { get; set; }
        public string Spend4_Name { get; set; }
    }
}