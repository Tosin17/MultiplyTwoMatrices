using System;
using System.Threading.Tasks;

namespace MultiplyTwoMatrices
{
    public class MatrixRowModel {
        public int[] Value { get; set; }
        public int RowIndex { get; set; } = 0;
        public string Cause { get; set; }
        public bool Success { get; set; }
    }

}
