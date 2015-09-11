using AutoMapper;
using IdentityServer3.Core.Models;
using IdentityServer3.Shaolinq.DataModel;

namespace IdentityServer3.Shaolinq.Mapping
{
	public static class DataModelMap
	{
		static DataModelMap()
		{
			Mapper.CreateMap<DbClientSecret, Secret>(MemberList.Destination);
			Mapper.CreateMap<DbClient, Client>(MemberList.Destination);

			Mapper.CreateMap<DbScope, Scope>(MemberList.Destination);
			Mapper.CreateMap<DbScopeClaim, ScopeClaim>(MemberList.Destination);
		}

		public static Client ToModel(this DbClient s)
		{
			return s == null ? null : Mapper.Map<Client>(s);
		}

		public static Scope ToModel(this DbScope s)
		{
			return s == null ? null : Mapper.Map<Scope>(s);
		}
	}
}