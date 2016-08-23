using System;
using System.Security.Claims;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using Newtonsoft.Json.Serialization;

namespace IdentityServer3.Shaolinq.Serialization
{
	public class ConverterContractResolver : DefaultContractResolver
	{
		private readonly IClientStore clientStore;
		private readonly IScopeStore scopeStore;

		public ConverterContractResolver(IClientStore clientStore, IScopeStore scopeStore)
		{
			this.clientStore = clientStore;
			this.scopeStore = scopeStore;
		}

		protected override JsonContract CreateContract(Type objectType)
		{
			var contract = base.CreateContract(objectType);

			if (objectType == typeof(Claim))
			{
				contract.Converter = new ClaimConverter();
			}
			else if (objectType == typeof(ClaimsPrincipal))
			{
				contract.Converter = new ClaimsPrincipalConverter();
			}
			else if (objectType == typeof(Client))
			{
				contract.Converter = new ClientConverter(clientStore);
			}
			else if (objectType == typeof(Scope))
			{
				contract.Converter = new ScopeConverter(scopeStore);
			}

			return contract;
		}
	}
}