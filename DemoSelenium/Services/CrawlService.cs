using DemoSelenium.SeleniumHelpers;
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
                options.AddArguments("--headless=new");

                driver = new ChromeDriver(options);
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

                // Thiết lập zoom màn hình
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript($"document.body.style.zoom='58%';");
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
                Console.WriteLine(ex.Message);
                return "";
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
