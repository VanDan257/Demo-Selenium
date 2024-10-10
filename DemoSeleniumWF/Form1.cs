using DemoSeleniumWF.Constant;
using DemoSeleniumWF.Models;
using DemoSeleniumWF.Services;
using DemoSeleniumWF.Utils;
using DemoSeleniumWF;
using log4net;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace AutoTest
{
    public partial class Form1 : Form
    {
        public static CrawlService crawlService = new CrawlService();
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        public Form1()
        {
            InitializeComponent();

            dataGridView1.MultiSelect = false; // không cho select nhiều cell cùng 1 lúc
        }

        // đường dẫn file excel gốc
        private string fileName = "automationTest-report.xlsx";
        private string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "file_run/automationTest-report.xlsx");

        // đường dẫn file excel template
        string templateFileName = "automationTest-report-template.xlsx";
        string templateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "template_test_case/automationTest-report.xlsx");

        // đường dẫn file lưu trữ danh sách tên file template
        string pathDataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data/data.json");

        // đường dẫn file lưu trữ log lỗi
        string pathErrorLogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs/log-error.txt");

        private void button1_Click(object sender, EventArgs e)
        {
            Reset();

            if (!string.IsNullOrEmpty(linkFileName.Text))
            {
                // Hiển thị hộp thoại xác nhận
                DialogResult result = MessageBox.Show(
                    "Previous test results will be lost, do you still want to continue?",    // Nội dung thông báo
                    "Run test",                           // Tiêu đề của hộp thoại
                    MessageBoxButtons.YesNo,                  // Nút Yes/No
                    MessageBoxIcon.Question                   // Biểu tượng dấu hỏi
                );

                // Kiểm tra phản hồi của người dùng
                if (result == DialogResult.Yes)
                {
                    RunTest();
                }
            }
            else
            {
                RunTest();
            }
        }

        public void RunTest()
        {
            crawlService.OpenConnection();

            try
            {
                if (dataGridView1.CurrentRow != null && dataGridView1.CurrentCell != null)
                {
                    int rowIndex = dataGridView1.CurrentRow.Index;

                    var fileNameSelected = dataGridView1.Rows[rowIndex].Cells[1].Value;

                    templateFilePath = templateFilePath.Split('/')[0];
                    templateFilePath += "/" + fileNameSelected;

                    File.Copy(templateFilePath, filePath, true); // true để ghi đè nếu file đã tồn tại
                }

                // Kiểm tra xem file có tồn tại không
                if (File.Exists(filePath))
                {
                    using (var package = new ExcelPackage(new FileInfo(filePath)))
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

                                var resultTestSheet = HandleSheetTest(package, worksheetTest, sheetName);

                                if (resultTestSheet != null)
                                {
                                    InsertValueToTestSheet(worksheet, rowCount, (int)EnumIndexSheet.ColNote, (int)EnumIndexSheet.ColTestResult, resultTestSheet.Note, resultTestSheet.TestResult);
                                }

                            }
                            rowCount++;
                        }
                        // Lưu lại file Excel sau khi chèn dữ liệu
                        package.Save();
                    }
                }
                else
                {
                    string note = "File not found!";
                    setLabelResultTest(Color.Red, note, true);
                }
            }
            catch (IOException ex)
            {
                setLabelResultTest(Color.Red, "The process cannot access the file, make sure the file is closed before running!", true);
                log.Error("Error: " + ex.Message + "\n");

                labelViewFile.Text = "View log: ";
                linkFileName.Text = pathErrorLogFile;
            }
            catch (Exception ex)
            {
                setLabelResultTest(Color.Red, "Error: " + ex.Message, true);
                log.Error("Error: " + ex.Message + "\n");

                labelViewFile.Text = "View log: ";
                linkFileName.Text = pathErrorLogFile;
            }
            finally
            {
                crawlService.CloseConnection();
            }
        }

        public ResultTestSheet HandleSheetTest(ExcelPackage package, ExcelWorksheet worksheet, string sheetName)
        {
            List<IWebElement> listPageReport = new List<IWebElement>();
            var heightImage = 45; // chiều cao 1 ảnh chiếm 40 row

            int rowCount = 2; // check từ row thứ 2

            var resultTestSheet = new ResultTestSheet // kết quả test report
            {
                TestResult = true
            };

            var totalItemTest = 0;
            var totalItemError = 0;
            var totalItemPass = 0;

            // Duyệt qua tất cả các dòng cột Items test có giá trị trong sheet
            while (worksheet.Cells[rowCount, (int)EnumTestSheet.ColItemTest].Value != null && !string.IsNullOrEmpty(worksheet.Cells[rowCount, (int)EnumTestSheet.ColItemTest].Text))
            {
                totalItemTest++;
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

                InsertValueToTestSheet(worksheet, rowCount, (int)EnumTestSheet.ColValueActual, (int)EnumTestSheet.ColValueActual + 1, valueActual, testResult);

                if (!testResult)
                {
                    totalItemError++;
                    resultTestSheet.TestResult = false;
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

            // Tạo sheet evidence nếu có case lỗi
            if (listPageReport.Count > 0)
            {
                var newSheetName = "Evidence-" + sheetName;
                var existingSheet = package.Workbook.Worksheets[newSheetName];
                // Nếu sheet đã tồn tại thì xóa nó
                if (existingSheet != null)
                {
                    package.Workbook.Worksheets.Delete(existingSheet);
                }

                var newWorksheet = package.Workbook.Worksheets.Add(newSheetName);

                package.Workbook.Worksheets.MoveAfter(newSheetName, sheetName); // di chuyển sheet mới đến vị trí sau sheet test

                for (int i = 0; i < listPageReport.Count; i++)
                {
                    var image = crawlService.ScreenShotPage(listPageReport[i]);
                    if (image != null)
                    {
                        InsertImageToExcel(newWorksheet, image, (i * heightImage + 1), 1, "page_report_" + i);
                    }
                }
            }

            totalItemPass = totalItemTest - totalItemError;
            resultTestSheet.Note = $"Total number of items tested: {totalItemTest} \nTotal number of error items: {totalItemError} \nTotal number of item passed: {totalItemPass}";
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
        public void RemoveImageByName(ExcelWorksheet worksheet, string imageName)
        {
            // Lấy tất cả các hình ảnh có tên chứa chuỗi namePattern
            var imagesToRemove = worksheet.Drawings
                                         .Where(drawing => drawing.Name.Contains(imageName))
                                         .ToList();

            // Xóa từng hình ảnh tìm thấy
            foreach (var image in imagesToRemove)
            {
                worksheet.Drawings.Remove(image);
            }
        }

        public void Reset()
        {
            labelViewFile.Text = "View result: ";
            linkFileName.Text = "";
            setLabelResultTest(Color.Black, "", false);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var link = linkFileName.Text;
                if (linkFileName.Text == fileName)
                {
                    link = filePath;
                }

                // Mở file với ứng dụng mặc định của hệ thống
                Process.Start(new ProcessStartInfo
                {
                    FileName = link, // Lấy đường dẫn từ LinkLabel
                    UseShellExecute = true // Sử dụng shell để mở file
                });
            }
            catch (Exception ex)
            {
                setLabelResultTest(Color.Red, $"Error: {ex.Message}", true);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.ReadOnly = true;

                if (File.Exists(pathDataFile))
                {
                    // Đọc nội dung của file JSON
                    string jsonContent = File.ReadAllText(pathDataFile);
                    List<TestFile> data = JsonConvert.DeserializeObject<List<TestFile>>(jsonContent);

                    // fill data into dataGridView
                    for (int i = 0; i < data.Count; i++)
                    {
                        var dateModified = DateTime.Parse(data[i].DateModified);
                        dataGridView1.Rows.Add(i + 1, data[i].FileName, dateModified.ToString("MM/dd/yyyy HH:mm:ss"), "X");
                    }
                }
            }
            catch (Exception ex)
            {
                setLabelResultTest(Color.Red, $"Error: {ex.Message}", true);
                log.Error("Error: " + ex.Message);
            }
        }

        private void uploadFile_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog1.FileName;
                    linkFileName.Text = selectedFilePath;

                    string fileName = Path.GetFileName(selectedFilePath);
                    templateFilePath = templateFilePath.Split('/')[0];
                    templateFilePath += "/" + fileName;

                    var newFile = new TestFile
                    {
                        FileName = fileName,
                        DateModified = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")
                    };

                    // Kiểm tra xem file có tồn tại không
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
                    }

                    File.Copy(selectedFilePath, templateFilePath, true); // true để ghi đè nếu file đã tồn tại

                    ReloadDataGridView();
                }
            }
            catch (Exception ex)
            {
                setLabelResultTest(Color.Red, $"Error: {ex.Message}", true);

                log.Error("Error: " + ex.Message);
            }
        }

        private void ReloadDataGridView()
        {
            try
            {
                Reset();

                // Clear dữ liệu của dataGridView1
                dataGridView1.Rows.Clear();

                if (File.Exists(pathDataFile))
                {
                    // Đọc nội dung của file JSON
                    string jsonContent = File.ReadAllText(pathDataFile);
                    List<TestFile> data = JsonConvert.DeserializeObject<List<TestFile>>(jsonContent);

                    // fill data into dataGridView
                    for (int i = 0; i < data.Count; i++)
                    {
                        var dateModified = DateTime.Parse(data[i].DateModified);
                        dataGridView1.Rows.Add(i + 1, data[i].FileName, dateModified.ToString("MM/dd/yyyy HH:mm:ss"), "X");
                    }
                }
            }
            catch (Exception ex)
            {
                setLabelResultTest(Color.Red, $"Error: {ex.Message}", true);

                log.Error("Error: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Kiểm tra xem chỉ số hàng có hợp lệ hay không (>= 0) để tránh click vào header
                if (e.RowIndex >= 0)
                {
                    // Lấy chỉ số của cột cuối cùng
                    int lastColumnIndex = dataGridView1.Columns.Count - 1;
                    // Kiểm tra nếu cột được click là cột cuối cùng, xóa file test
                    if (e.ColumnIndex == lastColumnIndex)
                    {
                        // Hiển thị hộp thoại xác nhận
                        DialogResult result = MessageBox.Show(
                            "Are you sure you want to delete this test file?",    // Nội dung thông báo
                            "Delete test file",                           // Tiêu đề của hộp thoại
                            MessageBoxButtons.YesNo,                  // Nút Yes/No
                            MessageBoxIcon.Question                   // Biểu tượng dấu hỏi
                        );

                        // Kiểm tra phản hồi của người dùng
                        if (result == DialogResult.Yes)
                        {
                            // Lấy giá trị của ô File Name
                            var fileNameClicked = dataGridView1.Rows[e.RowIndex].Cells[1].Value;

                            if (fileNameClicked != null)
                            {
                                templateFilePath = templateFilePath.Split('/')[0];
                                templateFilePath += "/" + fileNameClicked;

                                string jsonContent = File.ReadAllText(pathDataFile);
                                var testFiles = JsonConvert.DeserializeObject<List<TestFile>>(jsonContent);

                                var fileDelete = testFiles.FirstOrDefault(f => f.FileName == fileNameClicked.ToString());
                                testFiles.Remove(fileDelete);

                                // Ghi lại dữ liệu vào file JSON
                                string updatedJson = JsonConvert.SerializeObject(testFiles, Formatting.Indented);
                                File.WriteAllText(pathDataFile, updatedJson);

                                ReloadDataGridView();

                                if (File.Exists(templateFilePath))
                                {
                                    File.Delete(templateFilePath);
                                }
                                else
                                {
                                    setLabelResultTest(Color.Red, $"{fileNameClicked} cannot be found", true);
                                    log.Error($"{fileNameClicked} cannot be found");
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                setLabelResultTest(Color.Red, $"Error: {ex.Message}", true);
                log.Error("Error: " + ex.Message);
            }
        }

        private void setLabelResultTest(Color color, string note, bool isVisible)
        {
            labelResultTest.ForeColor = color;
            labelResultTest.Text = note;
            labelResultTest.Visible = isVisible;
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int colFileName = 1;
            // Kiểm tra nếu cột được click là cột file name, mở file template
            if (e.ColumnIndex == colFileName)
            {
                var fileNameClicked = dataGridView1.Rows[e.RowIndex].Cells[1].Value;
                templateFilePath = templateFilePath.Split('/')[0];

                templateFilePath += "/" + fileNameClicked;

                // Mở file với ứng dụng mặc định của hệ thống
                Process.Start(new ProcessStartInfo
                {
                    FileName = templateFilePath, // Lấy đường dẫn từ LinkLabel
                    UseShellExecute = true // Sử dụng shell để mở file
                });
            }
        }
    }
}
