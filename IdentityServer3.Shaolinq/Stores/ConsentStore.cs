using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Shaolinq.DataModel.Interfaces;
using Shaolinq;

namespace IdentityServer3.Shaolinq.Stores
{
	public class ConsentStore : IConsentStore
	{
		private readonly IIdentityServerOperationalDataAccessModel dataModel;

		public ConsentStore(IIdentityServerOperationalDataAccessModel dataModel)
		{
			this.dataModel = dataModel;
		}

		public async Task<IEnumerable<Consent>> LoadAllAsync(string subject)
		{
			var found = dataModel.Consents.Where(x => x.Subject == subject);

			var results = await found.Select(x => new Consent
			{
				Subject = x.Subject,
				ClientId = x.ClientId,
				Scopes = ParseScopes(x.Scopes)
			}).ToListAsync();

		    return results;
		}

		private static IEnumerable<string> ParseScopes(string scopes)
		{
			if (string.IsNullOrWhiteSpace(scopes))
			{
				return Enumerable.Empty<string>();
			}

			return scopes.Split(',');
		}

		private static string StringifyScopes(IEnumerable<string> scopes)
		{
			if (scopes == null || !scopes.Any())
			{
				return null;
			}

			return scopes.Aggregate((s1, s2) => s1 + "," + s2);
		}

		public async Task RevokeAsync(string subject, string client)
		{
			using (var scope = DataAccessScope.CreateReadCommitted())
			{
				await dataModel.Consents.DeleteAsync(x => x.Subject == subject && x.ClientId == client);

				await scope.CompleteAsync();
			}
		}

		public async Task<Consent> LoadAsync(string subject, string client)
		{
			var found = await dataModel.Consents.SingleOrDefaultAsync(x => x.Subject == subject && x.ClientId == client);

			if (found == null)
			{
			    return null;
			}

			var result = new Consent
			{
				Subject = found.Subject,
				ClientId = found.ClientId,
				Scopes = ParseScopes(found.Scopes)
			};

            return result;
		}

		public async Task UpdateAsync(Consent consent)
		{
			using (var scope = DataAccessScope.CreateReadCommitted())
			{
				var item = dataModel.Consents.SingleOrDefault(x => x.Subject == consent.Subject && x.ClientId == consent.ClientId);

				if (consent.Scopes == null || !consent.Scopes.Any())
				{
				    item?.Delete();
				}
				else
				{
					if (item == null)
					{
						item = dataModel.Consents.Create();
						item.Id = Guid.NewGuid();
						item.Subject = consent.Subject;
						item.ClientId = consent.ClientId;
					}

					item.Scopes = StringifyScopes(consent.Scopes);
				}

				await scope.CompleteAsync();
			}
		}
	}
}