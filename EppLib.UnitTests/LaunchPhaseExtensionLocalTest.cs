using EppLib.Extensions.LaunchPhase.ClaimCheck;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        [TestCategory("LocalCommand")]
        [DeploymentItem("TestData/ClaimCheckCommand.xml")]
        public void TestClaimCheckCommand()
        {
            string expected = File.ReadAllText("ClaimCheckCommand.xml");

            var command = new ClaimCheck("example1.com");
            var xml = command.ToXml().InnerXml;

            Assert.AreEqual(expected, xml);
        }

        #endregion Domain Check
    }
}
