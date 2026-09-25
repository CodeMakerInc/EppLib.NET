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
using System.Globalization;
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

        /// <summary>
        /// The transfer operation; defaults to Request.
        /// </summary>
        public TransferOperation Operation { get; set; } = TransferOperation.Request;

        /// <summary>
        /// Years or months to add to the registration when the transfer completes. Only used with Request.
        /// </summary>
        public DomainPeriod Period { get; set; }

        /// <summary>
        /// The repository object ID of the contact whose authorization information is in Password,
        /// when it is a contact's rather than the domain's (the pw roid attribute).
        /// </summary>
        public string AuthInfoRoid { get; set; }

        protected override XmlElement BuildCommandElement(XmlDocument doc, XmlElement commandRootElement)
        {
            var domainTransfer = BuildCommandElement(doc, "transfer", commandRootElement, Operation.ToEppValue());

            AddXmlElement(doc, domainTransfer, "domain:name", m_name, namespaceUri);

            if (Period != null)
            {
                var period = AddXmlElement(doc, domainTransfer, "domain:period", Period.Value.ToString(CultureInfo.InvariantCulture), namespaceUri);
                period.SetAttribute("unit", Period.Unit);
            }

            if (!string.IsNullOrWhiteSpace(Password))
            {
                var authInfo = AddXmlElement(doc, domainTransfer, "domain:authInfo", null, namespaceUri);
                var pw = AddXmlElement(doc, authInfo, "domain:pw", Password, namespaceUri);

                if (AuthInfoRoid != null)
                {
                    pw.SetAttribute("roid", AuthInfoRoid);
                }
            }

            return domainTransfer;
        }

        public override DomainTransferResponse FromBytes(byte[] bytes)
        {
            return new DomainTransferResponse(bytes);
        }
    }
}
