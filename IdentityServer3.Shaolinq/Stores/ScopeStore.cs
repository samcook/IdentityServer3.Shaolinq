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
			var query =
				from scope in dataModel.Scopes
				join scopeClaim in dataModel.ScopeClaims.DefaultIfEmpty() on scope equals scopeClaim.Scope
				select new
				{
					Scope = scope,
					ScopeClaim = scopeClaim
				};

			if (scopeNames != null && scopeNames.Any())
			{
				query = query.Where(x => scopeNames.Contains(x.Scope.Name));
			}

			var scopes = query.ToLookup(x => x.Scope, x => x.ScopeClaim);

			var model = scopes.Select(x => x.ToModel());

			return Task.FromResult(model);
		}

		public Task<IEnumerable<Scope>> GetScopesAsync(bool publicOnly = true)
		{
			var query =
				from scope in dataModel.Scopes
				join scopeClaim in dataModel.ScopeClaims.DefaultIfEmpty() on scope equals scopeClaim.Scope
				select new
				{
					Scope = scope,
					ScopeClaim = scopeClaim
				};

			if (publicOnly)
			{
				query = query.Where(x => x.Scope.ShowInDiscoveryDocument);
			}

			var scopes = query.ToLookup(x => x.Scope, x => x.ScopeClaim);
			
			var model = scopes.Select(x => x.ToModel());

			return Task.FromResult(model);
		}
	}
}