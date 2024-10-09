using DemoSeleniumWF.Constant;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSeleniumWF.SeleniumHelpers
{
    public class DriverFactory
    {
        private static FirefoxOptions FirefoxOptions
        {
            get
            {
                var firefoxProfile = new FirefoxOptions();
                firefoxProfile.SetPreference("network.automatic-ntlm-auth.trusted-uris", "http://localhost");
                return firefoxProfile;
            }
        }

        public IWebDriver Create()
        {
            IWebDriver driver;
            var driverToUse = DriverToUse.Chrome;

            switch (driverToUse)
            {
                case DriverToUse.InternetExplorer:
                    driver = new InternetExplorerDriver(AppDomain.CurrentDomain.BaseDirectory, new InternetExplorerOptions(), TimeSpan.FromMinutes(5));
                    break;
                case DriverToUse.Firefox:
                    var firefoxProfile = FirefoxOptions;
                    driver = new FirefoxDriver(firefoxProfile);
                    driver.Manage().Window.Maximize();
                    break;
                case DriverToUse.Chrome:
                    driver = new ChromeDriver();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            driver.Manage().Window.Maximize();
            var timeouts = driver.Manage().Timeouts();

            timeouts.ImplicitWait = TimeSpan.FromSeconds(30);
            timeouts.PageLoad = TimeSpan.FromSeconds(60);

            // Suppress the onbeforeunload event first. This prevents the application hanging on a dialog box that does not close.
            ((IJavaScriptExecutor)driver).ExecuteScript("window.onbeforeunload = function(e){};");
            return driver;
        }
    }
}
