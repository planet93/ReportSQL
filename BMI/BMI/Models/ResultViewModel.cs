using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMI.Models
{
    public class ResultViewModel
    {
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public bool Error { get; set; }
        public List<string> LineError { get; set; }
    }
}