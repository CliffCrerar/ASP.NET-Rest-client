using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebAPIClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            var startMsg = "|----------------< STARING REST CLIENT DEMO >----------------|";
            Console.WriteLine(startMsg);
            ProcessRepositories().Wait();
        }
        private static async Task ProcessRepositories()
        {
            Console.WriteLine("| -> Clear headers");
            client.DefaultRequestHeaders.Accept.Clear();

            Console.WriteLine("| -> Set Response Headers");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));

            Console.WriteLine("| -> Set Request Headers");
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            Console.WriteLine("| -> Instantiating Task");
            var stringTask = client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");

            Console.WriteLine("| -> Executing . . .");
            var msg = await stringTask;

            Console.Write(msg);  
        }
    }
}
