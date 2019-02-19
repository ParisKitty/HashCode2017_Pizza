using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode_2017_Pizza
{
    class Program
    {
        static void Main(string[] args)
        {
            Input input = new Input(args[0]);
            input.SplitInto(int.Parse(args[1]), int.Parse(args[2]));
            //input.Cut(int.Parse(args[3]));
            input.PrintResults(int.Parse(args[3]));
        }
    }
}
