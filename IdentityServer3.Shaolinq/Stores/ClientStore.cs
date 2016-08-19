using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Shaolinq.DataModel.Interfaces;
using IdentityServer3.Shaolinq.Mapping;
using Shaolinq;

namespace IdentityServer3.Shaolinq.Stores
{
	public class ClientStore : IClientStore
	{
		private readonly IIdentityServerClientDataAccessModel dataModel;

		public ClientStore(IIdentityServerClientDataAccessModel dataModel)
		{
			this.dataModel = dataModel;
		}

		public async Task<Client> FindClientByIdAsync(string clientId)
		{
			// TODO include clientsecrets, redirecturis, postlogoutredirecturis, allowedscopes,
			// identityproviderrestrictions, claims, allowedcustomgranttypes, allowedcorsorigins
			var client = await dataModel.Clients.SingleOrDefaultAsync(x => x.Id == clientId);

			var model = client.ToModel();

			return model;
		}
	}
}