using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSelenium.SeleniumHelpers
{
    public class SeleniumUtils
    {
        private readonly IWebDriver _driver;
        public SeleniumUtils(IWebDriver driver)
        {
            _driver = driver;
        }
        public bool IsElementPresent( By by)
        {
            try
            {
                _driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public IWebElement GetElement(By by)
        {
            try
            {
                if (IsElementPresent(by))
                {
                    return _driver.FindElement(by);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
