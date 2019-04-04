using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MultiplyTwoMatrices
{
    public static class ApiClient {
        public static HttpClient client = new HttpClient();

        public static void initializeApiClient() {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://recruitment-test.investcloud.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
