using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode_2017_Pizza
{
    class PizzaSlice
    {
        public int ID { get; private set; }
        public int StartRow { get; private set; }
        public int EndRow { get; private set; }
        public int StartColumn { get; private set; }
        public int EndColumn { get; private set; }

        public PizzaSlice(int id, int startRow, int endRow, int startColumn, int endColumn)
        {
            this.ID = id;
            this.StartRow = startRow;
            this.StartColumn = startColumn;
            this.EndColumn = endColumn;
            this.EndRow = endRow;
        }

        public int GetSize()
        {
            return (EndRow - StartRow + 1) * (EndColumn - StartColumn + 1);
        }

        public override string ToString()
        {
            return "Slice from [" + StartRow + "]" + "[" + StartColumn + "] to [" + EndRow + "]" + "[" + EndColumn + "]";
        }
    }
}
