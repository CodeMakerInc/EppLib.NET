using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using EppLib.Extensions.Iis;
using EppLib.Entities;

namespace EppLib.Tests
{
    [TestClass]
    public class LocalTest
    {
        private TestContext _context;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return _context; }
            set { _context = value; }
        }

        public LocalTest()
        {
        }

        /// <summary>
        /// Contact check response sample from RFC5733
        /// </summary>
        [TestMethod]
        [TestCategory("LocalResponse")]
        [DeploymentItem("TestData/ContactCheckResponse1.xml")]
        public void TestContactCheckResponse1()
        {
            byte[] input = File.ReadAllBytes("ContactCheckResponse1.xml");
            var response = new ContactCheckResponse(input);

            Assert.AreEqual("1000", response.Code);
            Assert.AreEqual("Command completed successfully", response.Message);

            //TODO
        }

        /// <summary>
        /// Contact info response sample from RFC5733
        /// </summary>
        [TestMethod]
        [TestCategory("LocalResponse")]
        [DeploymentItem("TestData/ContactInfoResponse1.xml")]
        public void TestContactInfoResponse1()
        {
            byte[] input = File.ReadAllBytes("ContactInfoResponse1.xml");
            var response = new IisContactInfoResponse(input);
            var contact = response.Contact;

            Assert.AreEqual("1000", response.Code);
            Assert.AreEqual("Command completed successfully", response.Message);

            Assert.AreEqual("sh8013", contact.Id);

            Assert.AreEqual("+1.7035555555", contact.Voice.Value);
            Assert.AreEqual("1234", contact.Voice.Extension);
            Assert.AreEqual("+1.7035555556", contact.Fax.Value);
            Assert.AreEqual("jdoe@example.com", contact.Email);

            Assert.AreEqual("int", contact.PostalInfo.m_type);
            Assert.AreEqual("John Doe", contact.PostalInfo.m_name);
            Assert.AreEqual("Example Inc.", contact.PostalInfo.m_org);

            Assert.AreEqual("123 Example Dr.", contact.PostalInfo.m_address.Street1);
            Assert.AreEqual("Suite 100", contact.PostalInfo.m_address.Street2);
            Assert.IsNull(contact.PostalInfo.m_address.Street3);
            Assert.AreEqual("Dulles", contact.PostalInfo.m_address.City);
            Assert.AreEqual("VA", contact.PostalInfo.m_address.StateProvince);
            Assert.AreEqual("20166-6503", contact.PostalInfo.m_address.PostalCode);
            Assert.AreEqual("US", contact.PostalInfo.m_address.CountryCode);

            Assert.AreEqual("SH8013-REP", contact.Roid);
            Assert.IsTrue(contact.StatusList.Contains("linked"));
            Assert.IsTrue(contact.StatusList.Contains("clientDeleteProhibited"));
            Assert.AreEqual("ClientY", contact.ClId);
            Assert.AreEqual("ClientX", contact.CrId);
            Assert.AreEqual("ClientX", contact.UpId);
            Assert.AreEqual("1999-04-03T22:00:00.0Z", contact.CrDate);
            Assert.AreEqual("1999-12-03T09:00:00.0Z", contact.UpDate);
            Assert.AreEqual("2000-04-08T09:00:00.0Z", contact.TrDate);

            Assert.AreEqual("2fooBAR", contact.Password);
            Assert.AreEqual(Contact.DiscloseFlags.All & ~Contact.DiscloseFlags.Voice & ~Contact.DiscloseFlags.Email, contact.DiscloseMask);
        }
    }
}
