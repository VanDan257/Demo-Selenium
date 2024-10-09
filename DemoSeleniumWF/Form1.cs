using DemoSeleniumWF.Services;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoSeleniumWF
{
    public partial class Form1 : Form
    {
        private readonly FileService fileService;
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        public Form1()
        {
            InitializeComponent();
            fileService = new FileService();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Reset();
            fileService.GetListSheetTest();
        }

        private void Reset()
        {
            labelResultTest.Visible = false;
            linkFileName.Text = "";
            labelResultTest.Text = "";
        }
    }
}
