using System;
using System.Xml;

namespace EppLib.Entities
{
    public class ContactInfo : ContactBase<ContactInfoResponse>
    {
        private string m_id;

        public ContactInfo(string mId)
        {
            m_id = mId;
        }

        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();

            var contact_info = BuildCommandElement(doc, "info");

            AddXmlElement(doc, contact_info, "contact:id", m_id, namespaceUri);

            return doc;
        }

        public override ContactInfoResponse FromBytes(byte[] bytes)
        {
            return new ContactInfoResponse(bytes);
        }
    }
}
