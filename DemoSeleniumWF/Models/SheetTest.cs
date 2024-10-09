using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSeleniumWF.Models
{
    public class SheetTest
    {
        public string ItemTest { get; set; }
        public string MethodTest { get; set; }
        public string ValueExpected { get; set; }
        public string ValueActual { get; set; }
        public string TestResult { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int IndexSheet { get; set; }
    }
}
