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
            pizza.Cut(int.Parse(args[1]));
            int desertZoneCount = 0;
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(args[0] + ".out"))
            {
                for (int i=0; i < pizza.Rows; i++)
                {
                    for (int j = 0; j < pizza.Columns; j++)
                    {
                        if (pizza.Cells[i, j] == -1)
                        {
                            desertZoneCount++;
                            sw.Write("-");
                        }
                        else {
                            sw.Write(pizza.CutCells[i, j] % 10);
                        }
                    }
                    sw.WriteLine();
                }

                sw.WriteLine();
                sw.Write("-------------------------------------------------");
                sw.WriteLine();
                sw.Write(desertZoneCount + " zones of desert, nearly " + desertZoneCount * pizza.MaxCellsPerSlice +" cells not cut");
            }
        }
    }
}
