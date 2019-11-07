using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace PasswordChecker
{
	public class PwnedService
	{
		private const string URL = "https://api.pwnedpasswords.com/range/{password}";

		public async Task<int> GetBreachCountAsync(string sha1Password)
		{
			if (string.IsNullOrWhiteSpace(sha1Password))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(sha1Password));
			
			var first = sha1Password.Substring(0, 5);
			var tail = sha1Password.Substring(5);

			var url = URL.Replace("{password}", first);
			
			var httpResult = await url.GetStringAsync();
			var result = handleResult(httpResult.Split(Environment.NewLine), tail);

			return result;
		}

		private int handleResult(IEnumerable<string> httpResult, string tail)
		{
			if (httpResult == null) 
				throw new ArgumentNullException(nameof(httpResult));
			if (string.IsNullOrWhiteSpace(tail))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(tail));

			return httpResult.Select(item => item.Split(':'))
				.Where(password => password.Length == 2 && password[0] == tail)
				.Select(password => int.Parse(password[1]))
				.FirstOrDefault();
		}
	}
}