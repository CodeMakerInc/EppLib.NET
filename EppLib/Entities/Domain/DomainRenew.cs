using System;
using System.Globalization;
using System.Xml;

namespace EppLib.Entities
{
    public class DomainRenew : DomainBase<DomainRenewResponse>
    {
        private string DomainName;
        private string CurrentExpirationDate;
        private DomainPeriod m_period;

        public DomainRenew(string domainName, string currentExpirationDate, DomainPeriod mPeriod)
        {
            DomainName = domainName;
            CurrentExpirationDate = currentExpirationDate;
            m_period = mPeriod;
        }

        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();

            var domainRenew = BuildCommandElement(doc, "renew");

            AddXmlElement(doc, domainRenew, "domain:name", DomainName, namespaceUri);
            AddXmlElement(doc, domainRenew, "domain:curExpDate", DateTime.Parse(CurrentExpirationDate,CultureInfo.InvariantCulture).ToString("yyyy-MM-dd",CultureInfo.InvariantCulture), namespaceUri);

            if (m_period != null)
            {
                var period = AddXmlElement(doc, domainRenew, "domain:period", m_period.Value.ToString(CultureInfo.InvariantCulture));
                period.SetAttribute("unit", m_period.Unit);
            }

            return doc;
        }

        public override DomainRenewResponse FromBytes(byte[] bytes)
        {
            return new DomainRenewResponse(bytes);
        }
    }
}
