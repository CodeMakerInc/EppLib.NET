using System.Xml;

namespace EppLib.Extensions.Smallregistry.ContactCreate
{
    public class SmallregistryContactPMCreateExtension : SmallregistryExtensionBase
    {
        public string CompanySerial { get; set; }
        public override XmlNode ToXml(XmlDocument doc)
        {
            var root = CreateElement(doc, "ext");

            var create = AddXmlElement(doc, root, "create", null);

            var contact = AddXmlElement(doc, create, "contact", null);

            var org = AddXmlElement(doc, contact, "org", null);

            if (CompanySerial != null)
            {
                AddXmlElement(doc, org, "companySerial", CompanySerial);
            }

            return root;
        }
    }
}
