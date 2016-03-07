using System;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Shaolinq.DataModel;
using IdentityServer3.Shaolinq.DataModel.Interfaces;
using Shaolinq;

namespace IdentityServer3.Shaolinq.Stores
{
	public class RefreshTokenStore : BaseTokenStore<RefreshToken>, IRefreshTokenStore
	{
		public RefreshTokenStore(IIdentityServerOperationalDataAccessModel dataModel, IClientStore clientStore, IScopeStore scopeStore) :
			base(dataModel, DbTokenType.RefreshToken, clientStore, scopeStore)
		{
		}

	    public override async Task StoreAsync(string key, RefreshToken value)
	    {
	        using (var scope = DataAccessScope.CreateReadCommitted())
	        {
	            var token = await DataModel.Tokens.SingleOrDefaultAsync(x => x.Key == key && x.TokenType == this.TokenType);

	            if (token == null)
	            {
	                token = DataModel.Tokens.Create();

	                token.Id = Guid.NewGuid();
	                token.Key = key;
	                token.SubjectId = value.SubjectId;
	                token.ClientId = value.ClientId;
	                token.JsonCode = ConvertToJson(value);
	                token.TokenType = this.TokenType;
	            }

	            token.Expiry = value.CreationTime.AddSeconds(value.LifeTime);

	            await scope.CompleteAsync();
	        }
	    }
	}
}