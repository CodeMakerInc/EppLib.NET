using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace EppLib.Entities
{
    public class ContactDelete : ContactBase<ContactDeleteResponse>
    {
        private string m_id;

        public ContactDelete(string mId)
        {
            m_id = mId;
        }

        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();

            var contact_delete = BuildCommandElement(doc, "delete");

            AddXmlElement(doc, contact_delete, "contact:id", m_id, namespaceUri);
            
            return doc;
        }

        public override ContactDeleteResponse FromBytes(byte[] bytes)
        {
            return new ContactDeleteResponse(bytes);
        }
    }
}
