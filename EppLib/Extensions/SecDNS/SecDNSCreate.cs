using EppLib.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace EppLib.Extensions.SecDNS
{
    /// <summary>
    /// The secDNS create extension (RFC 5910 section 5.2.1). Add either DS records to <see cref="DsData"/>
    /// or DNSKEY records to <see cref="KeyData"/>, not both; which one depends on the registry.
    /// </summary>
    public class SecDNSCreate : EppExtension
    {
        public int? MaxSigLife { get; set; }
        public IList<SecDNSData> DsData { get; } = new List<SecDNSData>();
        public IList<SecDNSKeyData> KeyData { get; } = new List<SecDNSKeyData>();

        protected override string Namespace { get; set; }

        public override XmlNode ToXml(XmlDocument doc)
        {
            if (DsData.Any() && KeyData.Any())
            {
                throw new InvalidOperationException("secDNS create takes either DsData or KeyData, not both (RFC 5910 section 4).");
            }

            var root = doc.CreateElement("secDNS:create", SecDNSKeyData.NamespaceUri);
            root.SetAttribute("xmlns:secDNS", SecDNSKeyData.NamespaceUri);

            var xsd = doc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xsd.Value = "urn:ietf:params:xml:ns:secDNS-1.1 secDNS-1.1.xsd";
            root.Attributes.Append(xsd);

            if (MaxSigLife.HasValue)
            {
                var maxSigLifeNode = doc.CreateElement("secDNS:maxSigLife", SecDNSKeyData.NamespaceUri);
                maxSigLifeNode.InnerText = MaxSigLife.Value.ToString(CultureInfo.InvariantCulture);
                root.AppendChild(maxSigLifeNode);
            }

            foreach (var data in DsData)
            {
                root.AppendChild(data.ToXml(doc));
            }

            foreach (var key in KeyData)
            {
                root.AppendChild(key.ToXml(doc));
            }

            return root;
        }
    }
}
