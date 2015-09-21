using System;
using IdentityServer3.Core;
using Platform.Validation;
using Shaolinq;

namespace IdentityServer3.Shaolinq.DataModel
{
	[DataAccessObject(Name = "ClientSecret")]
	public abstract class DbClientSecret : DataAccessObject<Guid>
	{
		public DbClientSecret SetDefaults()
		{
			this.Type = Constants.SecretTypes.SharedSecret;

			return this;
		}

		[PersistedMember]
		[ValueRequired]
		[SizeConstraint(MaximumLength = 250)]
		public abstract string Value { get; set; }

		[PersistedMember]
		[SizeConstraint(MaximumLength = 250)]
		public abstract string Type { get; set; }

		[PersistedMember]
		[SizeConstraint(MaximumLength = 2000)]
		public abstract string Description { get; set; }

		public DateTimeOffset? Expiration
		{
			get
			{
				return
					ExpirationDateTime == null
						? (DateTimeOffset?) null
						: new DateTimeOffset(ExpirationDateTime.Value);
			}
			set
			{
				ExpirationDateTime = value == null ? (DateTime?) null : value.Value.UtcDateTime;
			}
		}

		[PersistedMember]
		public abstract DateTime? ExpirationDateTime { get; set; }

		[BackReference]
		public abstract DbClient Client { get; set; }
	}
}