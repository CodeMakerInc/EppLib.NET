using System;
using System.Xml;

namespace EppLib.Entities
{
    public class DomainDelete : DomainBase<DomainDeleteResponse>
    {
        private readonly string DomainName;

        public DomainDelete(string domainName)
        {
            DomainName = domainName;
        }

        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();

            var domainCreate = BuildCommandElement(doc, "delete");

            AddXmlElement(doc, domainCreate, "domain:name", DomainName, namespaceUri);

            return doc;
        }

        public override DomainDeleteResponse FromBytes(byte[] bytes)
        {
            return new DomainDeleteResponse(bytes);
        }
    }
}
