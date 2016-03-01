using System;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Shaolinq.DataModel;
using IdentityServer3.Shaolinq.DataModel.Interfaces;
using Shaolinq;

namespace IdentityServer3.Shaolinq.Stores
{
	public class TokenHandleStore : BaseTokenStore<Token>, ITokenHandleStore
	{
		public TokenHandleStore(IIdentityServerOperationalDataAccessModel dataModel, IClientStore clientStore, IScopeStore scopeStore) :
			base(dataModel, DbTokenType.TokenHandle, clientStore, scopeStore)
		{
		}

		public override async Task StoreAsync(string key, Token value)
		{
			using (var scope = DataAccessScope.CreateReadCommitted())
			{
				var token = this.DataModel.Tokens.Create();

				token.Id = Guid.NewGuid();
				token.Key = key;
				token.SubjectId = value.SubjectId;
				token.ClientId = value.ClientId;
				token.JsonCode = ConvertToJson(value);
				token.Expiry = DateTimeOffset.UtcNow.AddSeconds(value.Lifetime);
				token.TokenType = this.TokenType;

				await scope.CompleteAsync();
			}
		}
	}
}