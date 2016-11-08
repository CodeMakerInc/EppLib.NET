using EppLib.Entities;
using System.Collections.Generic;
using System.Xml;

namespace EppLib.Extensions.LaunchPhase.ClaimCheck
{
    public class ClaimCheckResponse : EppResponse
    {
        public IList<ClaimCheckResult> Results { get; } = new List<ClaimCheckResult>();

        public ClaimCheckResponse(byte[] bytes) : base(bytes)
        { }

        protected override void ProcessExtensionNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            namespaces.AddNamespace("launch", "urn:ietf:params:xml:ns:launch-1.0");

            var nodes = doc.SelectNodes("/ns:epp/ns:response/ns:extension/launch:chkData/launch:cd", namespaces);

            foreach (XmlNode node in nodes)
            {
                Results.Add(new ClaimCheckResult(node, namespaces));
            }
        }
    }
}
