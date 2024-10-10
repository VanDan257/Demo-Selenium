using DemoSeleniumWF.Constant;
using DemoSeleniumWF.Models;
using DemoSeleniumWF.Utils;
using log4net;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V127.Emulation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSeleniumWF.Services
{
    public class FileService
    {
        public CrawlService crawlService = new CrawlService();

        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        // đường dẫn file excel gốc
        private string fileName = "automationTest-report.xlsx";
        private string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "file_run/");

        // đường dẫn file excel template
        string templateFileName = "automationTest-report-template.xlsx";
        string templateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "template_test_case/");

        // đường dẫn file lưu trữ danh sách tên file template
        string pathDataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data/data.json");

        public FileService() { }

        public void GetListSheetTest()
        {
            crawlService.OpenConnection();

            try
            {
                List<IndexTest> listSheetTest = new List<IndexTest>();
                // Kiểm tra xem file có tồn tại không
                if (File.Exists(filePath + fileName))
                {
                    using (var package = new ExcelPackage(new FileInfo(filePath + fileName)))
                    {
                        // Lấy worksheet đầu tiên - worksheet index
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        int rowCount = 2;
                        // Lấy tất cả các bản ghi có AllowTest = Yes trong sheet
                        while (worksheet.Cells[rowCount, (int)EnumIndexSheet.ColAllowTest].Value != null && !string.IsNullOrEmpty(worksheet.Cells[rowCount, (int)EnumIndexSheet.ColAllowTest].Text))
                        {
                            var allowTest = worksheet.Cells[rowCount, (int)EnumIndexSheet.ColAllowTest].Text;
                            if (allowTest == "Yes")
                            {
                                var siteUrl = worksheet.Cells[rowCount, (int)EnumIndexSheet.ColLink].Text;

                                // Navigate to Site Url
                                crawlService.NavigateToUrl(siteUrl);

                                var sheetName = worksheet.Cells[rowCount, (int)EnumIndexSheet.ColTestSheet].Text;

                                // Lấy sheet theo sheetName
                                ExcelWorksheet worksheetTest = package.Workbook.Worksheets[sheetName];

                                var resultTestSheet = HandleTest(package, worksheetTest, sheetName);

                                if (resultTestSheet != null)
                                {
                                    InsertValueToTestSheet(worksheet, rowCount, (int)EnumIndexSheet.ColNote, (int)EnumIndexSheet.ColTestResult, resultTestSheet.Note, resultTestSheet.TestResult);
                                }

                            }
                            rowCount++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error: " + ex.Message + "\n");
            }
        }

        public IndexTest HandleTest(ExcelPackage package, ExcelWorksheet worksheet, string sheetName)
        {
            List<IWebElement> listPageReport = new List<IWebElement>();
            var heightImage = 45; // chiều cao 1 ảnh chiếm 40 row

            int rowCount = 2; // check từ row thứ 2
            int colValueActualIndex = 6; // column Value actual

            var resultTestSheet = new IndexTest // kết quả test report
            {
                TestResult = true,
                Note = ""
            };
            // Duyệt qua tất cả các dòng cột Items test có giá trị trong sheet
            while (worksheet.Cells[rowCount, (int)EnumTestSheet.ColItemTest].Value != null && !string.IsNullOrEmpty(worksheet.Cells[rowCount, (int)EnumTestSheet.ColItemTest].Text))
            {
                string itemTest = worksheet.Cells[rowCount, (int)EnumTestSheet.ColItemTest].Text.Trim();
                var valueActual = crawlService.GetText(itemTest);

                bool testResult = false; // test result item

                string valueExpected = worksheet.Cells[rowCount, (int)EnumTestSheet.ColValueExpected].Text.Trim();

                var methodTest = worksheet.Cells[rowCount, (int)EnumTestSheet.ColMethodTest].Text;
                switch (methodTest)
                {
                    case MethodTest.CompareContain:
                        testResult = CompareMethods.CompareContain(valueActual, valueExpected);
                        break;
                    case MethodTest.CompareEqual:
                        testResult = CompareMethods.CompareEqual(valueActual, valueExpected);
                        break;
                    case MethodTest.CompareNumber:
                        testResult = CompareMethods.CompareNumber(valueActual, valueExpected);
                        break;
                    case MethodTest.Input:
                        valueActual = crawlService.InputValue(itemTest, valueExpected);
                        if (string.IsNullOrEmpty(valueActual))
                            testResult = true;
                        break;
                    case MethodTest.Click:
                        valueActual = crawlService.Click(itemTest);
                        if (string.IsNullOrEmpty(valueActual))
                            testResult = true;
                        break;
                    case MethodTest.Radio:
                        valueActual = crawlService.Radio(itemTest, valueExpected);
                        if (string.IsNullOrEmpty(valueActual))
                            testResult = true;
                        break;
                        
                    case MethodTest.DropdownList:
                        valueActual = crawlService.DropdownList(itemTest, valueExpected);
                        if (string.IsNullOrEmpty(valueActual))
                            testResult = true;
                        break;
                        
                    case MethodTest.Checkbox:
                        valueActual = crawlService.Checkbox(itemTest, valueExpected);
                        if (string.IsNullOrEmpty(valueActual))
                            testResult = true;
                        break;

                    default:
                        testResult = CompareMethods.CompareEqual(valueActual, valueExpected);
                        break;
                }

                InsertValueToTestSheet(worksheet, rowCount, colValueActualIndex, colValueActualIndex + 1, valueActual, testResult);

                if (!testResult)
                {
                    resultTestSheet.TestResult = false;
                    resultTestSheet.Note += $"- Item test: {itemTest} \nValue expected: {valueExpected} \nValue actual: {valueActual} \nMethod test: {methodTest} \n";
                    crawlService.MarkElementError(itemTest);

                    // chụp màn hình với mỗi page report có item test error
                    var pageReport = crawlService.GetPageContainElement(itemTest);
                    if (pageReport != null)
                    {
                        var indexPageReport = listPageReport.IndexOf(pageReport);
                        if (indexPageReport == -1)
                        {
                            listPageReport.Add(pageReport);
                        }
                        else
                        {
                            listPageReport[indexPageReport] = pageReport;
                        }
                    }
                }

                rowCount++;
            }

            return resultTestSheet;
        }

        public void InsertValueToTestSheet(ExcelWorksheet worksheet, int row, int colValue, int colTest, string value, bool check)
        {
            worksheet.Cells[row, colValue].Value = value;
            worksheet.Cells[row, colTest].Value = check;

            var cells = worksheet.Cells[row, colTest];
            cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
            if (check)
            {
                cells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Green); // thiết lập màu nền là xanh với kết quả test là true
            }
            else
            {
                cells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red); // thiết lập màu nền là đỏ với kết quả test là false
            }

        }

        public void InsertImageToExcel(ExcelWorksheet worksheet, byte[] imageBytes, int setPositionRow, int setPositionColum, string fileName)
        {
            // Kiểm tra mảng byte không null và có kích thước hợp lệ
            if (imageBytes != null && imageBytes.Length != 0)
            {
                using (var memoryStream = new MemoryStream(imageBytes))
                {
                    // Thêm hình ảnh vào worksheet
                    var imagePackage = worksheet.Drawings.AddPicture(fileName, memoryStream, ePictureType.Jpg);
                    imagePackage.SetPosition(setPositionRow, 0, setPositionColum, 0);
                }
            }
        }

        public void UploadTemplateFile(string fileSource, string fileTarget, string fileNameTarget)
        {
            var newFile = new TestFile
            {
                FileName = fileNameTarget,
                DateModified = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")
            };

            // inser đối tượng vào file json
            if (!File.Exists(pathDataFile))
            {
                // Nếu file không tồn tại, tạo file mới và viết đối tượng vào
                string initialJson = JsonConvert.SerializeObject(new[] { newFile }, Formatting.Indented);
                File.WriteAllText(pathDataFile, initialJson);
            }
            else
            {
                string jsonContent = File.ReadAllText(pathDataFile);
                var testFiles = JsonConvert.DeserializeObject<List<TestFile>>(jsonContent);

                var existingFile = testFiles.FirstOrDefault(x => x.FileName == fileName);
                if (existingFile == null)
                {
                    testFiles.Add(newFile);
                }
                else
                {
                    existingFile.DateModified = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                }

                // Ghi lại dữ liệu vào file JSON
                string updatedJson = JsonConvert.SerializeObject(testFiles, Formatting.Indented);
                File.WriteAllText(pathDataFile, updatedJson);

                File.Copy(fileTarget, templateFilePath + fileNameTarget, true); // true để ghi đè nếu file đã tồn tại
            }
        }
    }
}
