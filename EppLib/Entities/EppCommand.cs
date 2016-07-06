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
using System.Xml;

namespace EppLib.Entities
{
    public abstract class EppCommand<T> : EppBase<T> where T : EppResponse
    {
        protected readonly string nspace;
        protected readonly string namespaceUri;
        protected readonly string schemaLocation;

        /// <summary>
        /// Length is 6 - 16, ascii chars
        /// </summary>
        public string Password { get; set; }
        public string TransactionId { get; set; }

        public readonly IList<EppExtension> Extensions = new List<EppExtension>();

        protected EppCommand(string nspace, string namespaceUri, string schemaLocation)
        {
            this.nspace = nspace;
            this.namespaceUri = namespaceUri;
            this.schemaLocation = schemaLocation;
        }

        protected EppCommand()
        {
        }

        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();
            var commandRootElement = GetCommandRootElement(doc);

            var cmdElement = BuildCommandElement(doc, commandRootElement);

            AppendAuthInfo(doc, cmdElement);

            AppendTRID(doc, commandRootElement);

            return doc;
        }

        private void AppendAuthInfo(XmlDocument doc, XmlElement cmdElement)
        {
            if (!String.IsNullOrWhiteSpace(Password))
            {
                // We may have had to insert the authInfo tag in this namespace already, in which case don't insert it again
                if (doc.GetElementsByTagName(nspace + ":authInfo").Count == 0)
                {
                    var authInfo = AddXmlElement(doc, cmdElement, nspace + ":authInfo", null, namespaceUri);
                    AddXmlElement(doc, authInfo, nspace + ":pw", Password, namespaceUri);
                }
            }
        }

        private void SetCommonAttributes(XmlElement command)
        {
            command.SetAttribute("xmlns:" + nspace, namespaceUri);
            var xsd = command.OwnerDocument.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xsd.Value = schemaLocation;
            command.Attributes.Append(xsd);
        }

        protected XmlElement BuildCommandElement(XmlDocument doc, string qualifiedName, XmlElement commandRootElement, string query = null)
        {
            var command = GetCommand(doc, qualifiedName, commandRootElement, query);

            if (Extensions != null)
            {
                PrepareExtensionElement(doc, commandRootElement, Extensions);
            }

            return command;
        }

        private XmlElement GetCommand(XmlDocument doc, string qualifiedName, XmlElement commandRootElement, string query = null)
        {
            var elem = CreateElement(doc, qualifiedName);

            if (query != null)
            {
                elem.SetAttribute("op", query);
            }

            var command = CreateElement(doc, nspace + ":" + qualifiedName);

            SetCommonAttributes(command);

            elem.AppendChild(command);

            commandRootElement.AppendChild(elem);

            return command;
        }

        protected static XmlElement GetCommandRootElement(XmlDocument doc)
        {
            var root = CreateDocRoot(doc);

            doc.AppendChild(root);

            var command = CreateElement(doc, "command");

            root.AppendChild(command);

            return command;
        }

        protected void AppendTRID(XmlDocument doc, XmlNode command)
        {
            if (String.IsNullOrWhiteSpace(TransactionId)) return;

            var clTRIDElement = CreateElement(doc, "clTRID");

            clTRIDElement.InnerText = TransactionId;

            command.AppendChild(clTRIDElement);
        }

        protected abstract XmlElement BuildCommandElement(XmlDocument doc, XmlElement commandRootElement);
    }
}
