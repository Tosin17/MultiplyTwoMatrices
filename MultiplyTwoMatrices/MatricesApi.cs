using System.Threading.Tasks;
using System.Net.Http;
using System;

namespace MultiplyTwoMatrices
{
    public static class MatricesApi {
        public static async Task<MatrixRowModel> GetMatrixAsync(string path, int rowIndex) {
            MatrixRowModel row = new MatrixRowModel();

            HttpResponseMessage response = await ApiClient.client.GetAsync(path);
            if (response.IsSuccessStatusCode) {
                row = await response.Content.ReadAsAsync<MatrixRowModel>();
            }
            row.RowIndex = rowIndex;
            return row;
        }

        public static async Task<MatrixInitializeModel> InitMatrixAsync(string path) {
            MatrixInitializeModel init = new MatrixInitializeModel();

            HttpResponseMessage response = await ApiClient.client.GetAsync(path);
            if (response.IsSuccessStatusCode) {
                init = await response.Content.ReadAsAsync<MatrixInitializeModel>();
            }
            return init;
        }

        public static async Task<Uri> PostMatrixAsync(string path, string hash) {

            HttpResponseMessage response = await ApiClient.client.PostAsync(path, new StringContent(hash));
            response.EnsureSuccessStatusCode();


            return response.Headers.Location;
        }
    }
}
