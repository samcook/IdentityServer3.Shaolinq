using System.Linq;
using System.Security.Claims;
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
			Mapper.CreateMap<DbClient, Client>(MemberList.Destination)
				.ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.UpdateAccessTokenClaimsOnRefresh, opt => opt.MapFrom(src => src.UpdateAccessTokenOnRefresh))
				.ForMember(dest => dest.AllowAccessToAllCustomGrantTypes, opt => opt.MapFrom(src => src.AllowAccessToAllGrantTypes))
				.ForMember(x => x.AllowedCustomGrantTypes, opt => opt.MapFrom(src => src.AllowedCustomGrantTypes.Select(x => x.GrantType)))
				.ForMember(x => x.RedirectUris, opt => opt.MapFrom(src => src.RedirectUris.Select(x => x.Uri)))
				.ForMember(x => x.PostLogoutRedirectUris, opt => opt.MapFrom(src => src.PostLogoutRedirectUris.Select(x => x.Uri)))
				.ForMember(x => x.IdentityProviderRestrictions, opt => opt.MapFrom(src => src.IdentityProviderRestrictions.Select(x => x.Provider)))
				.ForMember(x => x.AllowedScopes, opt => opt.MapFrom(src => src.AllowedScopes.Select(x => x.Scope)))
				.ForMember(x => x.AllowedCorsOrigins, opt => opt.MapFrom(src => src.AllowedCorsOrigins.Select(x => x.Origin)))
				.ForMember(dest => dest.Claims, opt => opt.MapFrom(src => src.Claims.Select(x => new Claim(x.Type, x.Value))));

			Mapper.CreateMap<DbScope, Scope>(MemberList.Destination)
				.ForMember(dest => dest.Claims, opt => opt.MapFrom(src => src.ScopeClaims));

			Mapper.CreateMap<DbScopeClaim, ScopeClaim>(MemberList.Destination);

			//Mapper.AssertConfigurationIsValid();
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