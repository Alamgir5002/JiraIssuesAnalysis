using System.Net.Http.Headers;
using System.Text;

namespace WebApplication7.Services
{
    public class HttpClientService
    {
        private HttpClient httpClient;
        public HttpClientService()
        {
            httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> SendPostRequest(string endpoint, string jsonData, string basicAuthString)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthString);
            // Create StringContent with JSON data
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Send POST request
            HttpResponseMessage response = await httpClient.PostAsync(endpoint, content);
            return response;
        }

        public async Task<HttpResponseMessage> SendGetRequest(string endpoint)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            request.Headers.Add("Accept", "application/json");
            return await httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> SendGetRequestWithBasicAuthHeaders(string endpoint, string basicAuthString)
        {
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthString);
            return await httpClient.GetAsync(endpoint);
        }

    }
}
