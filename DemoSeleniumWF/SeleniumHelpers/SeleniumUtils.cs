using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSeleniumWF.SeleniumHelpers
{
    public static class SeleniumUtils
    {
        /// <summary>
        /// Kiểm tra có phần tử đó hay không
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public static bool IsElementPresent(this IWebDriver driver, By by)
        {
            try
            {
                var item = driver.FindElement(by);
                return item != null;
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra phần tử đó có được hiển thị hay không
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public static bool IsElementDisplayed(this IWebDriver driver, By by)
        {
            try
            {
                var item = driver.FindElement(by);

                return item.Displayed == true;
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Lấy phần tử đầu tiên được tìm thấy
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public static IWebElement GetElement(this IWebDriver driver, By by)
        {
            try
            {
                if (driver.IsElementPresent(by))
                {
                    return driver.FindElement(by);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Lấy tất cả phần tử được tìm thấy
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public static IEnumerable<IWebElement> GetElements(this IWebDriver driver, By by)
        {
            try
            {
                return driver.FindElements(by);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Lấy tất cả phần tử được hiển thị
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public static IEnumerable<IWebElement> GetElementsDisplayed(this IWebDriver driver, By by)
        {
            var elements = GetElements(driver, by);
            foreach (var element in elements)
            {
                yield return element;
            }
        }
    }
}
