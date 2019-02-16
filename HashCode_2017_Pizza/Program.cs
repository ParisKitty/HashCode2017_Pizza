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
            Pizza pizza = new Pizza(args[0]);
            int[,] cutPizza = pizza.Cut();
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(args[0] + ".out"))
            {
                for (int i=0; i < pizza.Rows; i++)
                {
                    for (int j = 0; j < pizza.Columns; j++)
                    {
                        sw.Write(cutPizza[i,j]);
                    }
                    sw.WriteLine();
                }
            }
        }
    }
}
