using System.Xml;

namespace EppLib.Extensions.SecDNS
{
    public enum SecDNSAlgorithm
    {
        RSAMD5 = 1,
        DH = 2,
        DSA = 3,
        ECC = 4,
        RSASHA1 = 5,
        INDIRECT = 252,
        PRIVATEDNS = 253,
        PRIVATEOID = 254
    }

    public class SecDNSData
    {
        public short KeyTag { get; set; }
        public SecDNSAlgorithm Algorithm { get; set; }
        public string Digest { get; set; }

        public XmlNode ToXml(XmlDocument doc)
        {
            var dataNode = doc.CreateElement("secDNS:dsData", "urn:ietf:params:xml:ns:secDNS-1.1");

            var keyTagNode = doc.CreateElement("secDNS:keyTag", "urn:ietf:params:xml:ns:secDNS-1.1");
            keyTagNode.InnerText = KeyTag.ToString();
            dataNode.AppendChild(keyTagNode);

            var algNode = doc.CreateElement("secDNS:alg", "urn:ietf:params:xml:ns:secDNS-1.1");
            algNode.InnerText = ((int)Algorithm).ToString();
            dataNode.AppendChild(algNode);

            var digestTypeNode = doc.CreateElement("secDNS:digestType", "urn:ietf:params:xml:ns:secDNS-1.1");
            digestTypeNode.InnerText = "1";
            dataNode.AppendChild(digestTypeNode);

            var digestNode = doc.CreateElement("secDNS:digest", "urn:ietf:params:xml:ns:secDNS-1.1");
            digestNode.InnerText = Digest;
            dataNode.AppendChild(digestNode);

            return dataNode;
        }
    }
}
