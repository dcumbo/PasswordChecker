using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;

namespace PasswordChecker
{
	public class PasswordBreachService : IPasswordBreachService
	{
		/// <summary>
		///     Service URL.
		/// </summary>
		private const string URL = "https://api.pwnedpasswords.com/range/{password}";

		/// <inheritdoc />
		public string ProviderName => "HaveIBeenPwned";

		/// <inheritdoc />
		public async Task<int> GetBreachCountAsync(string password)
		{
			if (string.IsNullOrWhiteSpace(password))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(password));

			var sha1Password = HashPassword(password);

			var first = sha1Password.Substring(0, 5);
			var tail = sha1Password.Substring(5);

			var url = URL.Replace("{password}", first);

			var httpResult = await url.GetStringAsync();
			var result = HandleResult(httpResult.Split(Environment.NewLine), tail);

			return result;
		}

		/// <summary>
		///     Parses result and gets the count of breaches the password has been in, if any.
		/// </summary>
		/// <param name="httpResult">Result from API service.</param>
		/// <param name="tail">The rest of the hash (from index 5 onwards).</param>
		/// <returns>Count of breaches.</returns>
		/// <exception cref="ArgumentNullException">If httpResult is null.</exception>
		/// <exception cref="ArgumentException">If tail is null or empty.</exception>
		private static int HandleResult(IEnumerable<string> httpResult, string tail)
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

		/// <summary>
		///     Hashes password.
		/// </summary>
		/// <param name="password">Cleartext password.</param>
		/// <returns>Hashes password using SHA1</returns>
		/// <exception cref="ArgumentException">If password is empty.</exception>
		private static string HashPassword(string password)
		{
			if (string.IsNullOrWhiteSpace(password))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(password));

			using var sha1 = new SHA1Managed();
			var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));

			return string.Concat(hash.Select(b => b.ToString("X2")));
		}
	}
}