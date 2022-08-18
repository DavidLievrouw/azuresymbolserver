using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace azuresymbolserver
{
    public class Symbols
    {
        [FunctionName("Symbols")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Symbols/{pat}/{name}/{id}/{file}")] HttpRequest req,
            string pat, string name, string id, string file, ILogger logger)
        {
            logger.LogInformation("Acquiring symbols for {0} ({1}, {2})", name, id, file);
            var fileStream = await GetSymbols(pat, name, id, file, logger);
            return new FileStreamResult(fileStream, "application/octet-stream");
        }

        public static async Task<Stream> GetSymbols(string pat, string name, string id, string file, ILogger logger)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{""}:{pat}")));
            var url = $"https://artifacts.dev.azure.com/recomatics/_apis/Symbol/symsrv/{name}/{id}/{file}";
            logger.LogInformation("Downloading from {0}...", url);
            using var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var stream = new MemoryStream();
            await (await response.Content.ReadAsStreamAsync()).CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
