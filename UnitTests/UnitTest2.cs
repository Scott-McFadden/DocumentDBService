using DocumentDbDAL;
using DocumentDbDAL.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class DocumentDBDALTests
    {

        static string Env = "LocalDev";
        static string   ConnectionString = "Data Source=10.203.23.90\\NRCdBoxixxDba0A, 26090; Initial Catalog=CDMAPSPEED;Integrated Security=true";
        static JObject config;
        
        [ClassInitialize]
        public static void Init(TestContext context)
        {

            string f = File.ReadAllText(@"appsettings.json");
              config = JObject.Parse(f);
            ConnectionString = config.SelectToken($"ConnectionStrings.{Env}").Value<string>().ResolveIP("DocumentDb");
            Env = config.SelectToken($"env").Value<string>();
        }

        [TestMethod]
        public void Get1RecordTest()
        {
            ADOBase<DocumentDBModel> db = new ADOBase<DocumentDBModel>(ConnectionString, "dbo.DocTable");
            string Id = config.SelectToken($"Files.{Env}.ID").Value<string>();
            var r = db.Get(new Guid(Id));

            Assert.IsTrue(r.id == new Guid(Id));
        }

        [TestMethod]
        public void GetManyRecordWithNullTest()
        {


            ADOBase<DocumentDBModel> db = new ADOBase<DocumentDBModel>(ConnectionString, "dbo.DocTable");
            var r = db.Get( );

            Assert.IsTrue(r.Count > 0);
        }

        [TestMethod]
        public void GetManyRecordWithCat()
        {
            ADOBase<DocumentDBModel> db = new ADOBase<DocumentDBModel>(ConnectionString, "dbo.DocTable");
            var r = db.Get("where category = 'QueryDef'");

            Assert.IsTrue(r.Count > 0 );
        }

        [TestMethod]
        public void TestGetPropertyNames()
        {
            DocumentDBModel m = new();

            List<string> ss = new List<string>();
            ss = m.GetPropertyNames();
            Assert.IsTrue(ss.Count  > 0);
        }

        [TestMethod]
        public void TestConvertListOfStringsToCommaSep()
        {
            DocumentDBModel m = new();
            string s = m.GetPropertyNames().ConvertListOfStringsToCommaSep();

            Assert.IsFalse(String.IsNullOrEmpty(s), "string not empty");
            Console.WriteLine(s);
        }

        [TestMethod]
        public void TestAddUpdateAndDelete()
        {
            DocumentDBModel m = new();
            m.category = "General";
            m.Owner = "scott";
            m.JsonDoc = "{ \"name\" : \"Scott\"}";

            ADOBase <DocumentDBModel> db = new ADOBase<DocumentDBModel>(ConnectionString, "dbo.DocTable");
            var r = db.Add(m);

            Assert.IsTrue(r == 1, "one added"); 

            m.Owner = "NewOwner";

            r = db.Update(m);
            Assert.IsTrue(r == 1, "one updated");
            r = db.Delete(m.id);

            Assert.IsTrue(r == 1, "one deleted");
        }


        [TestMethod]
        public void TestLookUp()
        {
            DomainLookUpModel m = new();
            m.Category = "DocumentDB";
            m.Name = "Category";
            m.Value = "Test";

            ADOBase<DomainLookUpModel> db = new ADOBase<DomainLookUpModel>(ConnectionString, "dbo.LookUpTable");
            var r = db.Add(m);

            Assert.IsTrue(r == 1, "one added");

            m.Value = "Updated";

            r = db.Update(m);
            Assert.IsTrue(r == 1, "one updated");
            r = db.Delete(m.id);

            Assert.IsTrue(r == 1, "one deleted");
        }

        [TestMethod]
        public void TestDuplicateAdd()
        {

            // this will only really test anything if there is a duplicate record in the database.
            // if the record does not exist, it will add it.  
            // So this test may need to be run twice to prove that it works if the records added does not already exist.

            string file = config.SelectToken($"Files.{Env}.QueryDef1").Value<string>();
            string def = File.ReadAllText(file);
            QueryDefModel model = QueryDefModel.deserialize(def);
            model.id = Guid.NewGuid();
            DocumentDBModel doc = new();
            doc.category = "QueryDef";
            doc.JsonDoc = model.serialize();
            doc.id = model.id;
            doc.KeyValue = model.name;
            doc.Owner = "scott";
            int r=0;

            try
            {
                ADOBase<DocumentDBModel> db = new ADOBase<DocumentDBModel>(ConnectionString, "dbo.DocTable");
                r = db.Add(doc);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Assert.IsTrue(ex.Message.Contains("duplicate"));
            }
            
            //Assert.IsTrue(r == 0);

        }


    }
}
