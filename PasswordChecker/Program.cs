using System;
using System.Threading.Tasks;

namespace PasswordChecker
{
	class Program
	{
		private static readonly IPasswordBreachService PasswordBreachService = new PasswordBreachService();
		
		static async Task Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine("Pass password value as argument.");
			}
			
			foreach (var item in args)
			{
				var count = await PasswordBreachService.GetBreachCountAsync(item);
				
				Console.WriteLine(count > 0
					? $"{item} found in {count:N0} breaches."
					: $"{item} not found in any breach.");
			}
		}
	}
}