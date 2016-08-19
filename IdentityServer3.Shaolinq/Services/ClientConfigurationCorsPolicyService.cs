using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer3.Core.Services;
using IdentityServer3.Shaolinq.DataModel.Interfaces;
using IdentityServer3.Shaolinq.Extensions;
using Shaolinq;

namespace IdentityServer3.Shaolinq.Services
{
	public class ClientConfigurationCorsPolicyService : ICorsPolicyService
	{
		private readonly IIdentityServerClientDataAccessModel dataModel;

		public ClientConfigurationCorsPolicyService(IIdentityServerClientDataAccessModel dataModel)
		{
			this.dataModel = dataModel;
		}

		public async Task<bool> IsOriginAllowedAsync(string origin)
		{
			var dbOrigins = await dataModel.ClientCorsOrigins.Select(x => x.Origin).ToListAsync();

			var origins = dbOrigins.Select(x => x.GetOrigin()).Where(x => x != null).Distinct();

			var result = origins.Contains(origin, StringComparer.OrdinalIgnoreCase);

			return result;
		}
	}
}