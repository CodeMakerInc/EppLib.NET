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
    /// Response to a contact transfer command (RFC 5733 section 3.2.4).
    /// </summary>
    public class ContactTransferResponse : EppResponse
    {
        public ContactTransferResponse(byte[] bytes) : base(bytes) { }

        public ContactTransferResponse(string xml) : base(xml) { }

        public string ContactId { get; private set; }

        /// <summary>
        /// Transfer status: clientApproved, clientCancelled, clientRejected, pending, serverApproved or serverCancelled.
        /// </summary>
        public string TransferStatus { get; private set; }

        /// <summary>
        /// The client that requested the transfer (reID).
        /// </summary>
        public string RequestClientId { get; private set; }

        /// <summary>
        /// When the transfer was requested (reDate), as sent by the registry.
        /// </summary>
        public string RequestDate { get; private set; }

        /// <summary>
        /// The client that should act on the transfer (acID).
        /// </summary>
        public string ActionClientId { get; private set; }

        /// <summary>
        /// When action was completed or is due (acDate), as sent by the registry.
        /// </summary>
        public string ActionDate { get; private set; }

        protected override void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            namespaces.AddNamespace("contact", "urn:ietf:params:xml:ns:contact-1.0");

            var trnData = doc.SelectSingleNode("/ns:epp/ns:response/ns:resData/contact:trnData", namespaces);

            if (trnData != null)
            {
                ContactId = trnData.SelectSingleNode("contact:id", namespaces)?.InnerText;
                TransferStatus = trnData.SelectSingleNode("contact:trStatus", namespaces)?.InnerText;
                RequestClientId = trnData.SelectSingleNode("contact:reID", namespaces)?.InnerText;
                RequestDate = trnData.SelectSingleNode("contact:reDate", namespaces)?.InnerText;
                ActionClientId = trnData.SelectSingleNode("contact:acID", namespaces)?.InnerText;
                ActionDate = trnData.SelectSingleNode("contact:acDate", namespaces)?.InnerText;
            }
        }
    }
}
