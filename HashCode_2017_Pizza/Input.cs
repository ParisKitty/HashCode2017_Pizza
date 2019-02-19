using System;
using System.Collections.Generic;
using System.IO;

namespace HashCode_2017_Pizza
{
    class Input
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int[,] Cells { get; set; }
        public int MinIngredientPerSlice { get; set; }
        public int MaxCellsPerSlice { get; set; }
        public Pizza[,] Pizzas { get; set; }
        public int XPieces { get; private set; }
        public int YPieces { get; private set; }
        public string FileName { get; set; }

        public Input(string fileName)
        {
            FileName = fileName;
            using (StreamReader sr = new StreamReader(FileName))
            {
                LoadDataFrom(sr);
            }
        }

        private void LoadDataFrom(StreamReader sr)
        {
            string line = sr.ReadLine();
            string[] parts = line.Split(' ');
            Rows = int.Parse(parts[0]);
            Columns = int.Parse(parts[1]);
            MinIngredientPerSlice = int.Parse(parts[2]);
            MaxCellsPerSlice = int.Parse(parts[3]);
            Cells = new int[Rows, Columns];
            for (int r = 0; r < Rows; r++)
            {
                line = sr.ReadLine();
                for (int c = 0; c < Columns; c++)
                {
                    // tomato cell is 1
                    if (line[c] == 'T')
                    {
                        Cells[r, c] = 1;
                    }
                    // mushroom cell is 0
                }
            }
        }

        public void SplitInto(int rowPerPiece, int columnPerPiece)
        {
            XPieces = Rows / rowPerPiece;
            YPieces = Columns / columnPerPiece;
            Pizzas = new Pizza[XPieces, YPieces];
            for (int x = 0; x < XPieces; x++)
            {
                for (int y = 0; y < YPieces; y++)
                {
                    Pizzas[x, y] = new Pizza(rowPerPiece, columnPerPiece, MinIngredientPerSlice, MaxCellsPerSlice);
                    for (int r = 0; r < rowPerPiece; r++)
                    {
                        for (int c = 0; c < columnPerPiece; c++)
                        {
                            Pizzas[x, y].Cells[r, c] = Cells[r + x * rowPerPiece, c + y * columnPerPiece];
                        }
                    }
                }
            }
        }

        //public void Cut(int maxBackCount)
        //{
        //    for (int x = 0; x < XPieces; x++)
        //    {
        //        for (int y = 0; y < YPieces; y++)
        //        {
        //        }
        //    }
        //}

        public void PrintResults(int maxBackCount)
        {
            int totalDesertZoneCount = 0;
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(FileName + ".total.out"))
            {
                for (int x = 0; x < XPieces; x++)
                {
                    for (int y = 0; y < YPieces; y++)
                    {
                        Pizzas[x, y].Cut(maxBackCount);
                        int desertZoneCount = Pizzas[x, y].PrintResult(FileName + "." + x + "." + y + ".out");
                        totalDesertZoneCount += desertZoneCount;
                        sw.Write("Pizza[" + x + "][" + y + "] : " + desertZoneCount + " zones of desert");
                        sw.WriteLine();
                        sw.Write("-------------------------------------------------");
                        sw.WriteLine();
                    }
                }
                sw.WriteLine();
                sw.Write("Total : " + totalDesertZoneCount + " zones of desert, nearly " + totalDesertZoneCount * MaxCellsPerSlice + " cells not cut");
            }
        }
    }
}
