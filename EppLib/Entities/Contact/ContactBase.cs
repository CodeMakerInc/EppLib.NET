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
    public abstract class ContactBase<T> : EppCommand<T> where T : EppResponse
    {
        protected ContactBase()
            : base("contact", "urn:ietf:params:xml:ns:contact-1.0", "urn:ietf:params:xml:ns:contact-1.0 contact-1.0.xsd")
        {
        }

        protected XmlElement AddressToXml(XmlDocument doc,
                                          String tagName,
                                          PostalInfo nameAddress)
        {
            XmlElement name_address_element = null;

            if (nameAddress != null)
            {
                name_address_element = doc.CreateElement(tagName, namespaceUri);

                if (nameAddress.m_type == null)
                {
                    throw new EppException("missing the address type (postal info)");
                }

                name_address_element.SetAttribute("type", nameAddress.m_type);

                if (nameAddress.m_name != null)
                {
                    AddXmlElement(doc, name_address_element, "contact:name", nameAddress.m_name, namespaceUri);
                }
                if (nameAddress.m_org != null)
                {
                    AddXmlElement(doc, name_address_element, "contact:org", nameAddress.m_org, namespaceUri);
                }

                if (nameAddress.m_address != null)
                {
                    var address = nameAddress.m_address;
                    var address_element = doc.CreateElement("contact:addr", namespaceUri);
                    // Because this method is used by contact create and update,
                    // the lowest common denominator (update), says that all
                    // members are optional.
                    if (address.Street1 != null)
                    {
                        AddXmlElement(doc, address_element, "contact:street", address.Street1, namespaceUri);
                    }
                    if (address.Street2 != null)
                    {
                        AddXmlElement(doc, address_element, "contact:street", address.Street2, namespaceUri);
                    }
                    if (address.Street3 != null)
                    {
                        AddXmlElement(doc, address_element, "contact:street", address.Street3, namespaceUri);
                    }
                    if (address.City != null)
                    {
                        AddXmlElement(doc, address_element, "contact:city", address.City, namespaceUri);
                    }
                    if (address.StateProvince != null)
                    {
                        AddXmlElement(doc, address_element, "contact:sp", address.StateProvince, namespaceUri);
                    }
                    if (address.PostalCode != null)
                    {
                        AddXmlElement(doc, address_element, "contact:pc", address.PostalCode, namespaceUri);
                    }
                    if (address.CountryCode != null)
                    {
                        AddXmlElement(doc, address_element, "contact:cc", address.CountryCode, namespaceUri);
                    }

                    if (address_element.ChildNodes.Count > 0)
                    {
                        name_address_element.AppendChild(address_element);
                    }
                }
            }

            return name_address_element;
        }

        protected XmlElement DiscloseToXml(XmlDocument doc, Contact.DiscloseFlags discloseMask, bool discloseFlag)
        {
            XmlElement discloseElement = doc.CreateElement("contact:disclose", namespaceUri);
            discloseElement.SetAttribute("flag", discloseFlag ? "1" : "0");

            var mask = discloseFlag ? discloseMask : ~discloseMask;

            if ((mask & Contact.DiscloseFlags.Voice) != 0)
            {
                AddXmlElement(doc, discloseElement, "contact:voice", null, namespaceUri);
            }
            if ((mask & Contact.DiscloseFlags.Fax) != 0)
            {
                AddXmlElement(doc, discloseElement, "contact:fax", null, namespaceUri);
            }
            if ((mask & Contact.DiscloseFlags.Email) != 0)
            {
                AddXmlElement(doc, discloseElement, "contact:email", null, namespaceUri);
            }
            if ((mask & Contact.DiscloseFlags.NameInt) != 0)
            {
                var nameInt = AddXmlElement(doc, discloseElement, "contact:name", null, namespaceUri);
                nameInt.SetAttribute("type", "int");
            }
            if ((mask & Contact.DiscloseFlags.NameLoc) != 0)
            {
                var nameLoc = AddXmlElement(doc, discloseElement, "contact:name", null, namespaceUri);
                nameLoc.SetAttribute("type", "loc");
            }
            if ((mask & Contact.DiscloseFlags.OrganizationInt) != 0)
            {
                var orgInt = AddXmlElement(doc, discloseElement, "contact:org", null, namespaceUri);
                orgInt.SetAttribute("type", "int");
            }
            if ((mask & Contact.DiscloseFlags.OrganizationLoc) != 0)
            {
                var orgLoc = AddXmlElement(doc, discloseElement, "contact:org", null, namespaceUri);
                orgLoc.SetAttribute("type", "loc");
            }
            if ((mask & Contact.DiscloseFlags.AddressInt) != 0)
            {
                var addrInt = AddXmlElement(doc, discloseElement, "contact:addr", null, namespaceUri);
                addrInt.SetAttribute("type", "int");
            }
            if ((mask & Contact.DiscloseFlags.AddressLoc) != 0)
            {
                var addrLoc = AddXmlElement(doc, discloseElement, "contact:addr", null, namespaceUri);
                addrLoc.SetAttribute("type", "loc");
            }

            return discloseElement;
        }
    }
}