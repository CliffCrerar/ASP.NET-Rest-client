using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
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
				ProcessRepositories().Wait(); // run program if user input != 'y'
			};
		}
		private static async Task ProcessRepositories()
		{
			try // include error handling
			{
				Console.WriteLine("|> >->->->->->->->> EXECUTING HTTP REQUEST <<-<-<-<-<-<-<-< <|");
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
