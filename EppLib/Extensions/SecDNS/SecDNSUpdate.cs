using EppLib.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace EppLib.Extensions.SecDNS
{
    public class SecDNSUpdate : EppExtension
    {
        public IList<SecDNSData> ToRemove { get; } = new List<SecDNSData>();
        public IList<SecDNSData> ToAdd { get; } = new List<SecDNSData>();

        protected override string Namespace { get; set; }

        public override XmlNode ToXml(XmlDocument doc)
        {
            var root = doc.CreateElement("secDNS:update", "urn:ietf:params:xml:ns:secDNS-1.1");
            root.SetAttribute("xmlns:secDNS", "urn:ietf:params:xml:ns:secDNS-1.1");

            var xsd = doc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xsd.Value = "urn:ietf:params:xml:ns:secDNS-1.1 secDNS-1.1.xsd";
            root.Attributes.Append(xsd);

            if (ToRemove.Any())
            {
                var removeNode = doc.CreateElement("secDNS:rem", "urn:ietf:params:xml:ns:secDNS-1.1");

                foreach (var data in ToRemove)
                {
                    removeNode.AppendChild(data.ToXml(doc));
                }

                root.AppendChild(removeNode);
            }

            if (ToAdd.Any())
            {
                var addNode = doc.CreateElement("secDNS:add", "urn:ietf:params:xml:ns:secDNS-1.1");

                foreach (var data in ToAdd)
                {
                    addNode.AppendChild(data.ToXml(doc));
                }

                root.AppendChild(addNode);
            }

            return root;
        }
    }
}
