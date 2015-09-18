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

		public Task<IEnumerable<Consent>> LoadAllAsync(string subject)
		{
			var found = dataModel.Consents.Where(x => x.Subject == subject);

			var results = found.Select(x => new Consent
			{
				Subject = x.Subject,
				ClientId = x.ClientId,
				Scopes = ParseScopes(x.Scopes)
			});

			return Task.FromResult(results.ToArray().AsEnumerable());
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

		public Task RevokeAsync(string subject, string client)
		{
			using (var scope = TransactionScopeFactory.CreateReadCommitted())
			{
				dataModel.Consents.DeleteWhere(x => x.Subject == subject && x.ClientId == client);

				scope.Complete();
			}

			return Task.FromResult(0);
		}

		public Task<Consent> LoadAsync(string subject, string client)
		{
			var found = dataModel.Consents.SingleOrDefault(x => x.Subject == subject && x.ClientId == client);

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

			return Task.FromResult(result);
		}

		public Task UpdateAsync(Consent consent)
		{
			using (var scope = TransactionScopeFactory.CreateReadCommitted())
			{
				var item = dataModel.Consents.SingleOrDefault(x => x.Subject == consent.Subject && x.ClientId == consent.ClientId);

				if (consent.Scopes == null || !consent.Scopes.Any())
				{
					if (item != null)
					{
						item.Delete();
					}
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

				scope.Complete();
			}

			return Task.FromResult(0);
		}
	}
}