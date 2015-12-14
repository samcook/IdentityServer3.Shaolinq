using IdentityServer3.Shaolinq.DataModel;
using log4net.Config;
using NUnit.Framework;
using Shaolinq;
using Shaolinq.Sqlite;

namespace IdentityServer3.Shaolinq.Tests
{
	[TestFixture]
    public class DataAccessModelTests
    {
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			XmlConfigurator.Configure();
		}

		[Test]
		public void TestCreateDataAccessModel()
		{
			var dataModel = DataAccessModel.BuildDataAccessModel<IdentityServerDataAccessModel>(SqliteConfiguration.Create(":memory:", null));

			dataModel.Create(DatabaseCreationOptions.DeleteExistingDatabase);
		}
    }
}
