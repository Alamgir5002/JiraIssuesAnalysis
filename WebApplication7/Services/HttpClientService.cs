using System.Net.Http.Headers;
using System.Text;
using WebApplication7.Models;

namespace WebApplication7.Services
{
    public class HttpClientService
    {
        private HttpClient httpClient;
        public HttpClientService()
        {
            httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> SendPostRequest(string endpoint, string jsonData, SourceCredentials sourceCredentials)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", getBasicAuthHeaders(sourceCredentials));
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

        public async Task<HttpResponseMessage> SendGetRequestWithBasicAuthHeaders(string endpoint, SourceCredentials sourceCredentials)
        {
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", getBasicAuthHeaders(sourceCredentials));
            return await httpClient.GetAsync(endpoint);
        }

        private string getBasicAuthHeaders(SourceCredentials sourceCredentials)
        {
            string basicAuthString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{sourceCredentials.SourceUserEmail}:{sourceCredentials.SourceAuthToken}"));
            return basicAuthString;
        }

    }
}
