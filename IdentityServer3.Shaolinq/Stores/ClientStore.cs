using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Shaolinq.DataModel.Interfaces;
using IdentityServer3.Shaolinq.Mapping;

namespace IdentityServer3.Shaolinq.Stores
{
	public class ClientStore : IClientStore
	{
		private readonly IIdentityServerClientDataAccessModel dataModel;

		public ClientStore(IIdentityServerClientDataAccessModel dataModel)
		{
			this.dataModel = dataModel;
		}

		public Task<Client> FindClientByIdAsync(string clientId)
		{
			// TODO include clientsecrets, redirecturis, postlogoutredirecturis, allowedscopes,
			// identityproviderrestrictions, claims, allowedcustomgranttypes, allowedcorsorigins
			var client = dataModel.Clients.SingleOrDefault(x => x.Id == Guid.Parse(clientId));

			var model = client.ToModel();

			return Task.FromResult(model);
		}
	}
}