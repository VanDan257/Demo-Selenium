using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSeleniumWF.Utils
{
    public static class CompareMethods
    {
        /// <summary>
        /// So sánh 2 chuỗi bằng nhau, bỏ qua xuống dòng
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool CompareEqual(string str1, string str2)
        {
            string normalizedStr1 = StringHelper.NormalizeText(str1); // replace xuống dòng thành khoảng trắng
            string normalizedStr2 = StringHelper.NormalizeText(str2);

            bool areEqual = normalizedStr1.Equals(normalizedStr2, StringComparison.OrdinalIgnoreCase);

            return areEqual;
        }

        /// <summary>
        /// So sánh chuỗi parentStr có chứa childStr không, bỏ qua xuống dòng
        /// </summary>
        /// <param name="parentStr"></param>
        /// <param name="childStr"></param>
        /// <returns></returns>
        public static bool CompareContain(string parentStr, string childStr)
        {
            string normalizedParentStr = StringHelper.NormalizeText(parentStr);
            string normalizedChildStr = StringHelper.NormalizeText(childStr);

            bool areContain = normalizedParentStr.Contains(normalizedChildStr);

            return areContain;
        }

        /// <summary>
        /// Convert 2 chuỗi truyền vào thành KDL double và so sánh 
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool CompareNumber(string str1, string str2)
        {
            var numberStr1 = ConvertNumber.ToDouble(str1);
            var numberStr2 = ConvertNumber.ToDouble(str2);

            bool areEqual = numberStr1 == numberStr2;

            return areEqual;
        }

        /// <summary>
        /// So sánh 1 mảng có KDL chuỗi có chứa chuỗi chỉ định không
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool CompareArrayContainString(string[] arr, string str2)
        {
            string normalizedStr2 = StringHelper.NormalizeText(str2);

            foreach (string str in arr)
            {
                string normalizedStr = StringHelper.NormalizeText(str); // replace xuống dòng thành khoảng trắng
                bool areEqual = normalizedStr.Equals(normalizedStr2, StringComparison.OrdinalIgnoreCase);

                if(areEqual)
                    return areEqual;
            }

            return false;
        }
    }
}
