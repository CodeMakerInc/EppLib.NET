using System.Globalization;
using System.Xml;

namespace EppLib.Extensions.SecDNS
{
    /// <summary>
    /// A DNSKEY record, the secDNS keyData element of RFC 5910. Used on its own by registries that
    /// compute the DS record themselves, or inside <see cref="SecDNSData"/>.
    /// </summary>
    public class SecDNSKeyData
    {
        internal const string NamespaceUri = "urn:ietf:params:xml:ns:secDNS-1.1";

        /// <summary>
        /// DNSKEY flags: 257 for a key signing key, 256 for a zone signing key.
        /// </summary>
        public int Flags { get; set; }

        /// <summary>
        /// DNSKEY protocol; always 3 (RFC 4034).
        /// </summary>
        public int Protocol { get; set; } = 3;

        public SecDNSAlgorithm Algorithm { get; set; }

        /// <summary>
        /// The public key, base64 encoded.
        /// </summary>
        public string PublicKey { get; set; }

        public XmlNode ToXml(XmlDocument doc)
        {
            var keyDataNode = doc.CreateElement("secDNS:keyData", NamespaceUri);

            var flagsNode = doc.CreateElement("secDNS:flags", NamespaceUri);
            flagsNode.InnerText = Flags.ToString(CultureInfo.InvariantCulture);
            keyDataNode.AppendChild(flagsNode);

            var protocolNode = doc.CreateElement("secDNS:protocol", NamespaceUri);
            protocolNode.InnerText = Protocol.ToString(CultureInfo.InvariantCulture);
            keyDataNode.AppendChild(protocolNode);

            var algNode = doc.CreateElement("secDNS:alg", NamespaceUri);
            algNode.InnerText = ((int)Algorithm).ToString(CultureInfo.InvariantCulture);
            keyDataNode.AppendChild(algNode);

            var pubKeyNode = doc.CreateElement("secDNS:pubKey", NamespaceUri);
            pubKeyNode.InnerText = PublicKey;
            keyDataNode.AppendChild(pubKeyNode);

            return keyDataNode;
        }

        internal static SecDNSKeyData FromXml(XmlNode node, XmlNamespaceManager namespaces)
        {
            return new SecDNSKeyData
            {
                Flags = int.Parse(node.SelectSingleNode("secDNS:flags", namespaces).InnerText, CultureInfo.InvariantCulture),
                Protocol = int.Parse(node.SelectSingleNode("secDNS:protocol", namespaces).InnerText, CultureInfo.InvariantCulture),
                Algorithm = (SecDNSAlgorithm)int.Parse(node.SelectSingleNode("secDNS:alg", namespaces).InnerText, CultureInfo.InvariantCulture),
                PublicKey = node.SelectSingleNode("secDNS:pubKey", namespaces).InnerText
            };
        }
    }
}
