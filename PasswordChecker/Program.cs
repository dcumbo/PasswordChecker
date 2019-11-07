using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordChecker
{
	class Program
	{
		private static readonly PwnedService service = new PwnedService();
		
		static async Task Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine("Pass password value as argument.");
			}
			
			foreach (var item in args)
			{
				var password = hashPassword(item);
				var count = await service.GetBreachCountAsync(password);
				
				Console.WriteLine(count > 0
					? $"{item} found in {count:N0} breaches."
					: $"{item} not found in any breach.");
			}
		}
		
		private static string hashPassword(string password)
		{
			if (string.IsNullOrWhiteSpace(password))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(password));
			
			using var sha1 = new SHA1Managed();
			var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
			return string.Concat(hash.Select(b => b.ToString("X2")));
		}
	}
}