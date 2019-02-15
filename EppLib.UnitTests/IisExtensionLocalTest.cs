using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EppLib.Entities;
using EppLib.Extensions.Iis;
using System.Diagnostics;

namespace EppLib.Tests
{
    [TestClass]
    public class IisExtensionLocalTest
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

        public IisExtensionLocalTest()
        {
        }

        /// <summary>
        /// Sample data from:
        /// https://www.iis.se/docs/EPP-Server-Protocol-description.pdf
        /// </summary>
        [TestMethod]
        [TestCategory("IisExtension")]
        [TestCategory("LocalResponse")]
        [DeploymentItem("TestData/IisContactInfoResponse1.xml")]
        public void TestIisContactInfoResponse1()
        {
            byte[] input = File.ReadAllBytes("IisContactInfoResponse1.xml");
            var response = new IisContactInfoResponse(input);
            var contact = response.Contact;

            Assert.AreEqual("1000", response.Code);
            Assert.AreEqual("Command completed successfully", response.Message);

            // IIS Extension
            Assert.AreEqual("[SE]802405-0190", contact.OrganizationNumber);
            Assert.AreEqual("SE802405019001", contact.VatNumber);

            // RFC5733
            Assert.AreEqual("sh0808-8013", contact.Id);

            Assert.AreEqual("+46.835555555", contact.Voice.Value);
            Assert.AreEqual("+46.835555556", contact.Fax.Value);
            Assert.AreEqual("er.sv@nic.se", contact.Email);

            Assert.AreEqual("loc", contact.PostalInfo.m_type);
            Assert.AreEqual("Erik Svensson", contact.PostalInfo.m_name);
            Assert.AreEqual("Example AB", contact.PostalInfo.m_org);

            Assert.AreEqual("c/o Johan Persson", contact.PostalInfo.m_address.Street1);
            Assert.AreEqual("Götgatan 13", contact.PostalInfo.m_address.Street2);
            Assert.IsNull(contact.PostalInfo.m_address.Street3);
            Assert.AreEqual("Stockholm", contact.PostalInfo.m_address.City);
            Assert.IsNull(contact.PostalInfo.m_address.StateProvince);
            Assert.AreEqual("11346", contact.PostalInfo.m_address.PostalCode);
            Assert.AreEqual("SE", contact.PostalInfo.m_address.CountryCode);

            Assert.AreEqual("CONTACT-12345", contact.Roid);
            Assert.IsTrue(contact.StatusList.Contains("ok"));
            Assert.AreEqual("ClientY", contact.ClId);
            Assert.AreEqual("ClientX", contact.CrId);
            Assert.AreEqual("ClientX", contact.UpId);
            Assert.AreEqual("1999-04-03T22:00:00.0Z", contact.CrDate);
            Assert.AreEqual("1999-12-03T09:00:00.0Z", contact.UpDate);
            Assert.IsNull(contact.Password);
            Assert.AreEqual(Contact.DiscloseFlags.All & ~Contact.DiscloseFlags.Voice & ~Contact.DiscloseFlags.Email, contact.DiscloseMask);
        }
    }
}
