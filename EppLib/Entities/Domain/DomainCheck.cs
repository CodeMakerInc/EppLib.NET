using System;
using System.Collections.Generic;
using System.Xml;

namespace EppLib.Entities
{
    public class DomainCheck : DomainBase<DomainCheckResponse>
    {
        private readonly IList<string> domains;

        public DomainCheck(string domain)
        {
            domains = new List<string>{domain};
        }

        public DomainCheck(IList<string> domains)
        {
            this.domains = domains;
        }

        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();

            var domainCheck = BuildCommandElement(doc, "check");

            foreach (var domain in domains)
            {
                AddXmlElement(doc, domainCheck, "domain:name", domain, namespaceUri);    
            }
            
            return doc;
        }

        public override DomainCheckResponse FromBytes(byte[] bytes)
        {
            return new DomainCheckResponse(bytes);
        }
    }
}
