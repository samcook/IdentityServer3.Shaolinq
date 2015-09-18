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
					ExpirationDateTime == null || ExpirationOffset == null
						? (DateTimeOffset?) null
						: new DateTimeOffset(ExpirationDateTime.Value, ExpirationOffset.Value);
			}
			set
			{
				ExpirationDateTime = value == null ? (DateTime?) null : value.Value.DateTime;
				ExpirationOffset = value == null ? (TimeSpan?) null : value.Value.Offset;
			}
		}

		[PersistedMember]
		public abstract DateTime? ExpirationDateTime { get; set; }

		[PersistedMember]
		public abstract TimeSpan? ExpirationOffset { get; set; }

		[BackReference]
		public abstract DbClient Client { get; set; }
	}
}