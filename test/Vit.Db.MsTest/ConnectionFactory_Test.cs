using Microsoft.VisualStudio.TestTools.UnitTesting;

using Vit.Core.Util.ConfigurationManager;
using Vit.Db.Util.Data;
using Vit.Extensions.Db_Extensions;



namespace Vit.Db.MsTest
{

    [TestClass]
    public partial class ConnectionFactory_Test
    {

        [TestMethod]
        public void Test()
        {
            using var conn = ConnectionFactory.GetConnection(Appsettings.json.GetByPath<ConnectionInfo>("App.Db"));
            //using var conn = ConnectionFactory.GetConnection("App.Db");

            var result = conn.ExecuteScalar(sql: "select 1 ");

            Assert.AreEqual(1L, result);

        }
    }
}
