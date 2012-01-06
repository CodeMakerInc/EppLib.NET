using System.Globalization;
using System.Xml;

namespace EppLib.Entities
{
    public class HostCheckResult
    {
        public HostCheckResult(XmlNode child, XmlNamespaceManager namespaces)
        {
            var nameNode = child.SelectSingleNode("host:name", namespaces);

            if (nameNode != null)
            {

                Name = nameNode.InnerText;

                if (nameNode.Attributes != null)
                {
                    var xmlAttribute = nameNode.Attributes["avail"];

                    if (xmlAttribute != null) Available = xmlAttribute.Value.ToLower(CultureInfo.InvariantCulture).Equals("true");
                }
            }

            var reasonNode = child.SelectSingleNode("host:reason", namespaces);

            if (reasonNode != null)
            {
                Reason = reasonNode.InnerText;
            }

        }

        protected string Reason { get; set; }

        protected bool Available { get; set; }

        protected string Name { get; set; }
    }
}