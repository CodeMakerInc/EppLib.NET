using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace EppLib.Entities
{
    public abstract class EppBase<T> where T: EppResponse
    {
        private const string _urnIetfParamsXmlNsEpp = "urn:ietf:params:xml:ns:epp-1.0";

        protected static XmlElement CreateDocRoot(XmlDocument doc)
        {
            return CreateDocRoot("epp", doc, "xmlns");
        }

        protected static void PrepareExtensionElement(XmlDocument doc, XmlElement command, IEnumerable<EppExtension> extensions)
        {

            if (extensions != null)
            {
                var extension_element = CreateElement(doc,"extension");

                foreach (var extension in extensions)
                {
                    var extension_node = doc.ImportNode(extension.ToXml(doc), true);

                    extension_element.AppendChild(extension_node);

                }

                command.AppendChild(extension_element);
            }
        }

        protected static XmlElement CreateElement(XmlDocument doc, string qualifiedName)
        {
            return doc.CreateElement(qualifiedName, _urnIetfParamsXmlNsEpp);
        }

        private static XmlElement CreateElement(XmlDocument doc, string qualifiedName, string namespaceURI)
        {
            return doc.CreateElement(qualifiedName, namespaceURI);
        }

        private static XmlElement CreateDocRoot(string rootString, XmlDocument doc, string prefix)
        {
            var schema = new XmlSchema();

            schema.Namespaces.Add(prefix, _urnIetfParamsXmlNsEpp);

            doc.Schemas.Add(schema);

            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", "no"));

            var root = doc.CreateElement(rootString, _urnIetfParamsXmlNsEpp);

            return root;
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

        protected static XmlElement AddXmlElement(XmlDocument doc, XmlElement containingElement, String tagName, String value, string namespaceUri)
        {
            var xml_element = CreateElement(doc, tagName, namespaceUri);

            if (!string.IsNullOrEmpty(value))
            {
                xml_element.AppendChild(doc.CreateTextNode(value));
            }
            containingElement.AppendChild(xml_element);
            return xml_element;
        }

        public abstract XmlDocument ToXml();
        public abstract T FromBytes(byte[] bytes);

       
    }
}
