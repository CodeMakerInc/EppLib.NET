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
    public class DomainCheck : DomainBase<DomainCheckResponse>
    {
        private readonly IList<string> domains;

        public DomainCheck(string domain)
        {
            domains = new List<string>{domain};
        }

        public DomainCheck(IList<string> domains)
        {
            this.domains = domains;
        }

        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();

            var domainCheck = BuildCommandElement(doc, "check");

            foreach (var domain in domains)
            {
                AddXmlElement(doc, domainCheck, "domain:name", domain, namespaceUri);    
            }
            
            return doc;
        }

        public override DomainCheckResponse FromBytes(byte[] bytes)
        {
            return new DomainCheckResponse(bytes);
        }
    }
}
