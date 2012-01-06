using System.Collections.Generic;
using System.Xml;

namespace EppLib.Entities
{
    public abstract class DomainBase<T> : EppCommand<T> where T : EppResponse
    {
        protected DomainBase()
            : base("domain", "urn:ietf:params:xml:ns:domain-1.0")
        {
        }

        protected XmlNode CreateNameServerElement(XmlDocument doc, IEnumerable<string> nameServers)
        {
            var nameServerElement = doc.CreateElement("domain:ns", namespaceUri);

            foreach (var serverName in nameServers)
            {
                AddXmlElement(doc, nameServerElement, "domain:hostObj", serverName,namespaceUri);
            }

            return nameServerElement;
        }

        
    }
}
