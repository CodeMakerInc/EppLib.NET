using System;
using System.Xml;

namespace EppLib.Entities
{
    public class DomainInfo : DomainBase<DomainInfoResponse>
    {
        private readonly string domainName;

        public DomainInfo(string domainName)
        {
            this.domainName = domainName;
        }

        public string Hosts { get; set; }

        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();

            var domainInfo = BuildCommandElement(doc, "info");

            var domainNameElement = AddXmlElement(doc, domainInfo, "domain:name", domainName, namespaceUri);

            if (!String.IsNullOrEmpty(Hosts))
            {
                domainNameElement.SetAttribute("hosts", Hosts);
            }

            return doc;
        }

        public override DomainInfoResponse FromBytes(byte[] bytes)
        {
            return new DomainInfoResponse(bytes);
        }
    }
}
