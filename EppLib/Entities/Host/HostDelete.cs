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
    public class HostDelete : HostBase<HostDeleteResponse>
    {
        public HostDelete(string hostname)
        {
            HostName = hostname;
        }

        protected override XmlElement BuildCommandElement(XmlDocument doc, XmlElement commandRootElement)
        {
            var hostInfo = BuildCommandElement(doc, "delete", commandRootElement);

            AddXmlElement(doc, hostInfo, "host:name", HostName, namespaceUri);

            return hostInfo;
        }

        protected string HostName { get; set; }

        public override HostDeleteResponse FromBytes(byte[] bytes)
        {
            return new HostDeleteResponse(bytes);
        }
    }
}
