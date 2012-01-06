using System.Xml;

namespace EppLib.Entities
{
    public class HostInfo : HostBase<HostInfoResponse>
    {
        private readonly string hostName;

        public HostInfo(string hostName)
        {
            this.hostName = hostName;
        }

        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();

            var hostInfo = BuildCommandElement(doc, "info");

            AddXmlElement(doc, hostInfo, "host:name", hostName, namespaceUri);
            
            return doc;
        }
        
        public override HostInfoResponse FromBytes(byte[] bytes)
        {
            return new HostInfoResponse(bytes);
        }
    }
}
