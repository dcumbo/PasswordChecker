using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CommandLine;

namespace PasswordChecker
{
	internal class Program
	{
		private static readonly IPasswordBreachService PasswordBreachService = new PasswordBreachService();

		private static async Task Main(string[] args)
		{
			await Parser.Default.ParseArguments<Options>(args)
				.MapResult(
					async options => await CheckPasswordsAsync(options), 
					async errors => HandleParseErrors(errors));
		}

		private static async Task CheckPasswordsAsync(Options options)
		{
			foreach (var item in options.Password)
			{
				var count = await PasswordBreachService.GetBreachCountAsync(item);

				Console.WriteLine(count > 0
					? $"{item} found in {count:N0} breaches."
					: $"{item} not found in any breach.");
			}
		}

		private static void HandleParseErrors(IEnumerable<Error> errors)
		{
			foreach (var error in errors)
			{
				Console.WriteLine(error);
			}
			
			Environment.Exit(1);
		}
	}
}