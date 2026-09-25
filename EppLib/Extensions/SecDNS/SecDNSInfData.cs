using EppLib.Entities;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace EppLib.Extensions.SecDNS
{
    /// <summary>
    /// The DNSSEC data returned with a domain info response (RFC 5910 section 5.1.2).
    /// </summary>
    public class SecDNSInfData
    {
        public int? MaxSigLife { get; set; }
        public IList<SecDNSData> DsData { get; } = new List<SecDNSData>();
        public IList<SecDNSKeyData> KeyData { get; } = new List<SecDNSKeyData>();

        /// <summary>
        /// Reads the secDNS infData from a domain info response, including registry-specific info responses.
        /// Returns null when the response has no DNSSEC data.
        /// </summary>
        public static SecDNSInfData FromResponse(EppResponse response)
        {
            var doc = new XmlDocument();
            doc.LoadXml(response.Xml);

            var namespaces = new XmlNamespaceManager(doc.NameTable);
            namespaces.AddNamespace("ns", "urn:ietf:params:xml:ns:epp-1.0");
            namespaces.AddNamespace("secDNS", SecDNSKeyData.NamespaceUri);

            var infData = doc.SelectSingleNode("/ns:epp/ns:response/ns:extension/secDNS:infData", namespaces);
            if (infData == null)
            {
                return null;
            }

            var result = new SecDNSInfData();

            var maxSigLifeNode = infData.SelectSingleNode("secDNS:maxSigLife", namespaces);
            if (maxSigLifeNode != null)
            {
                result.MaxSigLife = int.Parse(maxSigLifeNode.InnerText, CultureInfo.InvariantCulture);
            }

            foreach (XmlNode node in infData.SelectNodes("secDNS:dsData", namespaces))
            {
                result.DsData.Add(SecDNSData.FromXml(node, namespaces));
            }

            foreach (XmlNode node in infData.SelectNodes("secDNS:keyData", namespaces))
            {
                result.KeyData.Add(SecDNSKeyData.FromXml(node, namespaces));
            }

            return result;
        }
    }
}
