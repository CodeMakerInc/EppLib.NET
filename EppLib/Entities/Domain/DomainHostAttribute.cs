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
using System.Collections.Generic;
using System.Xml;

namespace EppLib.Entities
{
    /// <summary>
    /// A name server given by name and, for hosts inside the domain, IP addresses: the domain:hostAttr
    /// form of RFC 5731. Registries that do not use separate host objects expect this form.
    /// </summary>
    public class DomainHostAttribute
    {
        public DomainHostAttribute()
        {
            Addresses = new List<HostAddress>();
        }

        public DomainHostAttribute(string hostName, params HostAddress[] addresses)
        {
            HostName = hostName;
            Addresses = new List<HostAddress>(addresses);
        }

        public string HostName { get; set; }

        /// <summary>
        /// Glue addresses; set IPVersion to "v4" or "v6".
        /// </summary>
        public IList<HostAddress> Addresses { get; private set; }

        internal XmlElement ToXml(XmlDocument doc, string namespaceUri)
        {
            var hostAttr = doc.CreateElement("domain:hostAttr", namespaceUri);

            var hostName = doc.CreateElement("domain:hostName", namespaceUri);
            hostName.InnerText = HostName;
            hostAttr.AppendChild(hostName);

            foreach (var address in Addresses)
            {
                var hostAddr = doc.CreateElement("domain:hostAddr", namespaceUri);
                hostAddr.InnerText = address.IPAddress;

                if (address.IPVersion != null)
                {
                    hostAddr.SetAttribute("ip", address.IPVersion);
                }

                hostAttr.AppendChild(hostAddr);
            }

            return hostAttr;
        }
    }
}
