using System;
using System.Linq;
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
		public RefreshTokenStore(IIdentityServerOperationalDataAccessModel dataModel) :
			base(dataModel, DbTokenType.RefreshToken)
		{
		}

		public override Task StoreAsync(string key, RefreshToken value)
		{
			using (var scope = TransactionScopeFactory.CreateReadCommitted())
			{
				var token = DataModel.Tokens.SingleOrDefault(x => x.Key == key && x.TokenType == this.TokenType);

				if (token == null)
				{
					token = DataModel.Tokens.Create();

					token.Id = Guid.NewGuid();
					token.Key = key;
					token.SubjectId = value.SubjectId;
					token.ClientId = Guid.Parse(value.ClientId);
					token.JsonCode = ConvertToJson(value); // TODO bug? https://github.com/IdentityServer/IdentityServer3.EntityFramework/issues/65
					token.TokenType = this.TokenType;
				}

				token.Expiry = DateTimeOffset.UtcNow.AddSeconds(value.LifeTime);

				scope.Complete();
			}

			return Task.FromResult(0);
		}
	}
}