using System;
using System.Threading.Tasks;

namespace MultiplyTwoMatrices
{
    public static class Logs {
        public static void LogMatrixRow(MatrixRowModel row) {
            Console.WriteLine($"Value: {row.Value}");
            foreach (var i in row.Value)
            {
                Console.WriteLine(i);
            }
        }

        public static void LogInitializeResponse(PostMatrixModel init) {
            Console.WriteLine($"Initialized: {init.Value}");
        }

        public static void LogPostResponse(PostMatrixModel post) {
            Console.WriteLine($"Posted: {post.Value}");
        }
    }
}
