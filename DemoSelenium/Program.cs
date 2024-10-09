
using DemoSelenium.Services;
using OfficeOpenXml;

namespace DemoSelenium
{
    public class Program
    {
        public static CrawlService crawlService = new CrawlService();

        public static void Main(string[] args)
        {

            string filePath = "file_run/automationTest-report.xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            crawlService.OpenConnection();

        }
    }
}

