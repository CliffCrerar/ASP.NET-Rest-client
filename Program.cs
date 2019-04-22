using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ErrorHandlers;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using WebAPIClient;

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
				ProcessRepositories().Wait(); // run program if user input != 'y'
			};
		}
		private static async Task ProcessRepositories()
		{
			try // include error handling
			{
				var serializer = new DataContractJsonSerializer(typeof(List<repo>));
				Console.WriteLine("|> >->->->->->->->> EXECUTING HTTP REQUEST <<-<-<-<-<-<-<-< <|");
				Console.WriteLine("| -> Clear headers");
				client.DefaultRequestHeaders.Accept.Clear();

				Console.WriteLine("| -> Set Response Headers");
				client.DefaultRequestHeaders.Accept.Add(
					new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));

				Console.WriteLine("| -> Set Request Headers");
				client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

				Console.WriteLine("| -> Instantiating Task");
				// var stringTask = client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");
				var streamTask = client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
				Console.WriteLine("| -> Executing . . .");
				var repositories = serializer.ReadObject(await streamTask) as List<repo>;


				//var msg = await stringTask;
				//Console.Write(msg);
				Console.WriteLine("| -> Looping throug HTTP Response");
				foreach (var repo in repositories)
				{
					Console.WriteLine("Repo: " + repo.name);
				}

				EndProgram(); // End program
			}
			catch (Exception err)
			{
				errorHandler.Handle(err);
				EndProgram(); // End Program
			}
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

		private static void EndProgram()
		{
			Console.WriteLine("END OF PROGRAM");
		}
	}
}