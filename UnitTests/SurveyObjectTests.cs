using DocumentDbDAL;
using DocumentDbDAL.Models;
using DocumentDbDAL.Models.SurveyModels;
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
    public  class SurveyObjectTests
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

        }


        [TestMethod]
        public void A_Test_SurveyDefintion()
        {
            SurveyDefinitionModel sdm = new SurveyDefinitionModel();
            sdm.Name = "Test1";
            sdm.Description = "Unit Test Record";
            sdm.ConstantValues.Add("Constant1", "Constant One Value");
            sdm.ConstantValues.Add("Constant2", "2");
            sdm.ScopeDetails.Add(new ScopeItem { QueryDef = "DomainLookUp", VariableName = "id", DefaultValue = "5112b960-7229-4bbe-8551-6e5d8aab2edc" });
            
            string file = config.SelectToken($"Files.{Env}.SurveyDef1").Value<string>();
            string surveyDef = File.ReadAllText(file);

            sdm.SurveyDetails = surveyDef;

            if (sdm.Reasons.Count > 0)
            {
                Console.WriteLine("Validation Errors:");
                foreach (var i in sdm.Reasons)
                    Console.WriteLine(i);
            }
            else
                Console.WriteLine("No Validation Errors");
                

            Assert.IsTrue(sdm.Validate(), "Validating Survey Definition Model");
            Console.WriteLine(sdm.Serialize());



        }
    }
}
