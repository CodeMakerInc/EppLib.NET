using EppLib.Entities;
using System.Collections.Generic;
using System.Xml;

namespace EppLib.Extensions.LaunchPhase.ClaimCheck
{
    public class ClaimCheck : DomainBase<ClaimCheckResponse>
    {
        private readonly IList<string> domains;

        public ClaimCheck(string domain)
        {
            domains = new List<string> { domain };
        }

        public ClaimCheck(IList<string> domains)
        {
            this.domains = domains;
        }

        public override ClaimCheckResponse FromBytes(byte[] bytes)
        {
            return new ClaimCheckResponse(bytes);
        }

        public override XmlDocument ToXml()
        {
            Extensions.Clear();
            Extensions.Add(new ClaimCheckExtension());
            return base.ToXml();
        }

        protected override XmlElement BuildCommandElement(XmlDocument doc, XmlElement commandRootElement)
        {
            var claimCheck = BuildCommandElement(doc, "check", commandRootElement);

            foreach (var domain in domains)
            {
                AddXmlElement(doc, claimCheck, "domain:name", domain, namespaceUri);
            }

            return claimCheck;
        }
    }
}
