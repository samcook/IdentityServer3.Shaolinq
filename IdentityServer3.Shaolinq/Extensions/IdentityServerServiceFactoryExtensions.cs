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
		private static readonly Dictionary<string, DataAccessModel> DataAccessModels = new Dictionary<string, DataAccessModel>();

		public static IdentityServerServiceFactory RegisterOperationalServices<TDataModel>(this IdentityServerServiceFactory factory, ShaolinqServiceOptions options = null)
			where TDataModel : DataAccessModel, IIdentityServerOperationalDataAccessModel
		{
			factory.RegisterDataModelSingleton<IIdentityServerOperationalDataAccessModel, TDataModel>(options);

			factory.AuthorizationCodeStore = new Registration<IAuthorizationCodeStore, AuthorizationCodeStore> { Mode = RegistrationMode.Singleton };
			factory.TokenHandleStore = new Registration<ITokenHandleStore, TokenHandleStore> { Mode = RegistrationMode.Singleton };
			factory.ConsentStore = new Registration<IConsentStore, ConsentStore> { Mode = RegistrationMode.Singleton };
			factory.RefreshTokenStore = new Registration<IRefreshTokenStore, RefreshTokenStore> { Mode = RegistrationMode.Singleton };

			return factory;
		}

		public static IdentityServerServiceFactory RegisterClientStore<TDataModel>(this IdentityServerServiceFactory factory, ShaolinqServiceOptions options = null)
			where TDataModel : DataAccessModel, IIdentityServerClientDataAccessModel
		{
			factory.RegisterDataModelSingleton<IIdentityServerClientDataAccessModel, TDataModel>(options);

			factory.ClientStore = new Registration<IClientStore, ClientStore> { Mode = RegistrationMode.Singleton };
			factory.CorsPolicyService = new Registration<ICorsPolicyService, ClientConfigurationCorsPolicyService> { Mode = RegistrationMode.Singleton };

			return factory;
		}

		public static IdentityServerServiceFactory RegisterScopeStore<TDataModel>(this IdentityServerServiceFactory factory, ShaolinqServiceOptions options = null)
			where TDataModel : DataAccessModel, IIdentityServerScopeDataAccessModel
		{
			factory.RegisterDataModelSingleton<IIdentityServerScopeDataAccessModel, TDataModel>(options);

			factory.ScopeStore = new Registration<IScopeStore, ScopeStore> { Mode = RegistrationMode.Singleton };

			return factory;
		}

		private static void RegisterDataModelSingleton<TInterface, TImpl>(this IdentityServerServiceFactory factory, ShaolinqServiceOptions options)
			where TImpl : DataAccessModel, TInterface
			where TInterface : class
		{
			options = options ?? new ShaolinqServiceOptions();

			var configSection = options.DataAccessModelConfigSection;

			if (string.IsNullOrEmpty(configSection))
			{
				configSection = typeof (TImpl).Name;
			}

			var dictionaryKey = GetDataAccessModelDictionaryKey(typeof (TImpl), configSection);

			lock (LockObj)
			{
				DataAccessModel dataModel;

				if (!DataAccessModels.TryGetValue(dictionaryKey, out dataModel))
				{
					dataModel = DataAccessModel.BuildDataAccessModel<TImpl>();
					//dataModel.Create(DatabaseCreationOptions.DeleteExistingDatabase);
					DataAccessModels.Add(dictionaryKey, dataModel);
					factory.Register(new Registration<TImpl>((TImpl)dataModel));
				}

				factory.Register(new Registration<TInterface>((TImpl)dataModel));
			}
		}

		private static string GetDataAccessModelDictionaryKey(Type dataAccessModelType, string configSection)
		{
			return string.Format("{0}+{1}", dataAccessModelType.FullName, configSection);
		}
	}
}