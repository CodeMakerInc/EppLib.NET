using System.Xml;
using EppLib.Entities;

namespace EppLib.Extensions.Cira
{
    public class CiraAgreementExtension : EppExtension
    {
        private string language = "en";

        public override XmlNode ToXml(XmlDocument doc)
        {
            var root = CreateElement(doc, "cira:ciraInfo");

            AddXmlElement(doc, root, "cira:action", "get CIRA latest agreement");

            if (language != null)
            {
                AddXmlElement(doc, root, "cira:language", language);
            }

            return root;
        }
    }
}