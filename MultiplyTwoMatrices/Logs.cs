using System;

namespace MultiplyTwoMatrices
{
    public static class Logs {
        public static void LogMatrixRow(MatrixRowModel row) {
            Console.WriteLine($"Value: {row.Value}");
            foreach (var i in row.Value) {
                Console.WriteLine(i);
            }
        }
    }
}
