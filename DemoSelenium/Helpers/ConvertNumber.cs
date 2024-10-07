using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DemoSelenium.Helpers
{
    public static class ConvertNumber
    {
        public static double ToDouble(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string pattern = @"\D+";

                value = Regex.Replace(value.Trim(), pattern, "", RegexOptions.IgnoreCase); // loại bỏ các ký tự không phải là số
                double.TryParse(value, out double result);
                return result;
            }
            return 0d;
        }
    }
}
