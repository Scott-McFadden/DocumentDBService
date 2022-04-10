using DocumentDbDAL;
using DocumentDbDAL.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class LookUpTests
    {
        static ADOBase<DocumentDBModel> docDb;
        static ADOBase<DomainLookUpModel> lookupdb;
        static ADOBase<ConnectionModel> connectiondb;


        const string Env = "LocalDev";

        static string ConnectionString = "Data Source=10.203.23.90\\NRCdBoxixxDba0A, 26090; Initial Catalog=CDMAPSPEED;Integrated Security=true";
        static JObject config;

        [ClassInitialize]
        public static void DomainLookUp(TestContext context)
        {
            Console.WriteLine(DateTime.Now);
            string f = File.ReadAllText(@"appsettings.json");
            config = JObject.Parse(f);
            ConnectionString = config.SelectToken("ConnectionStrings.LocalDev").Value<string>().ResolveIP("DocumentDb");
            docDb = new ADOBase<DocumentDBModel>(ConnectionString, "dbo.DocTable");
            lookupdb = new ADOBase<DomainLookUpModel>(ConnectionString, "dbo.LookUpTable");
            connectiondb = new ADOBase<ConnectionModel>(ConnectionString, "dbo.connections");

            ConnectionService.Populate(connectiondb);
            QueryDefService.Populate(docDb);
            DomainLookUpService.Populate(lookupdb);
        }

        [TestMethod]
        public void a_PopulateTest()
        {
            //DomainLookUpService.Populate(lookupdb);
            Assert.IsTrue(DomainLookUpService.GetCategories().Count > 0);

        }
        [TestMethod]
        public void b_GetValues()
        {
            var a = DomainLookUpService.GetValues("QueryDef", "DataType");
            Assert.IsTrue(a.Count > 0);

        }
        [TestMethod]
        public void b_GetCategories()
        {
            Assert.IsTrue(DomainLookUpService.GetCategories().Count > 0);
        }
        [TestMethod]
        public void c_TestCRUD()
        {
            
            DomainLookUpModel model = new DomainLookUpModel();
            model.Category = "unittest";
            model.Name = "test1";
            model.Value = "test1value";
            Console.WriteLine("model", model);
            DomainLookUpService.Refresh();
            var t1 = DomainLookUpService.Get(model.id);
            Assert.IsNull(t1.Category, "Record does not exist");
            var t2 = DomainLookUpService.Add(model);
            Assert.IsTrue(t2, "add reported successful");

            var t3 = DomainLookUpService.Get(model.id);
            Assert.IsTrue(model.id == t3.id, "added record matches inserted record");

            model.Value = "Updated";
            var t4 = DomainLookUpService.Replace(model);
            Assert.IsTrue(DomainLookUpService.Get(model.id).Value == model.Value, "updated value correctly");

            Assert.IsTrue(DomainLookUpService.Get(model.Category, model.Name, model.Value).Value == model.Value, "Test for get with cat and name");
            Assert.IsTrue(DomainLookUpService.Get(model.Key()).Value == model.Value, "Test for get by key");
            Assert.IsTrue(DomainLookUpService.Delete(model.Key()), "Test delete");
        }
        [TestMethod]
        public void d_TestAPIForms()
        {
            Assert.IsTrue(DomainLookUpService.Get("QueryDef_DataType_float").Name == "DataType", "Get(string cat, string name, string value)");
            Assert.IsTrue(DomainLookUpService.GetCategories().Count > 0, " GetCategories()");
            Assert.IsTrue(DomainLookUpService.GetValues("QueryDef", "DataType").Count > 0, "DomainLookUpService.GetValues(category, name)");
            Assert.IsTrue(DomainLookUpService.Get().Count > 0, "Get()");
            Assert.IsTrue(DomainLookUpService.Get(Guid.Parse("49E0DE13-FDB8-416E-AFA5-B6E8CF9AAE8B")).Value == "float", "GetLookUpValues(string id)");
            Assert.IsTrue(DomainLookUpService.Get().Count > 0, "PutValues(string cat, string name, string value)");





        }
    }
}
