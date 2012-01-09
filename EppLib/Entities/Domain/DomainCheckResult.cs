using System.Globalization;
using System.Xml;

namespace EppLib.Entities
{
    public class DomainCheckResult
    {
        public DomainCheckResult(XmlNode child, XmlNamespaceManager namespaces)
        {
            var nameNode = child.SelectSingleNode("domain:name", namespaces);

            if (nameNode != null)
            {

                Name = nameNode.InnerText;

                if (nameNode.Attributes != null)
                {
                    var xmlAttribute = nameNode.Attributes["avail"];

                    if (xmlAttribute != null) Available = xmlAttribute.Value.ToLower(CultureInfo.InvariantCulture).Equals("true");
                }
            }

            var reasonNode = child.SelectSingleNode("domain:reason", namespaces);

            if (reasonNode != null)
            {
                Reason = reasonNode.InnerText;
            }

        }

        public string Reason { get; set; }

        public bool Available { get; set; }

        public string Name { get; set; }
    }
}