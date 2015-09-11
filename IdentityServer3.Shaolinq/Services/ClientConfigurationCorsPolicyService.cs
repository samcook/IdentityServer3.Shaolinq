using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer3.Core.Services;
using IdentityServer3.Shaolinq.DataModel.Interfaces;
using IdentityServer3.Shaolinq.Extensions;

namespace IdentityServer3.Shaolinq.Services
{
	public class ClientConfigurationCorsPolicyService : ICorsPolicyService
	{
		private readonly IIdentityServerClientDataAccessModel dataModel;

		public ClientConfigurationCorsPolicyService(IIdentityServerClientDataAccessModel dataModel)
		{
			this.dataModel = dataModel;
		}

		public Task<bool> IsOriginAllowedAsync(string origin)
		{
			var foo = dataModel.ClientCorsOrigins.Select(x => x.Origin);

			var origins = foo.ToList().Select(x => x.GetOrigin()).Where(x => x != null).Distinct();

			var result = origins.Contains(origin, StringComparer.OrdinalIgnoreCase);

			return Task.FromResult(result);
		}
	}
}