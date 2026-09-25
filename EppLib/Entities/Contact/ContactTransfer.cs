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
    /// <summary>
    /// Contact transfer command (RFC 5733 section 3.2.4). Set Password to the contact's authorization
    /// information; registries require it for a request and may require it for a query.
    /// </summary>
    public class ContactTransfer : ContactBase<ContactTransferResponse>
    {
        private readonly string contactId;

        public ContactTransfer(string contactId)
        {
            this.contactId = contactId;
        }

        public TransferOperation Operation { get; set; } = TransferOperation.Request;

        protected override XmlElement BuildCommandElement(XmlDocument doc, XmlElement commandRootElement)
        {
            var contactTransfer = BuildCommandElement(doc, "transfer", commandRootElement, Operation.ToEppValue());

            AddXmlElement(doc, contactTransfer, "contact:id", contactId, namespaceUri);

            return contactTransfer;
        }

        public override ContactTransferResponse FromBytes(byte[] bytes)
        {
            return new ContactTransferResponse(bytes);
        }
    }
}
