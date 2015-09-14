using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Shaolinq.DataModel;
using IdentityServer3.Shaolinq.DataModel.Interfaces;
using IdentityServer3.Shaolinq.Serialization;
using Newtonsoft.Json;
using Shaolinq;

namespace IdentityServer3.Shaolinq.Stores
{
	public abstract class BaseTokenStore<TToken>
		where TToken : class
	{
		protected readonly IIdentityServerOperationalDataAccessModel DataModel;
		protected readonly DbTokenType TokenType;
		private readonly IClientStore clientStore;
		private readonly IScopeStore scopeStore;

		protected BaseTokenStore(IIdentityServerOperationalDataAccessModel dataModel, DbTokenType tokenType, IClientStore clientStore, IScopeStore scopeStore)
		{
			this.DataModel = dataModel;
			this.TokenType = tokenType;
			this.clientStore = clientStore;
			this.scopeStore = scopeStore;
		}

		public abstract Task StoreAsync(string key, TToken value);

		public Task<TToken> GetAsync(string key)
		{
			var token = DataModel.Tokens.SingleOrDefault(x => x.Key == key && x.TokenType == TokenType);

			//if (token == null || token.Expiry < DateTimeOffset.UtcNow)
			if (token == null)
			{
				return null;
			}

			return Task.FromResult(ConvertFromJson(token.JsonCode));
		}

		public Task RemoveAsync(string key)
		{
			using (var scope = TransactionScopeFactory.CreateReadCommitted())
			{
				DataModel.Tokens.DeleteWhere(x => x.Key == key && x.TokenType == TokenType);

				scope.Complete();
			}

			return Task.FromResult(0);
		}

		public Task<IEnumerable<ITokenMetadata>> GetAllAsync(string subject)
		{
			var tokens = DataModel.Tokens.Where(x => x.SubjectId == subject && x.TokenType == TokenType);

			var results = tokens.ToList().Select(x => ConvertFromJson(x.JsonCode));

			return Task.FromResult(results.Cast<ITokenMetadata>());
		}

		public Task RevokeAsync(string subject, string client)
		{
			using (var scope = TransactionScopeFactory.CreateReadCommitted())
			{
				DataModel.Tokens.DeleteWhere(x => x.SubjectId == subject && x.ClientId == client && x.TokenType == TokenType);

				scope.Complete();
			}

			return Task.FromResult(0);
		}

		protected string ConvertToJson(TToken value)
		{
			return JsonConvert.SerializeObject(value, GetJsonSerializerSettings());
		}

		protected TToken ConvertFromJson(string json)
		{
			return JsonConvert.DeserializeObject<TToken>(json, GetJsonSerializerSettings());
		}

		private JsonSerializerSettings GetJsonSerializerSettings()
		{
			var settings = new JsonSerializerSettings();
			settings.Converters.Add(new ClaimConverter()); // TODO
			settings.Converters.Add(new ClaimsPrincipalConverter()); // TODO
			settings.Converters.Add(new ClientConverter(clientStore)); // TODO
			settings.Converters.Add(new ScopeConverter(scopeStore)); // TODO
			return settings;
		}
	}
}