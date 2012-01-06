using System.Xml;

namespace EppLib.Entities
{
    public class HostDelete : HostBase<HostDeleteResponse>
    {
        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();

            var hostInfo = BuildCommandElement(doc, "delete");

            AddXmlElement(doc, hostInfo, "host:name", HostName, namespaceUri);

            return doc;
        }

        protected string HostName { get; set; }

        public override HostDeleteResponse FromBytes(byte[] bytes)
        {
            return new HostDeleteResponse(bytes);
        }
    }
}
