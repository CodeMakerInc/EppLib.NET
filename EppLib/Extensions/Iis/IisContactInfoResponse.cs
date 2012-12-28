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
using EppLib.Entities;

namespace EppLib.Extensions.Iis
{
    public class IisContactInfoResponse : ContactInfoResponse
    {
        private IisContactExtension _iisContact = new IisContactExtension();
        public virtual new IisContactExtension Contact
        {
            get { return _iisContact; }
        }

        public IisContactInfoResponse(byte[] bytes) : base(bytes) { }

        protected override void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            base.ProcessDataNode(doc, namespaces);

            _iisContact.Id = _contact.Id;
            _iisContact.Voice = _contact.Voice;
            _iisContact.Fax = _contact.Fax;
            _iisContact.Email = _contact.Email;
            _iisContact.PostalInfo = _contact.PostalInfo;
            _iisContact.Roid = _contact.Roid;
            _iisContact.Status = _contact.Status;
            _iisContact.StatusList = _contact.StatusList;
            _iisContact.ClId = _contact.ClId;
            _iisContact.CrId = _contact.CrId;
            _iisContact.UpId = _contact.UpId;
            _iisContact.CrDate = _contact.CrDate;
            _iisContact.UpDate = _contact.UpDate;
            _iisContact.TrDate = _contact.TrDate;
        }

        protected override void ProcessExtensionNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            //namespaces.AddNamespace("contact", "urn:ietf:params:xml:ns:contact-1.0");
            namespaces.AddNamespace("iis", "urn:se:iis:xml:epp:iis-1.2");

            var children = doc.SelectSingleNode("/ns:epp/ns:response/ns:extension/iis:infData", namespaces);

            if (children != null)
            {
                XmlNode node;

                // OrganizationNumber
                node = children.SelectSingleNode("iis:orgno", namespaces);
                if (node != null)
                {
                    _iisContact.OrganizationNumber = node.InnerText;
                }

                // VatNumber
                node = children.SelectSingleNode("iis:vatno", namespaces);
                if (node != null)
                {
                    _iisContact.VatNumber = node.InnerText;
                }
            }
        }
    }
}