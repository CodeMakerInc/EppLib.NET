using EppLib.Extensions.LaunchPhase.ClaimCheck;
using EppLib.Extensions.LaunchPhase.ClaimCreate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace EppLib.Tests
{
    [TestClass]
    public class LaunchPhaseExtensionLocalTest
    {
        #region Claim Check

        /// <summary>
        /// Claim Check Command for Trademark Clearinghouse (TMCH) Processing
        /// </summary>
        [TestMethod]
        [TestCategory("LaunchPhaseExtension")]
        [DeploymentItem("TestData/ClaimCheckCommand.xml")]
        public void TestClaimCheckCommand()
        {
            string expected = File.ReadAllText("ClaimCheckCommand.xml");

            var command = new ClaimCheck("example1.com");
            var xml = command.ToXml().InnerXml;

            Assert.AreEqual(expected, xml);
        }

        /// <summary>
        /// Claim Check Response for Trademark Clearinghouse (TMCH) Processing
        /// </summary>
        [TestMethod]
        [TestCategory("LaunchPhaseExtension")]
        [DeploymentItem("TestData/ClaimCheckResponse.xml")]
        public void TestClaimCheckResponse()
        {
            byte[] input = File.ReadAllBytes("ClaimCheckResponse.xml");
            var response = new ClaimCheckResponse(input);

            Assert.AreEqual("1000", response.Code);
            Assert.AreEqual("Command completed successfully", response.Message);

            Assert.AreEqual(2, response.Results.Count);

            Assert.AreEqual("example1.com", response.Results[0].Name);
            Assert.AreEqual(true, response.Results[0].ClaimExists);
            Assert.AreEqual("2016100401/2/D/7/LXsvtbnP8C5bUdxDuoj-XvJK0000000263", response.Results[0].ClaimKeys[0].Value);


            Assert.AreEqual("example2.com", response.Results[1].Name);
            Assert.AreEqual(false, response.Results[1].ClaimExists);
            Assert.AreEqual(0, response.Results[1].ClaimKeys.Count);
        }

        #endregion Claim Check

        #region Claim Create

        /// <summary>
        /// Claim Create Command for Trademark Clearinghouse (TMCH) Processing
        /// </summary>
        [TestMethod]
        [TestCategory("LaunchPhaseExtension")]
        [DeploymentItem("TestData/ClaimCreateCommand.xml")]
        public void TestClaimCreateCommand()
        {
            string expected = File.ReadAllText("ClaimCreateCommand.xml");

            var command = new ClaimCreate("example.com", "jd1234");
            command.Period = new Entities.DomainPeriod(2, "y");
            command.NameServers.Add("ns1.example.net");
            command.NameServers.Add("ns2.example.net");
            command.DomainContacts.Add(new Entities.DomainContact("sh8013", "admin"));
            command.DomainContacts.Add(new Entities.DomainContact("sh8013", "tech"));
            command.Password = "2fooBAR";
            command.TransactionId = "ABC-12345";

            command.Notices.Add(new ClaimNotice
            {
                NoticeId = "370d0b7c9223372036854775807",
                NotAfter = DateTime.Parse("2016-10-11T20:21:24.0066014Z"),
                AcceptedDate = DateTime.Parse("2016-10-11T19:21:24.0066014Z")
            });

            var xml = command.ToXml().InnerXml;

            Assert.AreEqual(expected, xml);
        }

        #endregion Claim Create
    }
}
