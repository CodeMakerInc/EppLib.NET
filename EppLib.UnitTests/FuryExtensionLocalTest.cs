using EppLib.Entities;
using EppLib.Extensions.Fury;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace EppLib.Tests
{
    [TestClass]
    public class FuryExtensionLocalTest
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
        [DeploymentItem("TestData/FuryLoginCommand.xml")]
        public void FuryLogin()
        {
            string expected = File.ReadAllText("FuryLoginCommand.xml");

            var command = new Login("username", "password");
            command.Options = new Options { MLang = "en", MVersion = "1.0" };

            var services = new Services();

            services.ObjURIs.Add("urn:ietf:params:xml:ns:epp-1.0");
            services.ObjURIs.Add("urn:ietf:params:xml:ns:domain-1.0");
            services.ObjURIs.Add("urn:ietf:params:xml:ns:host-1.0");
            services.ObjURIs.Add("urn:ietf:params:xml:ns:contact-1.0");

            //Fury extension
            services.Extensions.Add("urn:ietf:params:xml:ns:fury-2.0");

            command.Services = services;
            command.TransactionId = "ABC-12345";

            var xml = command.ToXml().InnerXml;

            Assert.AreEqual(expected, xml);
        }


        [TestMethod]
        [TestCategory("FuryExtension")]
        [DeploymentItem("TestData/FuryDomainCreateCommand.xml")]
        public void FuryCreateDomainWithPrivacy()
        {
            string expected = File.ReadAllText("FuryDomainCreateCommand.xml");

            var command = new DomainCreate("mydomain.ca", "jd1234");
            command.Password = "2fooBAR";
            command.DomainContacts.Add(new DomainContact("sh8013", "admin"));
            command.DomainContacts.Add(new DomainContact("sh8013", "tech"));

            command.Extensions.Add(new FuryDomainCreateExtension(true));

            var xml = command.ToXml().InnerXml;

            Assert.AreEqual(expected, xml);
        }

        [TestMethod]
        [TestCategory("FuryExtension")]
        [DeploymentItem("TestData/FuryDomainUpdateCommand.xml")]
        public void FuryUpdateDomainWithPrivacy()
        {
            string expected = File.ReadAllText("FuryDomainUpdateCommand.xml");

            var command = new DomainUpdate("example.com");

            var domainChange = new DomainChange { AuthInfo = "password2" };

            command.DomainChange = domainChange;
            command.Extensions.Add(new FuryDomainUpdateExtension(false));

            var xml = command.ToXml().InnerXml;

            Assert.AreEqual(expected, xml);
        }

        [TestMethod]
        [TestCategory("FuryExtension")]
        [DeploymentItem("TestData/FuryContactCreateCommand.xml")]
        public void FuryContactCreateWithPrivacy()
        {
            string expected = File.ReadAllText("FuryContactCreateCommand.xml");

            var registrantContact = new Contact("agreed6",
    "Test Contact1", "Test Organization",
    "Ottawa", "123 Main Street", "ON", "K1R 7S8", "CA",
    "jdoe@example.com",
    new Telephone { Value = "+1.6471114444", Extension = "333" },
    new Telephone { Value = "+1.6471114445" });
            //registrantContact.Password = "password";
            var command = new ContactCreate(registrantContact);
            command.Password = "password";
            var furyExtension = new FuryContactCreateExtension("EN", "CCT", null);

            command.Extensions.Add(furyExtension);

            var xml = command.ToXml().InnerXml;

            Assert.AreEqual(expected, xml);
        }


        [TestMethod]
        [TestCategory("FuryExtension")]
        [DeploymentItem("TestData/FuryContactUpdateCommand.xml")]
        public void FuryContactUpdateWithPrivacy()
        {
            string expected = File.ReadAllText("FuryContactUpdateCommand.xml");

            var command = new ContactUpdate("agreed2");

            //change contact email and language
            var contactChange = new ContactChange();

            contactChange.Email = "noprops@domain.fr";

            command.ContactChange = contactChange;

            command.Extensions.Add(new FuryContactUpdateExtension("en", "fr"));

            var xml = command.ToXml().InnerXml;

            Assert.AreEqual(expected, xml);
        }


        [TestMethod]
        [TestCategory("FuryExtension")]
        [DeploymentItem("TestData/FuryGetAgreement.xml")]
        public void FuryGetAgreement()
        {
            string expected = File.ReadAllText("FuryGetAgreement.xml");

            var command = new GetAgreement("EN", null);

            var xml = command.ToXml().InnerXml;

            Assert.AreEqual(expected, xml);
        }

        [TestMethod]
        [TestCategory("FuryExtension")]
        [DeploymentItem("TestData/FuryGetAgreementResponse.xml")]
        public void FuryGetAgreementResponse()
        {
            string str = File.ReadAllText("FuryGetAgreementResponse.xml");

            var getAgreementResponse = new GetAgreementResponse(Encoding.UTF8.GetBytes(str));

            Assert.AreEqual(getAgreementResponse.Agreement, "Cogito ergo sum");
        }
    }
}
