using System.Collections.Generic;
using System.Xml;

namespace EppLib.Extensions.LaunchPhase.ClaimCreate
{
    public class ClaimCreate : Entities.DomainCreate
    {
        public IList<ClaimNotice> Notices { get; } = new List<ClaimNotice>();

        public ClaimCreate(string domainName, string registrantContactId) : base(domainName, registrantContactId)
        {
        }

        public override XmlDocument ToXml()
        {
            Extensions.Clear();
            Extensions.Add(new ClaimCreateExtension(Notices));
            return base.ToXml();
        }
    }
}
