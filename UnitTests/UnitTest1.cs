using DocumentDbDAL;
using DocumentDbDAL.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

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
            var r = db.Get(new Guid("12B7F49B-5A4D-48DB-9452-96CF0EC66C6B"));

            Assert.IsTrue(r.id == new Guid("12B7F49B-5A4D-48DB-9452-96CF0EC66C6B"));
        }

        [TestMethod]
        public void GetManyRecordWithNullTest()
        {


            ADOBase<DocumentDBModel> db = new ADOBase<DocumentDBModel>(ConnectionString, "dbo.DocTable");
            var r = db.Get( );

            Assert.IsTrue(r.Count == 2);
        }

        [TestMethod]
        public void GetManyRecordWithCat()
        {
            ADOBase<DocumentDBModel> db = new ADOBase<DocumentDBModel>(ConnectionString, "dbo.DocTable");
            var r = db.Get("where category = 'General'");

            Assert.IsTrue(r.Count == 2 );
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
    }
}
