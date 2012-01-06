using System.Xml;

namespace EppLib.Entities
{
    public class DomainUpdate : DomainBase<DomainUpdateResponse>
    {
        private readonly string domainName;

        private readonly EppDomainUpdateAddRemove toAdd = new EppDomainUpdateAddRemove();
        private readonly EppDomainUpdateAddRemove toRemove = new EppDomainUpdateAddRemove();

        public DomainUpdate(string domainName)
        {
            this.domainName = domainName;
        }

        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();

            var domainUpdate = BuildCommandElement(doc, "update");

            AddXmlElement(doc, domainUpdate, "domain:name", domainName, namespaceUri);

            var add_element = getAddRemoveElement(doc, ToAdd, "domain:add", namespaceUri);

            if (add_element != null)
            {
                domainUpdate.AppendChild(add_element);
            }

            var remove_element = getAddRemoveElement(doc, ToRemove, "domain:rem", namespaceUri);

            if (remove_element != null)
            {
                domainUpdate.AppendChild(remove_element);
            }

            if (DomainChange != null)
            {
                var change_element = doc.CreateElement("domain:chg", namespaceUri);

                if (DomainChange.RegistrantContactId != null)
                {
                    AddXmlElement(doc, change_element, "domain:registrant", DomainChange.RegistrantContactId, namespaceUri);
                }

                domainUpdate.AppendChild(change_element);
            }

            return doc;
        }

        public DomainChange DomainChange { get; set; }

        public EppDomainUpdateAddRemove ToAdd
        {
            get
            {
                return toAdd;
            }

        }

        public EppDomainUpdateAddRemove ToRemove
        {
            get
            {
                return toRemove;
            }

        }

        private XmlElement getAddRemoveElement(XmlDocument doc, EppDomainUpdateAddRemove add_remove_items, string tag_name, string namespaceURI)
        {
            XmlElement add_remove_element = null;

            if (add_remove_items != null)
            {

                if (add_remove_items.NameServers != null &&
                     add_remove_items.NameServers.Count > 0)
                {
                    add_remove_element = doc.CreateElement(tag_name, namespaceURI);

                    add_remove_element.AppendChild(CreateNameServerElement(doc, add_remove_items.NameServers));
                }

                if (add_remove_items.DomainContacts != null &&
                     add_remove_items.DomainContacts.Count > 0)
                {
                    if (add_remove_element == null)
                    {
                        add_remove_element = doc.CreateElement(tag_name, namespaceURI);
                    }

                    foreach (var domain_contact in add_remove_items.DomainContacts)
                    {
                        var contact_element = AddXmlElement(doc, add_remove_element, "domain:contact", domain_contact.Id, namespaceURI);

                        contact_element.SetAttribute("type", domain_contact.Type);
                    }
                }

                if (add_remove_items.Status != null &&
                     add_remove_items.Status.Count > 0)
                {
                    if (add_remove_element == null)
                    {
                        add_remove_element = doc.CreateElement(tag_name, namespaceURI);
                    }

                    foreach (var status in add_remove_items.Status)
                    {
                        var status_element = AddXmlElement(doc, add_remove_element, "domain:status", status.Value, namespaceURI);

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

        public override DomainUpdateResponse FromBytes(byte[] bytes)
        {
            return new DomainUpdateResponse(bytes);
        }
    }
}
