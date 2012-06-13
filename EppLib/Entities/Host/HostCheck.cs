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
    public class HostCheck : HostBase<HostCheckResponse>
    {
        private readonly IList<string> hosts;

        public HostCheck(string hostName)
        {
            hosts = new List<string> { hostName };
        }

        public HostCheck(IList<string> hosts)
        {
            this.hosts = hosts;
        }

        protected override XmlElement BuildCommandElement(XmlDocument doc, XmlElement commandRootElement)
        {
            var hostCheck = BuildCommandElement(doc, "check", commandRootElement);

            foreach (var host in hosts)
            {
                AddXmlElement(doc, hostCheck, "host:name", host, namespaceUri);
            }

            return hostCheck;
        }

        public override HostCheckResponse FromBytes(byte[] bytes)
        {
            return new HostCheckResponse(bytes);
        }
    }
}
