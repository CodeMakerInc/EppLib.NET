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
using System.Collections.Generic;
using System.Xml;

namespace EppLib.Entities
{
    public abstract class DomainBase<T> : EppCommand<T> where T : EppResponse
    {
        protected DomainBase()
            : base("domain", "urn:ietf:params:xml:ns:domain-1.0", "urn:ietf:params:xml:ns:domain-1.0 domain-1.0.xsd")
        {
        }

        protected XmlNode CreateNameServerElement(XmlDocument doc, IEnumerable<string> nameServers)
        {
            var nameServerElement = doc.CreateElement("domain:ns", namespaceUri);

            foreach (var serverName in nameServers)
            {
                AddXmlElement(doc, nameServerElement, "domain:hostObj", serverName,namespaceUri);
            }

            return nameServerElement;
        }

        /// <summary>
        /// Builds domain:ns from either host object names (hostObj) or host attributes (hostAttr); RFC 5731 allows one form per element.
        /// </summary>
        protected XmlNode CreateNameServerElement(XmlDocument doc, ICollection<string> hostObjects, ICollection<DomainHostAttribute> hostAttributes)
        {
            if (hostObjects.Count > 0 && hostAttributes.Count > 0)
            {
                throw new InvalidOperationException("domain:ns takes either host objects (NameServers) or host attributes (NameServerAttributes), not both (RFC 5731 section 1.1).");
            }

            if (hostObjects.Count > 0)
            {
                return CreateNameServerElement(doc, hostObjects);
            }

            var nameServerElement = doc.CreateElement("domain:ns", namespaceUri);

            foreach (var hostAttribute in hostAttributes)
            {
                nameServerElement.AppendChild(hostAttribute.ToXml(doc, namespaceUri));
            }

            return nameServerElement;
        }
    }
}
