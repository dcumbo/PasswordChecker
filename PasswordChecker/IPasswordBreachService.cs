using System.Threading.Tasks;

namespace PasswordChecker
{
	public interface IPasswordBreachService
	{
		/// <summary>
		///     Name of the provider being used.
		/// </summary>
		string ProviderName { get; }

		/// <summary>
		///     Checks against the provider to see if the password was part of a breach.
		/// </summary>
		/// <param name="password">Cleartext password</param>
		/// <returns>Count of breaches the password was found in.</returns>
		Task<int> GetBreachCountAsync(string password);
	}
}