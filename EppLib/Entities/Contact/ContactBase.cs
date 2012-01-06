using System;
using System.Xml;

namespace EppLib.Entities
{
    public abstract class ContactBase<T> : EppCommand<T> where T : EppResponse
    {
        protected ContactBase()
            : base("contact", "urn:ietf:params:xml:ns:contact-1.0")
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
                    var address_element = doc.CreateElement("contact:addr",namespaceUri);
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
    }
}