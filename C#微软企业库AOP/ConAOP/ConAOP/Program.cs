using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConAOP
{
    class Program
    {
        static void Main(string[] args)
        {
            int result = TestTool.GetInstance().Add(1, 2);
            Console.WriteLine("OVER");
            Console.ReadLine();
        }
    }
}
