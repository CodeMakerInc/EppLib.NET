using System.Collections.Generic;
using System.Xml;

namespace EppLib.Entities
{
    public class HostCheck : HostBase<HostCheckResponse>
    {
        private readonly IList<string> hosts;

        public HostCheck(string hostName)
        {
            hosts = new List<string> { hostName };
        }

        public HostCheck(IList<string> hosts)
        {
            this.hosts = hosts;
        }

        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();

            var hostInfo = BuildCommandElement(doc, "check");

            foreach (var host in hosts)
            {
                AddXmlElement(doc, hostInfo, "host:name", host, namespaceUri);
            }

            return doc;
        }
        
        public override HostCheckResponse FromBytes(byte[] bytes)
        {
            return new HostCheckResponse(bytes);
        }
    }
}
