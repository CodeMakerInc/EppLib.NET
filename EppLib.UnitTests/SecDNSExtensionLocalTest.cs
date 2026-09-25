using EppLib.Entities;
using EppLib.Extensions.SecDNS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

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

        private const string SecDnsNamespace = "urn:ietf:params:xml:ns:secDNS-1.1";

        // Returns the secDNS extension element of a command, after validating it against secDNS-1.1.xsd.
        private static XmlElement SecDnsElement(XmlDocument commandXml)
        {
            var namespaces = new XmlNamespaceManager(commandXml.NameTable);
            namespaces.AddNamespace("secDNS", SecDnsNamespace);
            var element = (XmlElement)commandXml.SelectSingleNode("//secDNS:*[parent::*[local-name()='extension']]", namespaces);
            Assert.IsNotNull(element, "no secDNS extension element");

            var doc = new XmlDocument();
            doc.AppendChild(doc.ImportNode(element, true));
            var schemaPath = Path.Combine(Path.GetDirectoryName(typeof(SecDNSExtensionLocalTest).Assembly.Location), "TestData", "secDNS-1.1.xsd");
            doc.Schemas.Add(SecDnsNamespace, schemaPath);
            doc.Validate((sender, e) => Assert.Fail("secDNS schema validation: " + e.Message));

            return doc.DocumentElement;
        }

        private static string Text(XmlNode node, string xpath)
        {
            var namespaces = new XmlNamespaceManager(node.OwnerDocument.NameTable);
            namespaces.AddNamespace("secDNS", SecDnsNamespace);
            var match = node.SelectSingleNode(xpath, namespaces);
            return match?.InnerText;
        }

        private static int Count(XmlNode node, string xpath)
        {
            var namespaces = new XmlNamespaceManager(node.OwnerDocument.NameTable);
            namespaces.AddNamespace("secDNS", SecDnsNamespace);
            return node.SelectNodes(xpath, namespaces).Count;
        }

        private static SecDNSKeyData RfcKey()
        {
            return new SecDNSKeyData { Flags = 257, Protocol = 3, Algorithm = SecDNSAlgorithm.RSAMD5, PublicKey = "AQPJ////4Q==" };
        }

        /// <summary>
        /// RFC 5910 section 5.2.1, DS Data Interface with the optional key data (issue #31).
        /// </summary>
        [TestMethod]
        [TestCategory("SecDNSExtension")]
        public void SecDNSCreateDsDataWithKeyData()
        {
            var command = new DomainCreate("example.com", "jd1234");
            var extension = new SecDNSCreate { MaxSigLife = 604800 };
            extension.DsData.Add(new SecDNSData
            {
                KeyTag = 12345,
                Algorithm = SecDNSAlgorithm.DSA,
                Digest = "49FD46E6C4B45C55D4AC",
                KeyData = RfcKey()
            });
            command.Extensions.Add(extension);

            var secDns = SecDnsElement(command.ToXml());

            Assert.AreEqual("604800", Text(secDns, "secDNS:maxSigLife"));
            Assert.AreEqual("12345", Text(secDns, "secDNS:dsData/secDNS:keyTag"));
            Assert.AreEqual("3", Text(secDns, "secDNS:dsData/secDNS:alg"));
            Assert.AreEqual("1", Text(secDns, "secDNS:dsData/secDNS:digestType"));
            Assert.AreEqual("257", Text(secDns, "secDNS:dsData/secDNS:keyData/secDNS:flags"));
            Assert.AreEqual("3", Text(secDns, "secDNS:dsData/secDNS:keyData/secDNS:protocol"));
            Assert.AreEqual("1", Text(secDns, "secDNS:dsData/secDNS:keyData/secDNS:alg"));
            Assert.AreEqual("AQPJ////4Q==", Text(secDns, "secDNS:dsData/secDNS:keyData/secDNS:pubKey"));
        }

        /// <summary>
        /// RFC 5910 section 5.2.1, Key Data Interface (issue #31).
        /// </summary>
        [TestMethod]
        [TestCategory("SecDNSExtension")]
        public void SecDNSCreateKeyData()
        {
            var command = new DomainCreate("example.com", "jd1234");
            var extension = new SecDNSCreate();
            extension.KeyData.Add(RfcKey());
            command.Extensions.Add(extension);

            var secDns = SecDnsElement(command.ToXml());

            Assert.AreEqual(0, Count(secDns, "secDNS:dsData"));
            Assert.AreEqual(1, Count(secDns, "secDNS:keyData"));
            Assert.AreEqual("AQPJ////4Q==", Text(secDns, "secDNS:keyData/secDNS:pubKey"));
        }

        [TestMethod]
        [TestCategory("SecDNSExtension")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SecDNSCreateRejectsDsDataAndKeyDataTogether()
        {
            var command = new DomainCreate("example.com", "jd1234");
            var extension = new SecDNSCreate();
            extension.DsData.Add(new SecDNSData { KeyTag = 1, Algorithm = SecDNSAlgorithm.RSASHA256, Digest = "AB" });
            extension.KeyData.Add(RfcKey());
            command.Extensions.Add(extension);

            command.ToXml();
        }

        /// <summary>
        /// Key tags go up to 65535, and modern DS records use SHA-256 with algorithm 13.
        /// </summary>
        [TestMethod]
        [TestCategory("SecDNSExtension")]
        public void SecDNSCreateHighKeyTagAndSha256()
        {
            var command = new DomainCreate("example.com", "jd1234");
            var extension = new SecDNSCreate();
            extension.DsData.Add(new SecDNSData
            {
                KeyTag = 65535,
                Algorithm = SecDNSAlgorithm.ECDSAP256SHA256,
                DigestType = SecDNSDigestType.SHA256,
                Digest = "E2D3C916F6DEEAC73294E8268FB5885044A833FC5459588F4A9184CFC41A5766"
            });
            command.Extensions.Add(extension);

            var secDns = SecDnsElement(command.ToXml());

            Assert.AreEqual("65535", Text(secDns, "secDNS:dsData/secDNS:keyTag"));
            Assert.AreEqual("13", Text(secDns, "secDNS:dsData/secDNS:alg"));
            Assert.AreEqual("2", Text(secDns, "secDNS:dsData/secDNS:digestType"));
        }

        /// <summary>
        /// RFC 5910 section 5.2.5: urgent update that replaces all DNSSEC data and changes maxSigLife.
        /// </summary>
        [TestMethod]
        [TestCategory("SecDNSExtension")]
        public void SecDNSUpdateRemoveAllAddChangeUrgent()
        {
            var command = new DomainUpdate("example.com");
            var extension = new SecDNSUpdate { RemoveAll = true, MaxSigLife = 605900, Urgent = true };
            extension.ToAdd.Add(new SecDNSData { KeyTag = 12346, Algorithm = SecDNSAlgorithm.DSA, Digest = "38EC35D5B3A34B44C39B" });
            command.Extensions.Add(extension);

            var secDns = SecDnsElement(command.ToXml());

            Assert.AreEqual("true", secDns.GetAttribute("urgent"));
            Assert.AreEqual("true", Text(secDns, "secDNS:rem/secDNS:all"));
            Assert.AreEqual("12346", Text(secDns, "secDNS:add/secDNS:dsData/secDNS:keyTag"));
            Assert.AreEqual("605900", Text(secDns, "secDNS:chg/secDNS:maxSigLife"));
        }

        /// <summary>
        /// RFC 5910 section 5.2.5: add and remove DNSKEY data.
        /// </summary>
        [TestMethod]
        [TestCategory("SecDNSExtension")]
        public void SecDNSUpdateKeyData()
        {
            var command = new DomainUpdate("example.com");
            var extension = new SecDNSUpdate();
            extension.KeyDataToRemove.Add(RfcKey());
            extension.KeyDataToAdd.Add(new SecDNSKeyData { Flags = 257, Algorithm = SecDNSAlgorithm.ED25519, PublicKey = "l02Woi0iS8Aa25FQkUd9RMzZHJpBoRQwAQEX1SxZJA4=" });
            command.Extensions.Add(extension);

            // Check the raw command: schema validation fills in the urgent="false" default.
            Assert.IsFalse(command.ToXml().OuterXml.Contains("urgent"));

            var secDns = SecDnsElement(command.ToXml());

            Assert.AreEqual("AQPJ////4Q==", Text(secDns, "secDNS:rem/secDNS:keyData/secDNS:pubKey"));
            Assert.AreEqual("15", Text(secDns, "secDNS:add/secDNS:keyData/secDNS:alg"));
            Assert.AreEqual(0, Count(secDns, "secDNS:chg"));
        }

        /// <summary>
        /// RFC 5910 section 5.2.5: change only maxSigLife.
        /// </summary>
        [TestMethod]
        [TestCategory("SecDNSExtension")]
        public void SecDNSUpdateChangeMaxSigLifeOnly()
        {
            var command = new DomainUpdate("example.com");
            command.Extensions.Add(new SecDNSUpdate { MaxSigLife = 605900 });

            var secDns = SecDnsElement(command.ToXml());

            Assert.AreEqual(0, Count(secDns, "secDNS:rem"));
            Assert.AreEqual(0, Count(secDns, "secDNS:add"));
            Assert.AreEqual("605900", Text(secDns, "secDNS:chg/secDNS:maxSigLife"));
        }

        [TestMethod]
        [TestCategory("SecDNSExtension")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SecDNSUpdateRejectsRemoveAllWithSpecificRemovals()
        {
            var command = new DomainUpdate("example.com");
            var extension = new SecDNSUpdate { RemoveAll = true };
            extension.ToRemove.Add(new SecDNSData { KeyTag = 1, Algorithm = SecDNSAlgorithm.RSASHA256, Digest = "AB" });
            command.Extensions.Add(extension);

            command.ToXml();
        }

        private const string InfoResponseTemplate =
            "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>" +
            "<epp xmlns=\"urn:ietf:params:xml:ns:epp-1.0\"><response><result code=\"1000\"><msg>Command completed successfully</msg></result>" +
            "<resData><domain:infData xmlns:domain=\"urn:ietf:params:xml:ns:domain-1.0\"><domain:name>example.com</domain:name><domain:roid>EXAMPLE1-REP</domain:roid></domain:infData></resData>" +
            "{0}<trID><clTRID>ABC-12345</clTRID><svTRID>54322-XYZ</svTRID></trID></response></epp>";

        /// <summary>
        /// RFC 5910 section 5.1.2, info response with DS data and key data.
        /// </summary>
        [TestMethod]
        [TestCategory("SecDNSExtension")]
        public void SecDNSInfDataDsData()
        {
            var xml = string.Format(InfoResponseTemplate,
                "<extension><secDNS:infData xmlns:secDNS=\"urn:ietf:params:xml:ns:secDNS-1.1\"><secDNS:maxSigLife>604800</secDNS:maxSigLife>" +
                "<secDNS:dsData><secDNS:keyTag>12345</secDNS:keyTag><secDNS:alg>3</secDNS:alg><secDNS:digestType>1</secDNS:digestType><secDNS:digest>49FD46E6C4B45C55D4AC</secDNS:digest>" +
                "<secDNS:keyData><secDNS:flags>257</secDNS:flags><secDNS:protocol>3</secDNS:protocol><secDNS:alg>1</secDNS:alg><secDNS:pubKey>AQPJ////4Q==</secDNS:pubKey></secDNS:keyData></secDNS:dsData>" +
                "<secDNS:dsData><secDNS:keyTag>54321</secDNS:keyTag><secDNS:alg>13</secDNS:alg><secDNS:digestType>2</secDNS:digestType><secDNS:digest>E2D3C916F6DEEAC7</secDNS:digest></secDNS:dsData>" +
                "</secDNS:infData></extension>");

            var info = SecDNSInfData.FromResponse(new DomainInfoResponse(xml));

            Assert.IsNotNull(info);
            Assert.AreEqual(604800, info.MaxSigLife);
            Assert.AreEqual(2, info.DsData.Count);
            Assert.AreEqual(0, info.KeyData.Count);
            Assert.AreEqual(12345, info.DsData[0].KeyTag);
            Assert.AreEqual(SecDNSAlgorithm.DSA, info.DsData[0].Algorithm);
            Assert.AreEqual(SecDNSDigestType.SHA1, info.DsData[0].DigestType);
            Assert.AreEqual("49FD46E6C4B45C55D4AC", info.DsData[0].Digest);
            Assert.AreEqual(257, info.DsData[0].KeyData.Flags);
            Assert.AreEqual("AQPJ////4Q==", info.DsData[0].KeyData.PublicKey);
            Assert.AreEqual(54321, info.DsData[1].KeyTag);
            Assert.AreEqual(SecDNSAlgorithm.ECDSAP256SHA256, info.DsData[1].Algorithm);
            Assert.AreEqual(SecDNSDigestType.SHA256, info.DsData[1].DigestType);
            Assert.IsNull(info.DsData[1].KeyData);
        }

        /// <summary>
        /// RFC 5910 section 5.1.2, info response with key data only.
        /// </summary>
        [TestMethod]
        [TestCategory("SecDNSExtension")]
        public void SecDNSInfDataKeyData()
        {
            var xml = string.Format(InfoResponseTemplate,
                "<extension><secDNS:infData xmlns:secDNS=\"urn:ietf:params:xml:ns:secDNS-1.1\">" +
                "<secDNS:keyData><secDNS:flags>257</secDNS:flags><secDNS:protocol>3</secDNS:protocol><secDNS:alg>1</secDNS:alg><secDNS:pubKey>AQPJ////4Q==</secDNS:pubKey></secDNS:keyData>" +
                "</secDNS:infData></extension>");

            var info = SecDNSInfData.FromResponse(new DomainInfoResponse(xml));

            Assert.IsNull(info.MaxSigLife);
            Assert.AreEqual(0, info.DsData.Count);
            Assert.AreEqual(1, info.KeyData.Count);
            Assert.AreEqual(3, info.KeyData[0].Protocol);
            Assert.AreEqual(SecDNSAlgorithm.RSAMD5, info.KeyData[0].Algorithm);
        }

        [TestMethod]
        [TestCategory("SecDNSExtension")]
        public void SecDNSInfDataAbsent()
        {
            var info = SecDNSInfData.FromResponse(new DomainInfoResponse(string.Format(InfoResponseTemplate, "")));

            Assert.IsNull(info);
        }
    }
}
