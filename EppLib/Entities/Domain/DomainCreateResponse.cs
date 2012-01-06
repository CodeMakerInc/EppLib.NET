using System.Xml;

namespace EppLib.Entities
{
    public class DomainCreateResponse : EppResponse
    {
        public DomainCreateResponse(byte[] bytes) : base(bytes)
        {
        }

        protected override void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            namespaces.AddNamespace("domain", "urn:ietf:params:xml:ns:domain-1.0");

            var children = doc.SelectSingleNode("/ns:epp/ns:response/ns:resData/domain:creData", namespaces);

            if (children != null)
            {
                var hostNode = children.SelectSingleNode("domain:name", namespaces);

                if (hostNode != null)
                {
                    DomainCreateResult = new DomainCreateResult {DomainName = hostNode.InnerText};

                    var crDateNode = children.SelectSingleNode("domain:crDate", namespaces);

                    if (crDateNode != null)
                    {
                        DomainCreateResult.CreatedDate = crDateNode.InnerText;
                    }

                    var exDateNode = children.SelectSingleNode("domain:expDate", namespaces);

                    if (exDateNode != null)
                    {
                        DomainCreateResult.ExpirationDate = exDateNode.InnerText;
                    }
                }
            }
        }

        public DomainCreateResult DomainCreateResult
        {
            get; set;
        }
    }
}
