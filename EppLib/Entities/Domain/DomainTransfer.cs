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
using System.Collections.Generic;
using System.Xml;

namespace EppLib.Entities
{
    public class DomainTransfer : DomainBase<DomainTransferResponse>
    {
        private string m_name;
        protected string registrantContactId;
        protected string adminContactId;
        protected IList<string> techContactIds;

        public DomainTransfer(string mName)
        {
            m_name = mName;
        }

        public DomainTransfer(string mName, string registrantContactId, string adminContactId, string techContactId)
        {
            m_name = mName;
            this.techContactIds = new List<string> { techContactId };
            this.adminContactId = adminContactId;
            this.registrantContactId = registrantContactId;
        }

        public DomainTransfer(string mName, string registrantContactId, string adminContactId, IList<string> techContactIds)
        {
            m_name = mName;
            this.adminContactId = adminContactId;
            this.techContactIds = techContactIds;
            this.registrantContactId = registrantContactId;
        }

        protected override XmlElement BuildCommandElement(XmlDocument doc, XmlElement commandRootElement)
        {
            var domainTransfer = BuildCommandElement(doc, "transfer", commandRootElement, "request");

            AddXmlElement(doc, domainTransfer, "domain:name", m_name, namespaceUri);

            return domainTransfer;
        }

        public override DomainTransferResponse FromBytes(byte[] bytes)
        {
            return new DomainTransferResponse(bytes);
        }
    }
}
