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
using System.Xml;

namespace EppLib.Entities
{
    public class DomainInfo : DomainBase<DomainInfoResponse>
    {
        private readonly string domainName;

        /// <summary>
        /// "all" (default if missing) request delegated and subordinate hosts.
        /// "del" request delegeted hosts only.
        /// "sub" request subordinate hosts only.
        /// "none" request no information about hosts.
        /// </summary>
        public string Hosts { get; set; }

        public DomainInfo(string domainName)
        {
            this.domainName = domainName;
        }

        protected override XmlElement BuildCommandElement(XmlDocument doc, XmlElement commandRootElement)
        {
            var domainInfo = BuildCommandElement(doc, "info", commandRootElement);

            var domainNameElement = AddXmlElement(doc, domainInfo, "domain:name", domainName, namespaceUri);

            if (!String.IsNullOrEmpty(Hosts))
            {
                domainNameElement.SetAttribute("hosts", Hosts);
            }

            return domainInfo;
        }

        public override DomainInfoResponse FromBytes(byte[] bytes)
        {
            return new DomainInfoResponse(bytes);
        }
    }
}
