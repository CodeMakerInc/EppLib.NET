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
    public class DomainReleaseResponse : EppResponse
    {
		public DomainReleaseResponse(byte[] bytes)
			: base(bytes)
        {
            
        }

        protected override void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            namespaces.AddNamespace("domain", "urn:ietf:params:xml:ns:domain-1.0");

            var children = doc.SelectSingleNode("/ns:epp/ns:response/ns:resData/domain:trnData", namespaces);

            if (children != null)
            {
                var hostNode = children.SelectSingleNode("domain:name", namespaces);

                if (hostNode != null)
                {
                    DomainTransferResult = new DomainTransferResult { DomainName = hostNode.InnerText };

                    var crDateNode = children.SelectSingleNode("domain:crDate", namespaces);

                    if (crDateNode != null)
                    {
                        DomainTransferResult.CreatedDate = crDateNode.InnerText;
                    }

                    var exDateNode = children.SelectSingleNode("domain:expDate", namespaces);

                    if (exDateNode != null)
                    {
                        DomainTransferResult.ExpirationDate = exDateNode.InnerText;
                    }
                }
            }
        }

        public DomainTransferResult DomainTransferResult
        {
            get;
            set;
        }
    }
}