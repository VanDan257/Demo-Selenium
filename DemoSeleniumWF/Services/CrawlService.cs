using DemoSeleniumWF.SeleniumHelpers;
using DemoSeleniumWF.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace DemoSeleniumWF.Services
{
    public class CrawlService
    {
        private IWebDriver driver;

        public CrawlService() { }

        public bool OpenConnection()
        {
            try
            {
                // Sử dụng WebDriverManager để tự động tải ChromeDriver phù hợp
                new DriverManager().SetUpDriver(new ChromeConfig());
                ChromeOptions options = new ChromeOptions();

                driver = new ChromeDriver(options);

                driver.Manage().Window.Maximize();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
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

        public string Click(string element)
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
                            return string.Empty;
                        }
                        return "*element not found";
                    }
                    else
                    {
                        var webElement = driver.GetElement(By.CssSelector(element));
                        if (webElement != null)
                        {
                            webElement.Click();
                            return string.Empty;
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
                            webElement.SendKeys("");

                            webElement.SendKeys(value);
                            return string.Empty;
                        }
                        return "*element not found";
                    }
                    else
                    {
                        var webElement = driver.GetElement(By.CssSelector(element));
                        if (webElement != null)
                        {
                            webElement.SendKeys("");

                            webElement.SendKeys(value);
                            return string.Empty;
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
                            return string.Empty;
                        }
                        return "*element not found";
                    }
                    else
                    {
                        var webElement = driver.GetElement(By.CssSelector(element));
                        if (webElement != null)
                        {
                            webElement.Click();
                            return string.Empty;
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
                        var webElement = driver.GetElements(By.XPath(element));
                        if (webElement.Count() > 0)
                        {
                            foreach (var item in webElement)
                            {
                                if (CompareMethods.CompareEqual(item.GetAttribute("value"), value))
                                {
                                    if (!item.Selected)
                                    {
                                        ForceClickUsingJs(item);
                                    }
                                    return string.Empty;
                                }
                            }
                            return "*there is no element with value equal to " + value;
                        }
                        return "*element not found";
                    }
                    else
                    {
                        var webElement = driver.GetElements(By.CssSelector(element));
                        if (webElement.Count() > 0)
                        {
                            foreach (var item in webElement)
                            {
                                if (CompareMethods.CompareEqual(item.GetAttribute("value"), value))
                                {
                                    item.Click();
                                    return string.Empty;
                                }
                            }
                            return "*there is no element with value equal to " + value;
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
                        if (string.IsNullOrEmpty(value))
                        {
                            var webElement = driver.GetElement(By.XPath(element));
                            if(webElement != null && !webElement.Selected)
                            {
                                if (!webElement.Selected)
                                {
                                    ForceClickUsingJs(webElement);
                                }
                                return string.Empty;
                            }
                        }
                        else
                        {
                            var webElement = driver.GetElements(By.XPath(element));
                            if (webElement.Count() > 0)
                            {
                                foreach (var item in webElement)
                                {
                                    var valueItem = item.GetAttribute("value");
                                    if (CompareMethods.CompareArrayContainString(values, valueItem))
                                    {
                                        if (!item.Selected)
                                        {
                                            ForceClickUsingJs(item);
                                        }
                                    }
                                }
                                return string.Empty;
                            }
                            return "*element not found";
                        }
                    }
                    else
                    {
                        var webElement = driver.GetElements(By.CssSelector(element));
                        if (webElement.Count() > 0)
                        {
                            foreach (var item in webElement)
                            {
                                if (CompareMethods.CompareArrayContainString(values, item.GetAttribute("value")))
                                {
                                    if (!item.Selected)
                                    {
                                        ForceClickUsingJs(item);
                                    }
                                }
                            }
                            return string.Empty;
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

        public string DropdownList(string element, string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(element))
                {
                    var firstCharacter = element[0];

                    Thread.Sleep(1000);

                    if (firstCharacter == '/')
                    {
                        var webElement = driver.GetElementsDisplayed(By.XPath(element));
                        if (webElement.Count() > 0)
                        {
                            webElement.First().Click();
                            return string.Empty;
                        }
                        return "*element not found";
                    }
                    else
                    {
                        var webElement = driver.GetElementsDisplayed(By.CssSelector(element));
                        if (webElement.Count() > 0)
                        {
                            webElement.First().Click();
                            return string.Empty;
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

        public void ScrollPageToItem(string element)
        {
            IWebElement webElement;
            var firstCharacter = element[0];
            if (firstCharacter == '/')
            {
                webElement = driver.GetElement(By.XPath(element));
            }
            else
            {
                webElement = driver.GetElement(By.CssSelector(element));
            }

            if(webElement != null)
            {
                // Cuộn tới phần tử bằng JavaScript
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].scrollIntoView(true);", webElement);
            }
        }

        public byte[] ScreenShotPage(IWebElement element)
        {
            // Cuộn tới phần tử bằng JavaScript
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            // Cuộn thêm 300px lên trên để phần tử ở giữa màn hình
            js.ExecuteScript("window.scrollBy(0, -300);");

            // Chờ phần tử hiển thị sau khi scroll
            WebDriverWait wait = new WebDriverWait(new SystemClock(), driver, TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(100));
            wait.Until(drv =>
            {
                try
                {
                    return element.Displayed;
                }
                catch
                {
                    return false;
                }
            });

            // Chụp ảnh màn hình và lưu trữ
            ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
            Screenshot screenshot = screenshotDriver.GetScreenshot();

            // Lưu ảnh chụp vào file
            return screenshot.AsByteArray;
        }

        public byte[] ScreenShotPageBySelector(string selector)
        {
            if (string.IsNullOrEmpty(selector))
                return null;

            IWebElement element = null;
            try
            {
                var firstCharacter = selector[0];
                if (firstCharacter == '/')
                {
                    element = driver.GetElement(By.XPath(selector));
                }
                else
                {
                    element = driver.GetElement(By.CssSelector(selector));
                }

                if (element == null)
                    return null;

                // Cuộn tới phần tử bằng JavaScript
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
                // Cuộn thêm 300px lên trên để phần tử ở giữa màn hình
                js.ExecuteScript("window.scrollBy(0, -300);");

                // Chờ phần tử hiển thị sau khi scroll
                WebDriverWait wait = new WebDriverWait(new SystemClock(), driver, TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(100));
                wait.Until(drv =>
                {
                    try
                    {
                        return element.Displayed;
                    }
                    catch
                    {
                        return false;
                    }
                });

                ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
                Screenshot screenshot = screenshotDriver.GetScreenshot();

                return screenshot.AsByteArray;
            }
            catch
            {
                return null;
            }
        }

        public void MarkElementError(string element)
        {
            // Tìm phần tử lỗi
            IWebElement webElement;
            var firstCharacter = element[0];
            if (firstCharacter == '/')
            {
                webElement = driver.GetElement(By.XPath(element));
            }
            else
            {
                webElement = driver.GetElement(By.CssSelector(element));
            }

            if (webElement != null)
            {
                // Sử dụng JavaScript để thay đổi background-color
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].style.backgroundColor = 'red';", webElement);
                js.ExecuteScript("arguments[0].style.border = '1px solid red';", webElement);
            }
        }

        public void ForceClickUsingJs(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
        }
    }
}
