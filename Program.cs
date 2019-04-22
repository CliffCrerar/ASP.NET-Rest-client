using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.IO;
// Custom modules
using ErrorHandlers;

namespace WebAPIClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient(); // instantiate http client
        private static readonly ErrorHandler errorHandler = new ErrorHandler(); // instantiate ErrorHandler

        static void Main(string[] args) // Main program or program entry
        {
            var startMsg = "|----------------< STARING REST CLIENT DEMO >----------------|";
            Console.WriteLine(startMsg);
            if (TestError())
            { // if user input us was to test error or not
                EndProgram(); // end program if user input = 'y'
            }
            else
            {
                var repositories = ProcessRepositories().Result; // run program if user input != 'y'
                Console.WriteLine("| -> Looping throug HTTP Response");
                var counter = 0;
                foreach (var repo in repositories)
                {
                    //Console.Write(repo);
                    Console.WriteLine("|> === === === === === === === === <|");
                    counter++;
                    Console.WriteLine("REPOSITORY NUMBER" + counter.ToString());
                    Console.WriteLine(repo.Name);
                    Console.WriteLine(repo.Description);
                    Console.WriteLine(repo.GitHubHomeUrl);
                    Console.WriteLine(repo.Homepage);
                    Console.WriteLine(repo.Watchers);
                    Console.WriteLine(repo.LastPush);
                    Console.WriteLine();
                    //Console.WriteLine("|> === === === === === === === === <|");


                }
                WriteNumberOfRepos(counter);
                EndProgram(); // End program
            };
        }
        private static async Task<List<Repository>> ProcessRepositories()
        {
            List<Repository> repositories = null;
            try // include error handling
            {
                var serializer = new DataContractJsonSerializer(typeof(List<Repository>));
                //var dataToFile = new DataContractJsonSerializer(typeof(string));
                Console.WriteLine("|> >->->->->->->->> EXECUTING HTTP REQUEST <<-<-<-<-<-<-<-< <|");
                Console.WriteLine("| -> Clear headers");
                client.DefaultRequestHeaders.Accept.Clear();

                Console.WriteLine("| -> Set Response Headers");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));

                Console.WriteLine("| -> Set Request Headers");
                client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

                Console.WriteLine("| -> Executing . . .");
                /* Call JSON string from source */
                var callString = false;

                if (callString)
                {
                    Console.WriteLine("| -> Running string task");
                    Console.WriteLine("| -> Instantiating String Task");
                    var msg = await client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");
                    Console.Write(msg);
                }
                Console.WriteLine("| -> NOT Running string task");

                /* Call MAP Json object with a stream */

                Console.WriteLine("| -> Instantiating Stream Task");
                var streamTask = client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
                // create stream and interpret object using List of type <Repository> to interpret
                // var repositories = serializer.ReadObject(await streamTask) as List<Repository>;
                repositories = serializer.ReadObject(await streamTask) as List<Repository>;
            }
            catch (Exception err)
            {
                errorHandler.Handle(err);
            }
            return repositories;
        }

        /* --------------------------------------------------------------------------------- */
        /* -------------------------MESSING AROUND FUNCTIONS-------------------------------- */
        /* --------------------------------------------------------------------------------- */

        private readonly Exception TestErr = new Exception();
        private static bool TestError()
        {
            Console.WriteLine("Do you want to test the error? [ y ]");
            var input = Console.Read();
            if (input == 'y')
            {
                try
                {
                    throw new InvalidOperationException("This is a test error!");
                }
                catch (Exception err)
                {
                    errorHandler.Handle(err);
                }
                return true;
            }
            return false;
        }

        public static void WriteNumberOfRepos(int counter)
        {
            Console.WriteLine("|> === === === === === === === === <|");
            Console.WriteLine("|> Number of repos: " + counter.ToString());
            Console.WriteLine("|> === === === === === === === === <|");
        }

        private static void EndProgram()
        {
            Console.WriteLine("END OF PROGRAM");
        }
    }
}