using EppLib.Entities;
using EppLib.Extensions.Idn;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;

namespace EppLib.Tests
{
    [TestClass]
    public class IdnExtensionLocalTest
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
        [TestCategory("FuryExtension")]
        [DeploymentItem("TestData/IdnDomainCreateCommand.xml")]
        public void Test()
        {
            string expected = File.ReadAllText("IdnDomainCreateCommand.xml");

            var command = new DomainCreate("xn--espaol-zwa.example.com", "jd1234");
            command.Password = "2fooBAR";
            command.DomainContacts.Add(new DomainContact("sh8013", "admin"));
            command.DomainContacts.Add(new DomainContact("sh8013", "tech"));

            command.Extensions.Add(new IdnExtension("es", "español.example.com"));

            var xml = command.ToXml().InnerXml;

            Assert.AreEqual(expected, xml);
        }
        
    }
}
