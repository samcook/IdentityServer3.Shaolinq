using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Shaolinq.DataModel.Interfaces;
using IdentityServer3.Shaolinq.Mapping;

namespace IdentityServer3.Shaolinq.Stores
{
	public class ScopeStore : IScopeStore
	{
		private readonly IIdentityServerScopeDataAccessModel dataModel;

		public ScopeStore(IIdentityServerScopeDataAccessModel dataModel)
		{
			this.dataModel = dataModel;
		}

		public Task<IEnumerable<Scope>> FindScopesAsync(IEnumerable<string> scopeNames)
		{
			// TODO include scopeclaims
			var scopes = dataModel.Scopes.Select(x => x);

			if (scopeNames != null && scopeNames.Any())
			{
				scopes = scopes.Where(x => scopeNames.Contains(x.Name));
			}

			var model = scopes.ToList().Select(x => x.ToModel());

			return Task.FromResult(model);
		}

		public Task<IEnumerable<Scope>> GetScopesAsync(bool publicOnly = true)
		{
			// TODO include scopeclaims
			var scopes = dataModel.Scopes.Select(x => x);

			if (publicOnly)
			{
				scopes = scopes.Where(x => x.ShowInDiscoveryDocument);
			}

			var model = scopes.ToList().Select(x => x.ToModel());

			return Task.FromResult(model);
		}
	}
}