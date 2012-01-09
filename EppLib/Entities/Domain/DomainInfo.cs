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

        public DomainInfo(string domainName)
        {
            this.domainName = domainName;
        }

        public string Hosts { get; set; }

        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();

            var domainInfo = BuildCommandElement(doc, "info");

            var domainNameElement = AddXmlElement(doc, domainInfo, "domain:name", domainName, namespaceUri);

            if (!String.IsNullOrEmpty(Hosts))
            {
                domainNameElement.SetAttribute("hosts", Hosts);
            }

            return doc;
        }

        public override DomainInfoResponse FromBytes(byte[] bytes)
        {
            return new DomainInfoResponse(bytes);
        }
    }
}
