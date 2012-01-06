using System.Xml;

namespace EppLib.Entities
{
    public class HostCreate : HostBase<HostCreateResponse>
    {
        public HostCreate(Host host)
        {
            Host = host;
        }

        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();

            var domainCreate = BuildCommandElement(doc, "create");

            AddXmlElement(doc, domainCreate, "host:name", Host.HostName, namespaceUri);

            foreach (var address in Host.Addresses)
            {
                var node = AddXmlElement(doc, domainCreate, "host:addr", address.IPAddress, namespaceUri);

                node.SetAttribute("ip", address.IPVersion);
            }
            
            return doc;
        }

        public Host Host { get; set; }

        public override HostCreateResponse FromBytes(byte[] bytes)
        {
            return new HostCreateResponse(bytes);
        }
    }
}
