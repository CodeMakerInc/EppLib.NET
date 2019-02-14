using EppLib.Entities;
using EppLib.Extensions.SecDNS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;

namespace EppLib.Tests
{
    [TestClass]
    public class SecDNSExtensionLocalTest
    {
        public TestContext TestContext { get; set; }
        
        [TestInitialize]
        public void TestSetup()
        {
            var testName = TestContext.TestName;
            var method = new StackFrame().GetMethod().DeclaringType.GetMethod(testName);
            var attributes = method.GetCustomAttributes(typeof(DeploymentItemAttribute), false);
            DeploymentUtility.CopyDeploymentItems(attributes);
        }

        [TestMethod]
        [TestCategory("SecDNSExtension")]
        [DeploymentItem("TestData/SecDNSCreateExtension.xml")]
        public void SecDNSCreateExtension()
        {
            string expected = File.ReadAllText("SecDNSCreateExtension.xml");

            var command = new DomainCreate("example1", "jd1234");
            command.Period = new DomainPeriod(2, "y");
            command.NameServers.Add("ns1.example.net");
            command.NameServers.Add("ns2.example.net");
            command.DomainContacts.Add(new DomainContact("sh8013", "admin"));
            command.DomainContacts.Add(new DomainContact("sh8013", "tech"));
            command.Password = "2fooBAR";
            command.TransactionId = "ABC-12345";

            // create extension
            var extension = new SecDNSCreate { MaxSigLife = 604800 };
            extension.DsData.Add(new SecDNSData {
                KeyTag = 12345,
                Algorithm = SecDNSAlgorithm.RSASHA1,
                Digest = "49FD46E6C4B45C55D4AC"
            });

            command.Extensions.Add(extension);

            var xml = command.ToXml().InnerXml;

            Assert.AreEqual(expected, xml);
        }

        [TestMethod]
        [TestCategory("SecDNSExtension")]
        [DeploymentItem("TestData/SecDNSUpdateExtension.xml")]
        public void SecDNSUpdateExtension()
        {
            string expected = File.ReadAllText("SecDNSUpdateExtension.xml");

            var command = new DomainUpdate("example1");

            var extension = new SecDNSUpdate();

            // remove
            extension.ToRemove.Add(new SecDNSData {
                KeyTag = 12345,
                Algorithm = SecDNSAlgorithm.RSASHA1,
                Digest = "49FD46E6C4B45C55D4AC"
            });

            // add
            extension.ToAdd.Add(new SecDNSData
            {
                KeyTag = 12346,
                Algorithm = SecDNSAlgorithm.RSASHA1,
                Digest = "38EC35D5B3A34B44C39B"
            });

            command.Extensions.Add(extension);

            var xml = command.ToXml().InnerXml;

            Assert.AreEqual(expected, xml);
        }
    }
}
