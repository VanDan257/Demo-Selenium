using DemoSelenium.SeleniumHelpers;
using DemoSelenium.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSelenium.Services
{
    public class CrawlService
    {
        private IWebDriver driver;

        public CrawlService() { }

        public void OpenConnection()
        {
            try
            {
                ChromeOptions options = new ChromeOptions();

                driver = new ChromeDriver(options);
                driver.Manage().Window.Maximize();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void NavigateToUrl(string url)
        {
            try
            {
                driver.Navigate().GoToUrl(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (driver != null)
                {
                    driver.Close();
                    driver.Quit();
                    driver = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public string GetText(string element)
        {
            try
            {
                if (!string.IsNullOrEmpty(element))
                {
                    var firstCharacter = element[0];
                    if (firstCharacter == '/')
                    {
                        var webElement = driver.GetElement(By.XPath(element));
                        if (webElement != null)
                        {
                            return webElement.Text.Trim();
                        }
                        return "*element not found";
                    }
                    else
                    {

                        var webElement = driver.GetElement(By.CssSelector(element));
                        if (webElement != null)
                        {
                            return webElement.Text.Trim();
                        }
                        return "*element not found";
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public string InputValue(string element, string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(element))
                {
                    var firstCharacter = element[0];
                    if (firstCharacter == '/')
                    {
                        var webElement = driver.GetElement(By.XPath(element));
                        if (webElement != null)
                        {
                            webElement.SendKeys(value);
                            return "";
                        }
                        return "*element not found";
                    }
                    else
                    {
                        var webElement = driver.GetElement(By.CssSelector(element));
                        if (webElement != null)
                        {
                            webElement.SendKeys(value);
                            return "";
                        }
                        return "*element not found";
                    }
                }
                return "*element not found";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public string Button(string element)
        {
            try
            {
                if (!string.IsNullOrEmpty(element))
                {
                    var firstCharacter = element[0];
                    if (firstCharacter == '/')
                    {
                        var webElement = driver.GetElement(By.XPath(element));
                        if (webElement != null)
                        {
                            webElement.Click();
                            return "";
                        }
                        return "*element not found";
                    }
                    else
                    {
                        var webElement = driver.GetElement(By.CssSelector(element));
                        if (webElement != null)
                        {
                            webElement.Click();
                            return "";
                        }
                        return "*element not found";
                    }
                }
                return "*element not found";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public string Radio(string element, string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(element))
                {
                    var firstCharacter = element[0];
                    if (firstCharacter == '/')
                    {
                        var webElement = driver.GetElementsDisplayed(By.XPath(element));
                        if (webElement.Count() > 0)
                        {
                            foreach (var item in webElement)
                            {
                                if (CompareMethods.CompareEqual(item.GetAttribute("value"), value))
                                {
                                    item.Click();
                                }
                            }
                            return "*there is no element with value equal to " + value;
                        }
                        return "*element not found";
                    }
                    else
                    {
                        var webElement = driver.GetElementsDisplayed(By.CssSelector(element));
                        if (webElement.Count() > 0)
                        {
                            foreach (var item in webElement)
                            {
                                if (CompareMethods.CompareEqual(item.GetAttribute("value"), value))
                                {
                                    item.Click();
                                }
                            }
                        }
                        return "*element not found";
                    }
                }
                return "*element not found";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public string Checkbox(string element, string value)
        {
            try
            {
                var values = value.Split(',');
                if (!string.IsNullOrEmpty(element))
                {
                    var firstCharacter = element[0];
                    if (firstCharacter == '/')
                    {
                        var webElement = driver.GetElementsDisplayed(By.XPath(element));
                        if (webElement.Count() > 0)
                        {
                            foreach (var item in webElement)
                            {
                                if (CompareMethods.CompareArrayContainString(values, item.GetAttribute("value")))
                                {
                                    item.Click();
                                }
                            }

                        }
                        return "*element not found";
                    }
                    else
                    {
                        var webElement = driver.GetElementsDisplayed(By.CssSelector(element));
                        if (webElement.Count() > 0)
                        {
                            foreach (var item in webElement)
                            {
                                if (CompareMethods.CompareArrayContainString(values, item.GetAttribute("value")))
                                {
                                    item.Click();
                                }
                            }
                        }
                        return "*element not found";
                    }
                }
                return "*element not found";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public IWebElement GetPageContainElement(string element)
        {
            // Tìm phần tử con
            IWebElement childElement;
            var firstCharacter = element[0];
            if (firstCharacter == '/')
            {
                childElement = driver.GetElement(By.XPath(element));
            }
            else
            {
                childElement = driver.GetElement(By.CssSelector(element));
            }

            if (childElement != null)
            {
                // Sử dụng JavaScript để lấy phần tử cha
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                //IWebElement parentElement = (IWebElement)js.ExecuteScript("return arguments[0].parentNode;", childElement);
                IWebElement parentElement = childElement.FindElement(By.XPath("ancestor::*[contains(@class, 'report-container-pdf')]"));

                return parentElement;
            }

            return null;
        }

        public byte[] ScreenShotPage(IWebElement element)
        {
            // Cuộn tới phần tử bằng JavaScript
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);

            // Thiết lập zoom màn hình
            js.ExecuteScript($"document.body.style.zoom='58%';");

            Thread.Sleep(500); // chờ trình duyệt scroll tới page cần chụp

            // Chụp ảnh màn hình và lưu trữ
            ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
            Screenshot screenshot = screenshotDriver.GetScreenshot();

            // Lưu ảnh chụp vào file
            return screenshot.AsByteArray;
        }
    }
}
