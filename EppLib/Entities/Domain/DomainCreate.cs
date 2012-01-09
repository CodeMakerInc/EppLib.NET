using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace EppLib.Entities
{
    public class DomainCreate : DomainBase<DomainCreateResponse>
    {
        private readonly IList<string> nameServers = new List<string>();

        private readonly IList<DomainContact> domainContacts = new List<DomainContact>();

        public string DomainName { get; set; }

        public DomainPeriod Period { get; set; }

        public string RegistrantContactId { get; set; }

        public IList<string> NameServers
        {
            get { return nameServers; }
        }

        public IList<DomainContact> DomainContacts
        {
            get { return domainContacts; }
        }

        public DomainCreate(string domainName, string registrantContactId)
        {
            DomainName = domainName;

            RegistrantContactId = registrantContactId;
        }

       public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();

            var domainCreate = BuildCommandElement(doc, "create");

            AddXmlElement(doc, domainCreate, "domain:name", DomainName, namespaceUri);

            if (Period != null)
            {
                var period = AddXmlElement(doc, domainCreate, "domain:period", Period.Value.ToString(CultureInfo.InvariantCulture), namespaceUri);

                period.SetAttribute("unit", Period.Unit);
            }

            if (NameServers != null && NameServers.Count>0)
            {
                domainCreate.AppendChild(CreateNameServerElement(doc, NameServers));
            }
            
            if (RegistrantContactId != null)
            {
                AddXmlElement(doc, domainCreate, "domain:registrant", RegistrantContactId, namespaceUri);
            }
            
            foreach (var contact in DomainContacts)
            {
                var contact_element = AddXmlElement(doc, domainCreate, "domain:contact", contact.Id,namespaceUri);

                contact_element.SetAttribute("type", contact.Type);
            }

            return doc;
        }

        public override DomainCreateResponse FromBytes(byte[] bytes)
        {
            return new DomainCreateResponse(bytes);
        }
    }
}
