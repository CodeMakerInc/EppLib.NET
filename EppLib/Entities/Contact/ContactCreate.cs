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
    public class ContactCreate : ContactBase<ContactCreateResponse>
    {
        protected readonly Contact contact;

        public ContactCreate(Contact contact)
        {
            this.contact = contact;
        }

        protected override XmlElement BuildCommandElement(XmlDocument doc, XmlElement commandRootElement)
        {
            if (contact.Id == null)
            {
                throw new EppException("missing contact id");
            }

            var contact_create = BuildCommandElement(doc, "create", commandRootElement);

            AddXmlElement(doc, contact_create, "contact:id", contact.Id, namespaceUri);

            var xml = AddressToXml(doc, "contact:postalInfo", contact.PostalInfo);

            contact_create.AppendChild(xml);

            if (contact.Voice != null)
            {
                var voice = AddXmlElement(doc, contact_create, "contact:voice", contact.Voice.Value, namespaceUri);

                if (contact.Voice.Extension != null)
                {
                    voice.SetAttribute("x", contact.Voice.Extension);
                }
            }

            if (contact.Fax != null)
            {
                var voice = AddXmlElement(doc, contact_create, "contact:fax", contact.Fax.Value, namespaceUri);
                if (contact.Fax.Extension != null)
                {
                    voice.SetAttribute("x", contact.Fax.Extension);
                }
            }

            if (contact.Email != null) 
            { 
                AddXmlElement(doc, contact_create, "contact:email", contact.Email, namespaceUri); 
            }

            if (Password!=null)
            {
                var authInfo = AddXmlElement(doc, contact_create, nspace + ":authInfo", null, namespaceUri);
                AddXmlElement(doc, authInfo, nspace + ":pw", Password, namespaceUri);

            }

            if (contact.DiscloseFlag != null)
            {
                var disclose = DiscloseToXml(doc, contact.DiscloseMask, (bool)contact.DiscloseFlag);
                contact_create.AppendChild(disclose);
            }

            return contact_create;
        }

        public override ContactCreateResponse FromBytes(byte[] bytes)
        {
            return new ContactCreateResponse(bytes);
        }
    }
}