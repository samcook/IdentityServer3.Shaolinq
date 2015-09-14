using System;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Shaolinq.DataModel;
using IdentityServer3.Shaolinq.DataModel.Interfaces;
using Shaolinq;

namespace IdentityServer3.Shaolinq.Stores
{
	public class AuthorizationCodeStore : BaseTokenStore<AuthorizationCode>, IAuthorizationCodeStore
	{
		public AuthorizationCodeStore(IIdentityServerOperationalDataAccessModel dataModel, IClientStore clientStore, IScopeStore scopeStore) :
			base(dataModel, DbTokenType.AuthorizationCode, clientStore, scopeStore)
		{
		}

		public override Task StoreAsync(string key, AuthorizationCode code)
		{
			using (var scope = TransactionScopeFactory.CreateReadCommitted())
			{
				var authCode = this.DataModel.Tokens.Create();

				authCode.Id = Guid.NewGuid();
				authCode.Key = key;
				authCode.SubjectId = code.SubjectId;
				authCode.ClientId = code.ClientId;
				authCode.JsonCode = ConvertToJson(code);
				authCode.Expiry = DateTimeOffset.UtcNow.AddSeconds(code.Client.AuthorizationCodeLifetime);
				authCode.TokenType = this.TokenType;

				scope.Complete();
			}

			return Task.FromResult(0);
		}
	}
}