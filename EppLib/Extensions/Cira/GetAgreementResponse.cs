using System.Xml;
using EppLib.Entities;

namespace EppLib.Extensions.Cira
{
    public class GetAgreementResponse : EppResponse
    {
        public string Language;
        public string AgreementVersion;
        public string Agreement;

        public GetAgreementResponse(byte[] bytes) : base(bytes)
        {
            
        }

        protected override void ProcessExtensionNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            namespaces.AddNamespace("cira", "urn:ietf:params:xml:ns:cira-1.0");

            var children = doc.SelectSingleNode("/ns:epp/ns:response/ns:extension/cira:ciraInfo", namespaces);

            if (children != null)
            {
                var crLanguage = children.SelectSingleNode("cira:language", namespaces);

                if (crLanguage != null)
                {
                    Language = crLanguage.InnerText;
                }

                var crCiraAgreementVersion = children.SelectSingleNode("cira:ciraAgreementVersion", namespaces);

                if (crCiraAgreementVersion != null)
                {
                    AgreementVersion = crCiraAgreementVersion.InnerText;
                }

                var crAgreement = children.SelectSingleNode("cira:ciraAgreement", namespaces);

                if (crAgreement != null)
                {
                    Agreement = crAgreement.InnerText;
                }

                
            }
        }
    }
}