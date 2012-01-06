using System;
using System.Xml;
using System.Xml.Schema;

namespace EasyEPP.Entities
{
    public abstract class Base
    {
        private readonly string _urnIetfParamsXmlNsEpp;

        protected Base(string _urnIetfParamsXmlNsEpp)
        {
            this._urnIetfParamsXmlNsEpp = _urnIetfParamsXmlNsEpp;
        }

        protected XmlElement CreateElement(XmlDocument doc, string qualifiedName)
        {
            return doc.CreateElement(qualifiedName, _urnIetfParamsXmlNsEpp);
        }

        private static XmlElement CreateElement(XmlDocument doc, string qualifiedName, string namespaceURI)
        {
            return doc.CreateElement(qualifiedName, namespaceURI);
        }
        
        protected XmlElement CreateDocRoot(string rootString, XmlDocument doc, string prefix)
        {
            var schema = new XmlSchema();

            schema.Namespaces.Add(prefix, _urnIetfParamsXmlNsEpp);

            doc.Schemas.Add(schema);

            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", "no"));

            var root = doc.CreateElement(rootString, _urnIetfParamsXmlNsEpp);

            return root;
        }

        protected XmlElement AddXmlElement(XmlDocument doc, XmlElement containing_element, String tag_name, String value)
        {
            var xml_element = CreateElement(doc,tag_name);

            if (!string.IsNullOrEmpty(value))
            {
                xml_element.AppendChild(doc.CreateTextNode(value));
            }
            containing_element.AppendChild(xml_element);
            return xml_element;
        }

        //HERE
        protected static XmlElement AddXmlElement(XmlDocument doc, XmlElement containing_element, String tag_name, String value, string namespaceURI)
        {
            var xml_element = CreateElement(doc, tag_name,namespaceURI);

            if (!string.IsNullOrEmpty(value))
            {
                xml_element.AppendChild(doc.CreateTextNode(value));
            }
            containing_element.AppendChild(xml_element);
            return xml_element;
        }

        public abstract XmlDocument ToXml();
        
    }
}