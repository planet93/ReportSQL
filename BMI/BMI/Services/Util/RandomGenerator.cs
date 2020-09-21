using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMI.Services.Util
{
    public static class RandomGenerator
    {
        public static string RandomString(int size = 32)
        {
            var guid = Guid.NewGuid();
            var guidStr = guid.ToString().Replace("-", "").Trim();
            return size > 32 ? string.Concat(guidStr, RandomString(size - 32)) : guidStr.Substring(0, size);
        }
    }
}