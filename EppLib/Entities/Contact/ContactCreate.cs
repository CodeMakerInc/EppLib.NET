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
using EppLib.Extensions.Cira;

namespace EppLib.Entities
{
    public class ContactCreate : ContactBase<ContactCreateResponse>
    {
        private readonly Contact contact;

        public ContactCreate(Contact contact)
        {
            this.contact = contact;
        }
        
        public override XmlDocument ToXml()
        {
            if (contact.Id == null)
            {
                throw new EppException("missing contact id");
            }

            var doc = new XmlDocument();

            var contact_create = BuildCommandElement(doc, "create");

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

            if (contact.Email != null) { AddXmlElement(doc, contact_create, "contact:email", contact.Email, namespaceUri); }


            return doc;
        }

        
        /*public static ContactCreate Make(string contactId, string FullName, string CompanyName, string mCity, string streetAddress, string province, string postalCode, string countryCode, string telephone, string extension, string fax, string cprCategory, string mEmail, string language, string aggreementValue, string originatingIpAddress, string createdByResellerId)
        {
            var postalAddress = new PostalAddress
            {
                City = mCity,
                Street1 = streetAddress,
                StateProvince = province,
                PostalCode = postalCode,
                CountryCode = countryCode
            };

            var postalInfo = new PostalInfo
            {
                m_name = FullName,
                m_org = CompanyName,
                m_type = PostalAddressType.LOC,
                m_address = postalAddress
            };

            var mVoice = new Telephone { Value = telephone, Extension = extension };

            var mFax = new Telephone { Value = fax };

            var contact = new Contact { Id = contactId, PostalInfo = postalInfo, Voice = mVoice, Fax = mFax, Email = mEmail };

            return MakeContact(contact, cprCategory, "2.0", aggreementValue, originatingIpAddress, language, createdByResellerId);
        }*/

        /*public static ContactCreate MakeContact(Contact contact, string cprCategory, string ciraAgreementVersion, string aggreementValue, string originatingIpAddress, string language, string createdByResellerId)
        {
            var createContact = new ContactCreate(contact);

            var ciraExtension = new CiraCreateExtension
                                    {
                                        Language = language,
                                        OriginatingIpAddress = originatingIpAddress,
                                        CprCategory = cprCategory,
                                        CiraAgreementVersion = ciraAgreementVersion,
                                        AggreementValue = aggreementValue,
                                        CreatedByResellerId = createdByResellerId
                                    };

            createContact.Extensions.Add(ciraExtension);

            return createContact;
        }*/

        public override ContactCreateResponse FromBytes(byte[] bytes)
        {
            return new ContactCreateResponse(bytes);
        }
    }
}