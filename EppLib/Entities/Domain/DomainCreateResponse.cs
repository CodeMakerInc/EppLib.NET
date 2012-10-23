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
    public class DomainCreateResponse : EppResponse
    {
		public DomainCreateResponse(string xml) : base(xml) { }
        public DomainCreateResponse(byte[] bytes) : base(bytes) { }

        protected override void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            namespaces.AddNamespace("domain", "urn:ietf:params:xml:ns:domain-1.0");

            var children = doc.SelectSingleNode("//domain:creData", namespaces);

            if (children != null)
            {
                var hostNode = children.SelectSingleNode("domain:name", namespaces);

                if (hostNode != null)
                {
                    DomainCreateResult = new DomainCreateResult {DomainName = hostNode.InnerText};

                    var crDateNode = children.SelectSingleNode("domain:crDate", namespaces);

                    if (crDateNode != null)
                    {
                        DomainCreateResult.CreatedDate = crDateNode.InnerText;
                    }

					var exDateNode = children.SelectSingleNode("domain:exDate", namespaces);

                    if (exDateNode != null)
                    {
                        DomainCreateResult.ExpirationDate = exDateNode.InnerText;
                    }
                }
            }
        }

        public DomainCreateResult DomainCreateResult
        {
            get; set;
        }
    }
}
