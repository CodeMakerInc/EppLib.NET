using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EppLib.Entities;
using EppLib.Extensions.Nominet.DomainCheck;
using EppLib.Extensions.Nominet.DomainInfo;
using EppLib.Extensions.Nominet.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EppLib.Tests
{
    [TestClass]
    public class NominetExtensionLocalTest
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

        public NominetExtensionLocalTest(){}

        #region Domain Check

        /// <summary>
        /// Nominet Domain check command
        /// example http://registrars.nominet.org.uk/namespace/uk/registration-and-domain-management/epp-commands#check
        /// </summary>
        [TestMethod]
        [TestCategory("NominetExtension")]
        [TestCategory("LocalCommand")]
        [DeploymentItem("TestData/NominetDomainCheckCommand1.xml")]
        public void TestNominetDomainCheckCommand1()
        {
            string expected = File.ReadAllText("NominetDomainCheckCommand1.xml");

            var command = new NominetDomainCheck("example1.uk");
            command.Email = "john.smith@example.uk";
            command.PostalInfo = new PostalInfo
            {
                m_type = "loc",
                m_name = "Contact name",
                m_org = "Org name",
                m_address = new PostalAddress
                {
                    Street1 = "222 Test Street",
                    City = "Test City",
                    StateProvince = "Testshire",
                    PostalCode = "TE57 1NG",
                    CountryCode = "GB"
                }
            };
            command.TransactionId = "ABC-12345";
            Assert.AreEqual(expected, command.ToXml().InnerXml);
        }

        #endregion

        #region Domain Info

        /// <summary>
        /// Nominet Domain check command
        /// example http://registrars.nominet.org.uk/namespace/uk/registration-and-domain-management/epp-commands#check
        /// </summary>
        [TestMethod]
        [TestCategory("NominetExtension")]
        [TestCategory("LocalCommand")]
        [DeploymentItem("TestData/NominetDomainInfoCommand1.xml")]
        public void TestNominetDomainInfoCommand1()
        {
            byte[] input = File.ReadAllBytes("NominetDomainInfoCommand1.xml");
            var response = new NominetDomainInfoResponse(input);

            Assert.AreEqual("30", response.Domain.AutoBill);
            Assert.AreEqual("1000", response.Code);
            Assert.AreEqual("Command completed successfully", response.Message);

        }

        #endregion

        #region Domain Name Cancellation Notification

        /// <summary>
        /// Poll response, Domain Name Cancellation notification messages
        /// </summary>
        [TestMethod]
        [TestCategory("LocalResponse")]
        [DeploymentItem("TestData/DomainNameCancelledNotification.xml")]
        public void TestPollMsgsResponse2()
        {
            byte[] input = File.ReadAllBytes("DomainNameCancelledNotification.xml");
            var response = new PollResponse(input);

            Assert.AreEqual("1301", response.Code);
            Assert.AreEqual("Command completed successfully; ack to dequeue", response.Message);

            Assert.AreEqual("Domain Name Cancellation Notification", response.Body);

            Assert.AreEqual("ABC-12345", response.ClientTransactionId);
            Assert.AreEqual("54322-XYZ", response.ServerTransactionId);

            var notification = new DomainCancellationNotification(File.ReadAllText("DomainNameCancelledNotification.xml"));

            Assert.AreEqual("cancelleddomain.co.uk", notification.DomainName);
            Assert.AreEqual("originator@nominet.org.uk", notification.Originator);
        }

        #endregion
    }
}
