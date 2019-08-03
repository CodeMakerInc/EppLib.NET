using System;
using System.Xml;

namespace EppLib.Extensions.Smallregistry
{
    public class SmallregistryContactPPCreateExtension : SmallregistryExtensionBase
    {
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public override XmlNode ToXml(XmlDocument doc)
        {
            var root = CreateElement(doc, "ext");

            var create = AddXmlElement(doc, root, "create", null);

            var contact = AddXmlElement(doc, create, "contact", null);

            var person = AddXmlElement(doc, contact, "person", null);

            if (BirthDate != null)
            {
                AddXmlElement(doc, person, "birthDate", BirthDate.ToString("yyyy-MM-dd"));
            }

            if (BirthPlace != null)
            {
                AddXmlElement(doc, person, "birthPlace", BirthPlace);
            }

            return root;
        }
    }
}
