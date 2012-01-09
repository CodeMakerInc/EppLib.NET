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
    public class HostCreateResponse : EppResponse
    {
        public HostCreateResponse(byte[] bytes)
            : base(bytes)
        {
        }

        protected override void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            namespaces.AddNamespace("host", "urn:ietf:params:xml:ns:host-1.0");

            var children = doc.SelectSingleNode("/ns:epp/ns:response/ns:resData/host:creData", namespaces);

            if (children != null)
            {
                var hostNode = children.SelectSingleNode("host:name", namespaces);

                if(hostNode!=null)
                {
                    HostCreateResult = new HostCreateResult {HostName = hostNode.InnerText};

                    var crDateNode = children.SelectSingleNode("host:crDate", namespaces);

                    if(crDateNode!=null)
                    {
                        HostCreateResult.CreatedDate = crDateNode.InnerText;
                    }
                }
            }
        }

        protected HostCreateResult HostCreateResult { get; set; }
    }
}
