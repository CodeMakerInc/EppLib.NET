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

        protected override XmlElement BuildCommandElement(XmlDocument doc, XmlElement commandRootElement)
        {
            var domainRenew = BuildCommandElement(doc, "renew", commandRootElement);

            AddXmlElement(doc, domainRenew, "domain:name", DomainName, namespaceUri);
            AddXmlElement(doc, domainRenew, "domain:curExpDate", DateTime.Parse(CurrentExpirationDate, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), namespaceUri);

            if (m_period != null)
            {
                var period = AddXmlElement(doc, domainRenew, "domain:period", m_period.Value.ToString(CultureInfo.InvariantCulture), namespaceUri);
                period.SetAttribute("unit", m_period.Unit);
            }

            return domainRenew;
        }

        public override DomainRenewResponse FromBytes(byte[] bytes)
        {
            return new DomainRenewResponse(bytes);
        }
    }
}
