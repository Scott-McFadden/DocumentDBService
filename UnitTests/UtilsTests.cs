using DocumentDbDAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class UtilsTests
    {
        [TestMethod]
        public void TestGetIPAddress()
        {
            string ip = Utils.GetIPAddress();
            Console.WriteLine("ip address: " + ip);
            Assert.IsNotNull(ip);

        }

        [TestMethod]
        public void TestResolveIP()
        {
            string s1 = "Data Source=*DocumentDB*; Initial Catalog=DocumentDb; ".ResolveIP("DocumentDB");
            Assert.IsTrue(s1.Contains("192.168.68.105"), "Test 1 - document db resolved");
            string s2 = "Data Source=*LOST*; Initial Catalog=DocumentDb; ".ResolveIP("LOST");
            Assert.IsTrue(s2.Contains("LOST"), "Test 2 - Lost not in resolve list - no changes made");
            string s3 = "Data Source=123.123.123.123,122; Initial Catalog=DocumentDb; ".ResolveIP("DocumentDB");
            Assert.IsFalse(s3.Contains("192.168.68.105"), "Test 3 - named connection was not listed");
            string s4 = "Data Source=*TEST*; Initial Catalog=DocumentDb; ".ResolveIP("TEST");
            Assert.IsTrue(s4.Contains("localhost"), "Test 4 - second Item listed was replaced");
            string s5 = "Data Source=*IPADDRESS*; Initial Catalog=DocumentDb; ".ResolveIP("IPADDRESS");
            Assert.IsTrue(s5.Contains("192.168.68.105"), "Test 1 - IPAddress has been resolved.");



        }
    }
}
