using System;
using IdentityServer3.Core.Models;
using Platform.Validation;
using Shaolinq;

namespace IdentityServer3.Shaolinq.DataModel
{
	[DataAccessObject(Name = "ClientPostLogoutRedirectUri")]
	public abstract class DbClientPostLogoutRedirectUri : DataAccessObject<Guid>
	{
		[PersistedMember]
		[ValueRequired]
		[SizeConstraint(MaximumLength = 2000)]
		public abstract string Uri { get; set; }

		[BackReference]
		[Index]
		public abstract DbClient Client { get; set; }
	}
}