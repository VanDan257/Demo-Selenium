using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSeleniumWF.Models
{
    public class IndexTest
    {
        public string TestSheet { get; set; }
        public string Link { get; set; }
        public bool AllowTest { get; set; }
        public bool TestResult{ get; set; }
        public string Note { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
