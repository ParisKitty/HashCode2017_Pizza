using System;
using System.Collections.Generic;
using System.IO;

namespace HashCode_2017_Pizza
{
    class Pizza
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int[,] Cells { get; set; }
        public int[,] CutCells { get; set; }
        public int MinIngredientPerSlice { get; set; }
        public int MaxCellsPerSlice { get; set; }
        public int CurrentSliceID = 1;
        public int CurrentSliceIDClone = 1;
        Dictionary<int, PizzaSlice> sliceHash = new Dictionary<int, PizzaSlice>();
        Dictionary<int, PizzaSlice> aboveSliceHash = new Dictionary<int, PizzaSlice>();
        public int CurrentStartRow = 0;
        public int CurrentStartRowClone = 0;
        public int CurrentStartColumn = 0;
        public int CurrentStartColumnClone = 0;
        public bool needRecover;

        public Pizza(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
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
            CutCells = new int[Rows, Columns]; // all cells are identified with 0 at the begining
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

        public int[,] Cut()
        {
            using (StreamWriter sw = new StreamWriter("../../input/log"))
            {
                // TODO
                PizzaSlice cutSlice = null;
                bool isNotEnd = true;
                while (isNotEnd)
                {
                    cutSlice = CutOneSlice();
                    if (cutSlice != null)
                    {
                        sw.Write(cutSlice);
                        isNotEnd = RegisterCurrentSlice(cutSlice);
                    }
                    else {
                        sw.Write("Recut From Neigbour -------");
                        cutSlice = RecutFromTheAboveSlice(Direction.LEFT);
                        if (cutSlice == null)
                        {
                            RecoverAboveSlices();
                            cutSlice = RecutFromTheAboveSlice(Direction.TOP);
                        }
                        if (cutSlice == null)
                        {
                            RecoverAboveSlices();
                            cutSlice = RecutFromTheAboveSlice(Direction.RIGHT);
                        }
                        if (cutSlice != null)
                        {
                            sw.Write(cutSlice);
                            isNotEnd = RegisterCurrentSlice(cutSlice);
                        }
                        else
                        {
                            RecoverAboveSlices();
                            sw.Write("Recut From N-1 -------");
                            do
                            {
                                cutSlice = RecutFromTheAboveSlice(Direction.MINUS_ONE);
                            } while (cutSlice == null);
                            sw.Write(cutSlice);
                            isNotEnd = RegisterCurrentSlice(cutSlice);
                        }

                    }
                }
                return CutCells;
            }
        }

        private void RecoverAboveSlices()
        {
            if (!needRecover)
            {
                return;
            }
            foreach (KeyValuePair<int, PizzaSlice> entry in aboveSliceHash)
            {
                setIdentifier(entry.Value, entry.Key);
                sliceHash.Add(entry.Key, entry.Value);
            }
            CurrentStartRow = CurrentStartRowClone;
            CurrentStartColumn = CurrentStartColumnClone;
            CurrentSliceID = CurrentSliceIDClone;
        }

        private PizzaSlice RecutFromTheAboveSlice(Direction direction)
        {
            int aboveSliceID = 0;
            if (direction == Direction.MINUS_ONE)
            {
                aboveSliceID = CurrentSliceID - 1;
            }
            else if (direction == Direction.LEFT)
            {
                if (CurrentStartColumn == 0) {
                    needRecover = false;
                    return null;
                }
                aboveSliceID = CutCells[CurrentStartRow, CurrentStartColumn - 1];
            }
            else if (direction == Direction.TOP)
            {
                if (CurrentStartRow == 0) {
                    needRecover = false;
                    return null;
                }
                aboveSliceID = CutCells[CurrentStartRow - 1, CurrentStartColumn];
            }
            else if (direction == Direction.RIGHT)
            {
                if (CurrentStartRow == Rows - 1) {
                    needRecover = false;
                    return null;
                }
                aboveSliceID = CutCells[CurrentStartRow + 1, CurrentStartColumn];
            }

            if (aboveSliceID == 0)
            {
                needRecover = false;
                return null;
            }

            if (direction != Direction.MINUS_ONE)
            {
                CurrentStartRowClone = CurrentStartRow;
                CurrentStartColumnClone = CurrentStartColumn;
                CurrentSliceIDClone = CurrentSliceID;
                CloneAboveSlices(aboveSliceID);
                needRecover = true;
            }

            PizzaSlice aboveSlice;
            sliceHash.TryGetValue(aboveSliceID, out aboveSlice);
            //if (aboveSlice == null)
            //{
            //    return null;
            //}

            CleanAboveSlices(aboveSliceID);
            CurrentStartColumn = aboveSlice.StartColumn;
            CurrentStartRow = aboveSlice.StartRow;
            CurrentSliceID = aboveSlice.ID;
            if (aboveSlice.EndColumn == aboveSlice.StartColumn) // the above slice is already the last solution for his zone
            {
                return null;
            }
            return RecutOneSlice(aboveSlice.EndColumn - 1);
        }

        private void CleanAboveSlices(int aboveSliceID)
        {
            PizzaSlice sliceToRecut;
            for (int i = aboveSliceID; i < CurrentSliceID; i++)
            {
                sliceHash.TryGetValue(i, out sliceToRecut);
                setIdentifier(sliceToRecut, 0);
                sliceHash.Remove(i);
            }

        }
        private void CloneAboveSlices(int aboveSliceID)
        {
            aboveSliceHash.Clear();
            PizzaSlice sliceToRecut;
            for (int i = aboveSliceID; i < CurrentSliceID; i++)
            {
                sliceHash.TryGetValue(i, out sliceToRecut);
                aboveSliceHash.Add(i,sliceToRecut);
            }

        }

        private PizzaSlice CutOneSlice()
        {
            int firtEndColumn = Math.Min(Columns - 1, CurrentStartColumn + MaxCellsPerSlice - 1);
            return RecutOneSlice(firtEndColumn);
        }

        private PizzaSlice RecutOneSlice(int firstEndColumn)
        {
            int startRow = CurrentStartRow;
            int startColumn = CurrentStartColumn;
            int endColumn = firstEndColumn;
            int endRow = Math.Min(this.Rows - 1, startRow + MaxCellsPerSlice / (endColumn - startColumn + 1) - 1);
            while (endColumn >= startColumn)
            {
                if (IsValidatedSlice(startRow, endRow, startColumn, endColumn))
                {
                    return new PizzaSlice(CurrentSliceID, startRow, endRow, startColumn, endColumn);
                }

                endColumn--;
                if (endColumn >= startColumn)
                {
                    endRow = Math.Min(this.Rows - 1, startRow + MaxCellsPerSlice / (endColumn - startColumn + 1) - 1);
                }
            }
            return null;
        }

        private bool RegisterCurrentSlice(PizzaSlice slice)
        {
            sliceHash.Add(slice.ID, slice);
            setIdentifier(slice, CurrentSliceID);

            CurrentSliceID++;
            CurrentStartColumn = slice.EndColumn + 1;
            if (slice.EndColumn == Columns - 1 || CutCells[CurrentStartRow, CurrentStartColumn] != 0)
            {

                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        if (CutCells[i, j] == 0)
                        {
                            CurrentStartRow = i;
                            CurrentStartColumn = j;
                            return true;
                        }
                    }
                }
                return false;
            }
            return true;
        }

        private void setIdentifier(PizzaSlice slice, int id)
        {
            for (int r = slice.StartRow; r <= slice.EndRow; r++)
            {
                for (int c = slice.StartColumn; c <= slice.EndColumn; c++)
                {
                    CutCells[r, c] = id;
                }
            }
        }

        private void GiveUpCells()
        {
            CutCells[CurrentStartRow, CurrentStartColumn] = -1;
            CurrentStartRow++;            
        }

        private bool IsValidatedSlice(int startRow, int endRow, int startColumn, int endColumn)
        {
            int tomatoNbr = 0;
            for (int r = startRow; r <= endRow; r++)
            {
                for (int c = startColumn; c <= endColumn; c++)
                {
                    if (CutCells[r, c] > 0) return false;
                    tomatoNbr += Cells[r, c];
                }
            }
            int mushroomNbr = (endRow - startRow + 1) * (endColumn - startColumn + 1) - tomatoNbr;
            return tomatoNbr >= MinIngredientPerSlice && mushroomNbr >= MinIngredientPerSlice;
        }
    }

}
