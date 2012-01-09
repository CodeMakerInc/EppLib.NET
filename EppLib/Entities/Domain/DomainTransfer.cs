using System;
using System.Collections.Generic;
using System.Xml;

namespace EppLib.Entities
{
    public class DomainTransfer : DomainBase<DomainTransferResponse>
    {
        private string m_name;
        protected string registrantContactId;
        protected string adminContactId;
        protected IList<string> techContactIds;

        public DomainTransfer(string mName)
        {
            m_name = mName;
        }

        public DomainTransfer(string mName, string registrantContactId, string adminContactId, string techContactId)
        {
            m_name = mName;
            this.techContactIds = new List<string> { techContactId };
            this.adminContactId = adminContactId;
            this.registrantContactId = registrantContactId;
        }

        public DomainTransfer(string mName, string registrantContactId, string adminContactId, IList<string> techContactIds)
        {
            m_name = mName;
            this.adminContactId = adminContactId;
            this.techContactIds = techContactIds;
            this.registrantContactId = registrantContactId;
        }

        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();

           var domainCreate = BuildCommandElement(doc, "transfer", "request" );

            AddXmlElement(doc, domainCreate, "domain:name", m_name,namespaceUri);

            return doc;
        }

        public override DomainTransferResponse FromBytes(byte[] bytes)
        {
            return new DomainTransferResponse(bytes);
        }
    }
}
