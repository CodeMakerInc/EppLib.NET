using System.Collections.Generic;
using System.Xml;

namespace EppLib.Entities
{
    public class HostCheckResponse: EppResponse
    {
        private readonly List<HostCheckResult> results = new List<HostCheckResult>();

        public HostCheckResponse(byte[] bytes)
            : base(bytes)
        {
        }

        public IList<HostCheckResult> Results
        {
            get { return results; }
        }

        protected override void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            namespaces.AddNamespace("host", "urn:ietf:params:xml:ns:host-1.0");

            var children = doc.SelectNodes("/ns:epp/ns:response/ns:resData/host:chkData/host:cd", namespaces);

            if (children != null)
            {
                foreach (XmlNode child in children)
                {
                    results.Add(new HostCheckResult(child,namespaces));
                }
            }
        }
    }
}
