using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace MultiplyTwoMatrices
{
    public static class Matrix {
        public const int RowSize = 1000;
        public const int ColSize = 1000;

        public static string GetAsString(long[,] array) {
            int row = array.GetLength(0);
            int col = array.GetLength(1);

            var sb = new StringBuilder();
            for (int i = 0; i < row; i++) {
                for (int j = 0; j < col; j++) {
                    sb.Append(array[i, j]);
                }
            }
            return sb.ToString();
        }

        public static long[,] MultiplyMatrix(int[,] A, int[,] B) {
            var timer = new Stopwatch();
            timer.Start();

            int rA = A.GetLength(0);
            int cA = A.GetLength(1);
            int rB = B.GetLength(0);
            int cB = B.GetLength(1);
            long[,] matrixC = new long[RowSize, ColSize];

            if (cA != rB) {
                Console.WriteLine("Cannot be multiplied!");
                return matrixC;
            }

            Parallel.For(0, rA, i => {
                Parallel.For(0, cB, j => {
                    for (int k = 0; k < cA; k++) {
                        matrixC[i, j] += A[i, k] * B[k, j];
                    }
                });
            });

            timer.Stop();
            Console.WriteLine($"Multiplication: {timer.Elapsed.Seconds} Seconds");
            timer.Reset();

            return matrixC;
        }
    }
}
