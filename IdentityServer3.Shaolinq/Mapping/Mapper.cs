using System.Linq;
using System.Security.Claims;
using AutoMapper;
using IdentityServer3.Core.Models;
using IdentityServer3.Shaolinq.DataModel;

namespace IdentityServer3.Shaolinq.Mapping
{
	internal static class DataModelMap
	{
		static DataModelMap()
		{
			Mapper.CreateMap<DbClientSecret, Secret>(MemberList.Destination);
			Mapper.CreateMap<DbClient, Client>(MemberList.Destination)
				.ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.UpdateAccessTokenClaimsOnRefresh, opt => opt.MapFrom(src => src.UpdateAccessTokenOnRefresh))
				.ForMember(dest => dest.AllowAccessToAllCustomGrantTypes, opt => opt.MapFrom(src => src.AllowAccessToAllGrantTypes))
				.ForMember(dest => dest.AllowedCustomGrantTypes, opt => opt.MapFrom(src => src.AllowedCustomGrantTypes.Select(x => x.GrantType)))
				.ForMember(dest => dest.RedirectUris, opt => opt.MapFrom(src => src.RedirectUris.Select(x => x.Uri)))
				.ForMember(dest => dest.PostLogoutRedirectUris, opt => opt.MapFrom(src => src.PostLogoutRedirectUris.Select(x => x.Uri)))
				.ForMember(dest => dest.IdentityProviderRestrictions, opt => opt.MapFrom(src => src.IdentityProviderRestrictions.Select(x => x.Provider)))
				.ForMember(dest => dest.AllowedScopes, opt => opt.MapFrom(src => src.AllowedScopes.Select(x => x.Scope)))
				.ForMember(dest => dest.AllowedCorsOrigins, opt => opt.MapFrom(src => src.AllowedCorsOrigins.Select(x => x.Origin)))
				.ForMember(dest => dest.Claims, opt => opt.MapFrom(src => src.Claims.Select(x => new Claim(x.Type, x.Value))));

			Mapper.CreateMap<DbScope, Scope>(MemberList.Destination)
				.ForMember(dest => dest.Claims, opt => opt.Ignore());

			Mapper.CreateMap<DbScopeClaim, ScopeClaim>(MemberList.Destination);

			//Mapper.AssertConfigurationIsValid();
		}

		internal static Client ToModel(this DbClient s)
		{
			return s == null ? null : Mapper.Map<Client>(s);
		}

		internal static Scope ToModel(this IGrouping<DbScope, DbScopeClaim> scopeAndClaims)
		{
			var scope = Mapper.Map<Scope>(scopeAndClaims.Key);

			if (scopeAndClaims.Any(x => x != null))
			{
				scope.Claims = scopeAndClaims.Where(x => x != null).Select(Mapper.Map<ScopeClaim>).ToList();
			}

			return scope;
		}
	}
}