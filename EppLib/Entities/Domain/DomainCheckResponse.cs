using System.Collections.Generic;
using System.Xml;

namespace EppLib.Entities
{
    public class DomainCheckResponse : EppResponse
    {
        private readonly List<DomainCheckResult> results =  new List<DomainCheckResult>();

        public DomainCheckResponse(byte[] bytes)
            : base(bytes)
        {


        }

        public IList<DomainCheckResult> Results
        {
            get { return results; }
        }

        protected override void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            namespaces.AddNamespace("domain", "urn:ietf:params:xml:ns:domain-1.0");

            var children = doc.SelectNodes("/ns:epp/ns:response/ns:resData/domain:chkData/domain:cd", namespaces);

            if (children != null)
            {
                foreach (XmlNode child in children)
                {
                    results.Add(new DomainCheckResult(child,namespaces));
                }
            }
        }
    }
}
