using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Data.FileDatabase
{
	/// <summary>
	/// Detects the Installed Access Engine version.
	/// </summary>
	public static class MicrosoftAccessProvider
	{
		/// <summary>
		/// Returns true if the Access Database Engine provider was found.
		/// </summary>
		public static bool ProviderFound => MicrosoftAccessProvider.Current != null;

		/// <summary>
		/// Gets the current IOleDbProvider instance.
		/// </summary>
		public static IOleDbProvider Current { get; private set; }

		/// <summary>
		/// Returns true if the preferred provider is the current provider.
		/// </summary>
		public static bool IsPreferred { get; set; }

		/// <summary>
		/// Perform the check for the current provider.
		/// </summary>
		/// <returns></returns>
		public static async Task<IOleDbProvider> CheckForAccessProviderAsync(string preferredProvider = JetDatabase.Providers.AccessEngine12)
		{
			IOleDbProvider returnValue = null;

			// ***
			// *** Get a list of all providers.
			// ***
			IEnumerable<IOleDbProvider> providers = await JetDatabase.GetOleDbProvidersAsync();

			// ***
			// *** Filter Access drivers.
			// ***
			IEnumerable<IOleDbProvider> filtered = providers.Where(t => t.Description.Contains("Jet") | t.Description.Contains("Access Database"));

			// ***
			// *** Check for the preferred provider
			// ***
			IOleDbProvider preferred = filtered.Where(t => t.Name == preferredProvider).SingleOrDefault();

			if (preferred != null)
			{
				MicrosoftAccessProvider.Current = preferred;
				MicrosoftAccessProvider.IsPreferred = true;
				returnValue = preferred;
			}
			else
			{
				// ***
				// *** Get the first driver in the filtered list.
				// ***
				IOleDbProvider secondary = filtered.FirstOrDefault();

				if (secondary != null)
				{
					MicrosoftAccessProvider.Current = secondary;
					MicrosoftAccessProvider.IsPreferred = true;
					returnValue = secondary;
				}
				else
				{
					MicrosoftAccessProvider.Current = null;
					MicrosoftAccessProvider.IsPreferred = false;
					returnValue = null;
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Gets a list of providers installed on the current machine.
		/// </summary>
		/// <returns></returns>
		public static async Task<IEnumerable<IOleDbProvider>> Providers()
		{
			// ***
			// *** Get a list of all providers.
			// ***
			IEnumerable<IOleDbProvider> providers = await JetDatabase.GetOleDbProvidersAsync();

			// ***
			// *** Filter Access drivers.
			// ***
			return providers.Where(t => t.Description.Contains("Jet") | t.Description.Contains("Access Database"));
		}

		/// <summary>
		/// Sets the preferred provider by name. This provider is used by all
		/// newly created instances of AccessDatabase.
		/// </summary>
		/// <param name="providerName">The name of the provider to user.</param>
		public static async Task SetProviderByNameAsync(string providerName)
		{
			// ***
			// *** Check for the preferred provider
			// ***
			IOleDbProvider selected = (await MicrosoftAccessProvider.Providers()).Where(t => t.Name == providerName).SingleOrDefault();

			if (selected != null)
			{
				MicrosoftAccessProvider.Current = selected;
				MicrosoftAccessProvider.IsPreferred = true;
			}
			else
			{
				throw new SpecifiedProviderNotFoundException(providerName);
			}
		}
	}
}
