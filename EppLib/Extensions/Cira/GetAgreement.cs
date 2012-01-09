using System.Collections.Generic;
using System.Xml;
using EppLib.Entities;

namespace EppLib.Extensions.Cira
{
    public class GetAgreement : EppCommand<GetAgreementResponse>
    {
        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();

            var commandRootElement = GetCommandRootElement(doc);

            var ciraAgreementExtension = new List<EppExtension>{new CiraAgreementExtension()};

            PrepareExtensionElement(doc, commandRootElement, ciraAgreementExtension);

            return doc;
        }
        
        public override GetAgreementResponse FromBytes(byte[] bytes)
        {
            return new GetAgreementResponse(bytes);
        }
    }
}
