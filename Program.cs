using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;

class Program
{
    private static async Task Main(string[] args)
    {
        string cluster = "https://<cluster-name>.<region>.kusto.windows.net";
        string database = "<database-name>";
        string query = "<kusto-query>";
        string clientId = "<client-id>";
        string clientSecret = "<client-secret>";
        string tenantId = "<tenant-id>";

        string token = await GetAccessToken(clientId, clientSecret, tenantId);

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent($"{{\"db\":\"{database}\",\"csl\":\"{query}\"}}", System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{cluster}/v1/rest/query", content);
            string result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
        }
    }

    private static async Task<string> GetAccessToken(string clientId, string clientSecret, string tenantId)
    {
        IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(clientId)
            .WithClientSecret(clientSecret)
            .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
            .Build();

        string[] scopes = new string[] { "https://kusto.kusto.windows.net/.default" };
        AuthenticationResult result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
        return result.AccessToken;
    }
}

