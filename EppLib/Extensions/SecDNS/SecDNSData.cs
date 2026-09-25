using System.Globalization;
using System.Xml;

namespace EppLib.Extensions.SecDNS
{
    /// <summary>
    /// DNSSEC algorithm numbers (IANA "DNS Security Algorithm Numbers"). Values not listed can be cast from int.
    /// </summary>
    public enum SecDNSAlgorithm
    {
        RSAMD5 = 1,
        DH = 2,
        DSA = 3,
        ECC = 4,
        RSASHA1 = 5,
        DSANSEC3SHA1 = 6,
        RSASHA1NSEC3SHA1 = 7,
        RSASHA256 = 8,
        RSASHA512 = 10,
        ECCGOST = 12,
        ECDSAP256SHA256 = 13,
        ECDSAP384SHA384 = 14,
        ED25519 = 15,
        ED448 = 16,
        INDIRECT = 252,
        PRIVATEDNS = 253,
        PRIVATEOID = 254
    }

    /// <summary>
    /// DS record digest types (IANA "Delegation Signer (DS) Resource Record (RR) Type Digest Algorithms").
    /// </summary>
    public enum SecDNSDigestType
    {
        SHA1 = 1,
        SHA256 = 2,
        GOSTR341194 = 3,
        SHA384 = 4
    }

    /// <summary>
    /// A DS record, the secDNS dsData element of RFC 5910.
    /// </summary>
    public class SecDNSData
    {
        /// <summary>
        /// The key tag, 0 to 65535.
        /// </summary>
        public int KeyTag { get; set; }
        public SecDNSAlgorithm Algorithm { get; set; }

        /// <summary>
        /// Defaults to SHA1 for compatibility with earlier versions; most registries expect SHA256 today.
        /// </summary>
        public SecDNSDigestType DigestType { get; set; } = SecDNSDigestType.SHA1;

        /// <summary>
        /// The digest, as hexadecimal.
        /// </summary>
        public string Digest { get; set; }

        /// <summary>
        /// The DNSKEY the DS record was made from. Optional; some registries require it.
        /// </summary>
        public SecDNSKeyData KeyData { get; set; }

        public XmlNode ToXml(XmlDocument doc)
        {
            var dataNode = doc.CreateElement("secDNS:dsData", SecDNSKeyData.NamespaceUri);

            var keyTagNode = doc.CreateElement("secDNS:keyTag", SecDNSKeyData.NamespaceUri);
            keyTagNode.InnerText = KeyTag.ToString(CultureInfo.InvariantCulture);
            dataNode.AppendChild(keyTagNode);

            var algNode = doc.CreateElement("secDNS:alg", SecDNSKeyData.NamespaceUri);
            algNode.InnerText = ((int)Algorithm).ToString(CultureInfo.InvariantCulture);
            dataNode.AppendChild(algNode);

            var digestTypeNode = doc.CreateElement("secDNS:digestType", SecDNSKeyData.NamespaceUri);
            digestTypeNode.InnerText = ((int)DigestType).ToString(CultureInfo.InvariantCulture);
            dataNode.AppendChild(digestTypeNode);

            var digestNode = doc.CreateElement("secDNS:digest", SecDNSKeyData.NamespaceUri);
            digestNode.InnerText = Digest;
            dataNode.AppendChild(digestNode);

            if (KeyData != null)
            {
                dataNode.AppendChild(KeyData.ToXml(doc));
            }

            return dataNode;
        }

        internal static SecDNSData FromXml(XmlNode node, XmlNamespaceManager namespaces)
        {
            var keyDataNode = node.SelectSingleNode("secDNS:keyData", namespaces);

            return new SecDNSData
            {
                KeyTag = int.Parse(node.SelectSingleNode("secDNS:keyTag", namespaces).InnerText, CultureInfo.InvariantCulture),
                Algorithm = (SecDNSAlgorithm)int.Parse(node.SelectSingleNode("secDNS:alg", namespaces).InnerText, CultureInfo.InvariantCulture),
                DigestType = (SecDNSDigestType)int.Parse(node.SelectSingleNode("secDNS:digestType", namespaces).InnerText, CultureInfo.InvariantCulture),
                Digest = node.SelectSingleNode("secDNS:digest", namespaces).InnerText,
                KeyData = keyDataNode != null ? SecDNSKeyData.FromXml(keyDataNode, namespaces) : null
            };
        }
    }
}
