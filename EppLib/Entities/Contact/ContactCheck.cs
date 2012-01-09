using System.Collections.Generic;
using System.Xml;

namespace EppLib.Entities
{
    public class ContactCheck : ContactBase<ContactCheckResponse>
    {
        private readonly IList<string> contactIds;

        public ContactCheck(string contactId)
        {
            contactIds = new List<string> {contactId};
        }

        public ContactCheck(IList<string> contactIds)
        {
            this.contactIds = contactIds;
        }

        public override ContactCheckResponse FromBytes(byte[] bytes)
        {
            return new ContactCheckResponse(bytes);
        }

        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();
            
            var contact_check = BuildCommandElement(doc, "check");
           
            foreach (var contactId in contactIds)
            {
                AddXmlElement(doc, contact_check, "contact:id", contactId, namespaceUri);    
            }
            
            return doc;
        }

    }
}
