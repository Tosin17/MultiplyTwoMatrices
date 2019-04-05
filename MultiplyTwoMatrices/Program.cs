using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace MultiplyTwoMatrices
{
    class Program {
        static int[,] matrixA = new int[Matrix.RowSize, Matrix.ColSize];
        static int[,] matrixB = new int[Matrix.RowSize, Matrix.ColSize];

        static void Main(string[] args){
            Console.WriteLine("Starting.......................");
            var timer = new Stopwatch();
            timer.Start();

            var result = RunAsync().Result;
            timer.Stop();
            timer.Reset();

            string content = Matrix.GetAsString(result);

            string hashed = Md5Encryption.GetMd5Hash(content);
            Console.WriteLine(hashed);
        }

        public static async Task<long[,]> RunAsync() {
            matrixA = await GetMatrixData("A");
            matrixB = await GetMatrixData("B");
            return Matrix.MultiplyMatrix(matrixA, matrixB);
        }

        public static async Task<int[,]> GetMatrixData(string matrixName) {
            int[,] output = new int[Matrix.RowSize, Matrix.ColSize];
            ApiClient.initializeApiClient();
            try {
                MatrixInitializeModel init = await MatricesApi.InitMatrixAsync("api/numbers/init/1000");
                var tasks = new List<Task<MatrixRowModel>>();

                var timer = new Stopwatch();
                timer.Start();

                var batchSize = 100;
                int numberOfBatches = (int)Math.Ceiling((double)Matrix.RowSize / batchSize);

                for (int i = 0; i < numberOfBatches; i++) {
                    int start = i * batchSize;
                    var value = Enumerable.Range(start, batchSize).Select(k => MatricesApi.GetMatrixAsync($"api/numbers/{matrixName}/row/{k}", k));                    
                    tasks.AddRange(value);
                }

                var result = (await Task.WhenAll(tasks)).Select(m => m);

                Parallel.For(0, result.Count(), y => {
                    int[] values = result.Where(x => x.RowIndex == y).Select(x => x.Value).First();
                    Buffer.BlockCopy(values, 0, output, Matrix.ColSize * y, values.Length);
                });

                timer.Stop();
                Console.WriteLine($"fetched {matrixName} in: {timer.Elapsed.Seconds} Seconds");
                timer.Reset();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            finally {
                ApiClient.client.Dispose();
            }

            return output;
        }
    }

}
