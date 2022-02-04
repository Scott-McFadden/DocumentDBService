using DocumentDbDAL;
using DocumentDbDAL.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class DocumentDBDALTests
    {

        string   ConnectionString = "Data Source=localhost; Initial Catalog=DocumentDb; User Id=dev; Password=dev";

        [TestMethod]
        public void Get1RecordTest()
        {
            ADOBase<DocumentDBModel> db = new ADOBase<DocumentDBModel>(ConnectionString, "dbo.DocTable");
            var r = db.Get(new Guid("A7EACE2C-E0F4-44B5-9C46-043664CE149A"));

            Assert.IsTrue(r.id == new Guid("A7EACE2C-E0F4-44B5-9C46-043664CE149A"));
        }

        [TestMethod]
        public void GetManyRecordWithNullTest()
        {


            ADOBase<DocumentDBModel> db = new ADOBase<DocumentDBModel>(ConnectionString, "dbo.DocTable");
            var r = db.Get( );

            Assert.IsTrue(r.Count == 1);
        }

        [TestMethod]
        public void GetManyRecordWithCat()
        {
            ADOBase<DocumentDBModel> db = new ADOBase<DocumentDBModel>(ConnectionString, "dbo.DocTable");
            var r = db.Get("where category = 'QueryDef'");

            Assert.IsTrue(r.Count == 1 );
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
            //string def = File.ReadAllText(@"C:\Users\impro\source\repos\DocumentDBService\DocumentDbDAL\BaseDefintions\DomainLookUp.def.json");
            string def = File.ReadAllText(@"C:\Users\impro\source\repos\DocumentDBService\DocumentDbDAL\BaseDefintions\Connection.def.json");
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
