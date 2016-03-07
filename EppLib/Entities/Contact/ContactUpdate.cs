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
    public class ContactUpdate : ContactBase<ContactUpdateResponse>
    {
        public string ContactId;

        public ContactChange ContactChange;

        public EppContactUpdateAddRemove ToAdd;
        public EppContactUpdateAddRemove ToRemove;
      

        public ContactUpdate(string contactId)
        {
            ContactId = contactId;
        }

        protected override XmlElement BuildCommandElement(XmlDocument doc, XmlElement commandRootElement)
        {
            if (ContactId == null)
            {
                throw new EppException("missing contact id");
            }

            var contact_update = BuildCommandElement(doc, "update", commandRootElement);

            AddXmlElement(doc, contact_update, "contact:id", ContactId, namespaceUri);

            var add_element = getAddRemoveElement(doc, ToAdd);

            if (add_element != null)
            {
                contact_update.AppendChild(add_element);
            }

            var remove_element = getAddRemoveElement(doc, ToRemove);

            if (remove_element != null)
            {
                contact_update.AppendChild(remove_element);
            }

            if (ContactChange != null)
            {
                var change_element = doc.CreateElement("contact:chg", namespaceUri);

                if (ContactChange.PostalInfo != null)
                {
                    var xml = AddressToXml(doc, "contact:postalInfo", ContactChange.PostalInfo);
                    change_element.AppendChild(xml);
                }

                if (ContactChange.Voice != null)
                {
                    var voice = AddXmlElement(doc, change_element, "contact:voice", ContactChange.Voice.Value, namespaceUri);

                    if (!String.IsNullOrEmpty(ContactChange.Voice.Extension))
                    {
                        voice.SetAttribute("x", ContactChange.Voice.Extension);
                    }
                }

                if (ContactChange.Fax != null)
                {
                    var voice = AddXmlElement(doc, change_element, "contact:fax", ContactChange.Fax.Value, namespaceUri);

                    if (!String.IsNullOrEmpty(ContactChange.Fax.Extension))
                    {
                        voice.SetAttribute("x", ContactChange.Fax.Extension);
                    }
                }

                if (ContactChange.DiscloseFlag != null)
                {
                    var disclose = DiscloseToXml(doc, ContactChange.DiscloseMask, (bool)ContactChange.DiscloseFlag);
                    change_element.AppendChild(disclose);
                }
                    
                if (ContactChange.Email != null) 
                { 
                    AddXmlElement(doc, change_element, "contact:email", ContactChange.Email, namespaceUri); 
                }

                contact_update.AppendChild(change_element);
            }

            return contact_update;
        }

        private static XmlElement getAddRemoveElement(XmlDocument doc, EppContactUpdateAddRemove addRemoveItems)
        {
            XmlElement add_remove_element = null;

            if (addRemoveItems != null)
            {
                if (addRemoveItems.Status != null &&
                     addRemoveItems.Status.Count > 0)
                {
                    foreach (var status in addRemoveItems.Status)
                    {

                        var status_element = AddXmlElement(doc, add_remove_element, "contact:status", status.Value);

                        status_element.SetAttribute("s", status.Type);

                        if (status.Lang != null)
                        {
                            status_element.SetAttribute("lang", status.Lang);
                        }
                    }
                }

            }


            return add_remove_element;
        }

        public override ContactUpdateResponse FromBytes(byte[] bytes)
        {
            return new ContactUpdateResponse(bytes);
        }
    }
}
