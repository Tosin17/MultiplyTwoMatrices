using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace MultiplyTwoMatrices
{
    class Program
    {
        static int[,] matrixA = new int[Matrix.RowSize, Matrix.ColSize];
        static int[,] matrixB = new int[Matrix.RowSize, Matrix.ColSize];

        static void Main(string[] args)
        {
            Console.WriteLine("Starting.......................");
            var timer = new Stopwatch();
            timer.Start();

            var result = RunAsync().Result;
            timer.Stop();
            timer.Reset();
            
            var content = GetAsString(result);

            var hashed = GetMd5Hash(content);
            Console.WriteLine(hashed);
        }

        private static long[,] MultiplyMatrix(int[,] A, int[,] B)
        {
            var timer = new Stopwatch();
            timer.Start();

            int rA = A.GetLength(0);
            int cA = A.GetLength(1);
            int rB = B.GetLength(0);
            int cB = B.GetLength(1);
            long[,] matrixC = new long[Matrix.RowSize, Matrix.ColSize];

            if (cA != rB)
            {
                Console.WriteLine("Cannot be multiplied!");
                return matrixC;
            }

            Parallel.For(0, rA, i =>
            {
                Parallel.For(0, cB, j =>
                {
                    for (int k = 0; k < cA; k++)
                    {
                        matrixC[i, j] += A[i, k] * B[k, j];
                    }
                });
            });

            timer.Stop();
            Console.WriteLine($"Multiplication: {timer.Elapsed.Seconds} Seconds");
            timer.Reset();

            return matrixC;
        }

        static string GetMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                    sBuilder.Append(data[i].ToString("x2"));

                return sBuilder.ToString();
            }
        }

        public static async Task<long[,]> RunAsync()
        {
            matrixA = await GetMatrixData("A");
            matrixB = await GetMatrixData("B");
            return MultiplyMatrix(matrixA, matrixB);
        }

        public static async Task<int[,]> GetMatrixData(string matrixName)
        {
            int[,] output = new int[Matrix.RowSize, Matrix.ColSize];
            ApiClient.initializeApiClient();
            try
            {
                MatrixInitializeModel init = await MatricesApi.InitMatrixAsync("api/numbers/init/1000");
                var tasks = new List<Task<MatrixRowModel>>();

                var timer = new Stopwatch();
                timer.Start();

                var batchSize = 100;
                int numberOfBatches = (int)Math.Ceiling((double)Matrix.RowSize / batchSize);

                for (int i = 0; i < numberOfBatches; i++)
                {
                    int start = i * batchSize;
                    var value = Enumerable.Range(start, batchSize).Select(k => MatricesApi.GetMatrixAsync($"api/numbers/{matrixName}/row/{k}", k));                    
                    tasks.AddRange(value);
                }

                var result = (await Task.WhenAll(tasks)).Select(m => m);

                Parallel.For(0, result.Count(), y =>
                {
                    int[] values = result.Where(x => x.RowIndex == y).Select(x => x.Value).First();
                    Buffer.BlockCopy(values, 0, output, Matrix.ColSize * y, values.Length);
                });

                timer.Stop();
                Console.WriteLine($"fetched {matrixName} in: {timer.Elapsed.Seconds} Seconds");
                timer.Reset();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                ApiClient.client.Dispose();
            }

            return output;
        }

        private static string GetAsString(long[,] array)
        {
            int row = array.GetLength(0);
            int col = array.GetLength(1);

            var sb = new StringBuilder();
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    sb.Append(array[i, j]);
                }
            }
            return sb.ToString();
        }
    }

}
