// Copyright 2012 Code Maker Inc. (http://codemaker.net)
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (extensions != null && extensions.Count()>0)
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

            var xsd = doc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xsd.Value = "urn:ietf:params:xml:ns:epp-1.0 epp-1.0.xsd";
            root.Attributes.Append(xsd);

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
