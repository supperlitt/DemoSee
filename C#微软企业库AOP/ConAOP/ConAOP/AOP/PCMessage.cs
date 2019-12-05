using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConAOP
{
    public class PCMessage
    {
        public int monitor_index { get; set; }

        public int line_index { get; set; }

        public string class_name { get; set; }

        public string method_name { get; set; }

        public string[] args { get; set; }

        public string result { get; set; }
    }
}
