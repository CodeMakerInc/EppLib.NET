using System.Xml;

namespace EppLib.Entities
{
    public class DomainTransferResponse : EppResponse
    {
        public DomainTransferResponse(byte[] bytes):base(bytes)
        {
            
        }

        protected override void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            namespaces.AddNamespace("domain", "urn:ietf:params:xml:ns:domain-1.0");

            var children = doc.SelectSingleNode("/ns:epp/ns:response/ns:resData/domain:trnData", namespaces);

            if (children != null)
            {
                var hostNode = children.SelectSingleNode("domain:name", namespaces);

                if (hostNode != null)
                {
                    DomainTransferResult = new DomainTransferResult { DomainName = hostNode.InnerText };

                    var crDateNode = children.SelectSingleNode("domain:crDate", namespaces);

                    if (crDateNode != null)
                    {
                        DomainTransferResult.CreatedDate = crDateNode.InnerText;
                    }

                    var exDateNode = children.SelectSingleNode("domain:expDate", namespaces);

                    if (exDateNode != null)
                    {
                        DomainTransferResult.ExpirationDate = exDateNode.InnerText;
                    }
                }
            }
        }

        public DomainTransferResult DomainTransferResult
        {
            get;
            set;
        }
    }
}