using System;
using System.Collections.Generic;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.Shaolinq.DataModel.Interfaces;
using IdentityServer3.Shaolinq.Services;
using IdentityServer3.Shaolinq.Stores;
using Shaolinq;

namespace IdentityServer3.Shaolinq.Extensions
{
	public static class IdentityServerServiceFactoryExtensions
	{
		private static readonly object LockObj = new object();
		private static readonly Dictionary<Type, DataAccessModel> DataAccessModels = new Dictionary<Type, DataAccessModel>();

		public static IdentityServerServiceFactory RegisterOperationalServices<T>(this IdentityServerServiceFactory factory, ShaolinqServiceOptions options)
			where T : DataAccessModel, IIdentityServerOperationalDataAccessModel
		{
			factory.RegisterDataModelSingleton<IIdentityServerOperationalDataAccessModel, T>();

			factory.AuthorizationCodeStore = new Registration<IAuthorizationCodeStore, AuthorizationCodeStore> { Mode = RegistrationMode.Singleton };
			factory.TokenHandleStore = new Registration<ITokenHandleStore, TokenHandleStore> { Mode = RegistrationMode.Singleton };
			factory.ConsentStore = new Registration<IConsentStore, ConsentStore> { Mode = RegistrationMode.Singleton };
			factory.RefreshTokenStore = new Registration<IRefreshTokenStore, RefreshTokenStore> { Mode = RegistrationMode.Singleton };

			return factory;
		}

		public static IdentityServerServiceFactory RegisterClientStore<T>(this IdentityServerServiceFactory factory, ShaolinqServiceOptions options)
			where T : DataAccessModel, IIdentityServerClientDataAccessModel
		{
			factory.RegisterDataModelSingleton<IIdentityServerClientDataAccessModel, T>();

			factory.ClientStore = new Registration<IClientStore, ClientStore> { Mode = RegistrationMode.Singleton };
			factory.CorsPolicyService = new Registration<ICorsPolicyService, ClientConfigurationCorsPolicyService> { Mode = RegistrationMode.Singleton };

			return factory;
		}

		public static IdentityServerServiceFactory RegisterScopeStore<T>(this IdentityServerServiceFactory factory, ShaolinqServiceOptions options)
			where T : DataAccessModel, IIdentityServerScopeDataAccessModel
		{
			factory.RegisterDataModelSingleton<IIdentityServerScopeDataAccessModel, T>();

			factory.ScopeStore = new Registration<IScopeStore, ScopeStore> { Mode = RegistrationMode.Singleton };

			return factory;
		}

		private static void RegisterDataModelSingleton<TInterface, TImpl>(this IdentityServerServiceFactory factory)
			where TImpl : DataAccessModel, TInterface
			where TInterface : class
		{
			lock (LockObj)
			{
				DataAccessModel dataModel;

				if (!DataAccessModels.TryGetValue(typeof(TImpl), out dataModel))
				{
					dataModel = DataAccessModel.BuildDataAccessModel<TImpl>();
					DataAccessModels.Add(typeof(TImpl), dataModel);
				}

				factory.Register(new Registration<TInterface>((TImpl)dataModel));
			}
		}
	}
}