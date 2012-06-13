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
        private readonly string nspace;
        protected readonly string namespaceUri;

        /// <summary>
        /// Lenght is 6 - 16, ascii chars
        /// </summary>
        public string Password;

        public readonly IList<EppExtension> Extensions = new List<EppExtension>();
        private static readonly string ClTrid = Guid.NewGuid().ToString();

        protected EppCommand(string nspace, string namespaceUri)
        {
            this.nspace = nspace;
            this.namespaceUri = namespaceUri;
        }

        protected EppCommand()
        {
        }

        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();
            var commandRootElement = GetCommandRootElement(doc);

            BuildCommandElement(doc, commandRootElement);

            AppendTRID(doc, commandRootElement);

            return doc;
        }

        private void SetCommonAttributes(XmlElement command)
        {
            command.SetAttribute("xmlns:" + nspace, namespaceUri);
        }

        protected XmlElement BuildCommandElement(XmlDocument doc, string qualifiedName, XmlElement commandRootElement, string query = null)
        {
            var command = GetCommand(doc, qualifiedName, commandRootElement, query);

            if (Extensions != null)
            {
                PrepareExtensionElement(doc, commandRootElement, Extensions);
            }

            if (!String.IsNullOrWhiteSpace(Password))
            {
                var authInfo = AddXmlElement(doc, command, "domain:authInfo", null, namespaceUri);

                AddXmlElement(doc, authInfo, "domain:pw", Password, namespaceUri);
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

        protected static void AppendTRID(XmlDocument doc, XmlNode command)
        {
            var clTRIDElement = CreateElement(doc, "clTRID");

            clTRIDElement.InnerText = ClTrid;

            command.AppendChild(clTRIDElement);
        }

        protected abstract XmlElement BuildCommandElement(XmlDocument doc, XmlElement commandRootElement);
    }
}
