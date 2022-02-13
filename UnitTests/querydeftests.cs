using DocumentDbDAL;
using DocumentDbDAL.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class QueryDefTests
    {

         static ADOBase<DocumentDBModel> docDb;
        static ADOBase<DomainLookUpModel> lookupdb;
        static ADOBase<ConnectionModel> connectiondb;
        //static ConfigurationManager config; 


        const string Env = "LocalDev";


        static string ConnectionString = "Data Source=10.203.23.90\\NRCdBoxixxDba0A, 26090; Initial Catalog=CDMAPSPEED;Integrated Security=true";
        static JObject config;


        [ClassInitialize]
         public static void DomainLookUp(TestContext context)
        {
            
            string f = File.ReadAllText(@"appsettings.json");
            config = JObject.Parse(f);
            ConnectionString = config.SelectToken("ConnectionStrings.LocalDev").Value<string>();
            docDb = new ADOBase<DocumentDBModel>(ConnectionString, "dbo.DocTable");
            lookupdb = new ADOBase<DomainLookUpModel>(ConnectionString, "dbo.LookUpTable");
            connectiondb = new ADOBase<ConnectionModel>(ConnectionString, "dbo.connections");

            ConnectionService.Populate(connectiondb);
            QueryDefService.Populate(docDb);
            
        }

        [TestMethod]
        public void GetConnection()
        {

            string tar = "Connections";
            var c = ConnectionService.Get(tar);
            Assert.AreEqual<string>(tar, c.name);

            
        }

        [TestMethod]
        public void GetQueryDef()
        {
            string tar = "DomainLookUp";
            var c = QueryDefService.Get(tar);
            Assert.AreEqual<string>(tar, c.name);

        }

        [TestMethod]
        public void UseQDefGet()
        {
            string tar = "DomainLookUp";
            var queryDef = QueryDefService.Get(tar);
            Assert.AreEqual<string>(tar, queryDef.name, "ensure correct definition has been retrieved");
            var connection = ConnectionService.Get(queryDef.connection);

            Assert.AreEqual<string>(queryDef.connection, connection.name, "ensure correct connection definition was retrieved");
        }

        [TestMethod]
        public void TestGetOneInSQL()
        {
            ExecuteQueryDef EQD = new();
            EQD.QueryDefName = "DomainLookUp";

            string id = config.SelectToken($"Files.{Env}.TestGetOneInSQL").Value<string>();
            JObject result = EQD.GetOne(id);

            Assert.IsTrue(result["Value"].Value<string>() == "test");
        }
        
        [TestMethod]
        public void SelectFieldList()
        {  // obsolete test
            //string tar = "DomainLookUp";
            //var queryDef = QueryDefService.Get(tar);
            //string result = queryDef.fields.SelectFieldList();
            //Assert.IsTrue(result == " [id], [name], [category], [Value]", "correct field list has been returned and formated correctly.");
        }

        [TestMethod]
        public void TestGetWithCriteriainSql()
        {
            ExecuteQueryDef EQD = new();
            EQD.QueryDefName = "DomainLookUp";
            JArray result = EQD.Get(" Category='QueryDef' and Name='DataType' ");
            Console.WriteLine(result.ToString());
            Assert.IsTrue(result.Count  == 12);

        }


        [TestMethod]
        public void TestGetAllinSql() 
        {
            ExecuteQueryDef EQD = new();
            EQD.QueryDefName = "DomainLookUp";
           
            JArray result = EQD.Get();

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void TestAddUpdateDelete()
        {
            Console.WriteLine(DateTime.Now);
            
            DomainLookUpModel model = new();
            model.Category = "test";
            model.Name = "unittest";
            model.Value = "test";

            ExecuteQueryDef EQD = new();
            EQD.QueryDefName = "DomainLookUp";

            var result = EQD.Insert(JObject.Parse(model.serialize()));

            Assert.IsTrue(result == 1, "add result");

            model.Value = "Updated";
            var r3 = EQD.Update(JObject.Parse(model.serialize()));

            Assert.IsTrue(r3 == 1, "update result");

            var r2 = EQD.Delete(model.id.ToString());
            Assert.IsTrue(r2 == 1, "delete result");

            var r4 = EQD.GetOne(model.id.ToString());
             
            Console.WriteLine(r4.ToString());
            Assert.IsTrue(r4.Count == 0, "count not find record") ;

        }

        [TestMethod]
        public void TestQueryDefGet()
        {
            var ret = QueryDefService.GetName();
            Assert.IsTrue(ret.Count > 0);
        }


    }
}
