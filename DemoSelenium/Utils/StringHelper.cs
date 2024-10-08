using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DemoSelenium.Utils
{
    public static class StringHelper
    {
        // Loại bỏ thẻ HTML
        public static string RemoveHtmlTags(string html)
        {
            return Regex.Replace(html, "<.*?>", String.Empty);
        }

        // Chuẩn hóa văn bản: loại bỏ khoảng trắng thừa và trim
        public static string NormalizeText(string text)
        {
            return Regex.Replace(text.Trim(), @"\s+", " ");
        }
    }
}
