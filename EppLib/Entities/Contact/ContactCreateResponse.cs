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
using System.Xml;

namespace EppLib.Entities
{
    public class ContactCreateResponse : EppResponse
    {
        public string ContactId { get; set; }
        public string DateCreated { get; set; }

        public ContactCreateResponse(byte[] bytes) : base(bytes) { }

        protected override void ProcessDataNode(System.Xml.XmlDocument doc, System.Xml.XmlNamespaceManager namespaces)
        {
            namespaces.AddNamespace("contact", "urn:ietf:params:xml:ns:contact-1.0");

            var children = doc.SelectSingleNode("//contact:creData", namespaces);

            if (children != null)
            {
                XmlNode node;

                // ContactId
                node = children.SelectSingleNode("contact:id", namespaces);
                if (node != null)
                {
                    ContactId = node.InnerText;
                }

                // DateCreated
                node = children.SelectSingleNode("contact:crDate", namespaces);
                if (node != null)
                {
                    DateCreated = node.InnerText;
                }
            }
        }
    }
}