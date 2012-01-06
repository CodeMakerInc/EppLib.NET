using System;
using System.Xml;

namespace EppLib.Entities
{
    public abstract class EppExtension 
    {
        private const string _namespace = "urn:ietf:params:xml:ns:cira-1.0";

        public abstract XmlNode ToXml(XmlDocument doc);

        protected static XmlElement CreateElement(XmlDocument doc, string qualifiedName)
        {
            return doc.CreateElement(qualifiedName, _namespace);
        }

        protected static XmlElement AddXmlElement(XmlDocument doc, XmlElement containingElement, String tagName, String value)
        {
            var xml_element = CreateElement(doc, tagName);

            if (!string.IsNullOrEmpty(value))
            {
                xml_element.AppendChild(doc.CreateTextNode(value));
            }
            containingElement.AppendChild(xml_element);
            return xml_element;
        }
    }
}