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
using System.Globalization;
using System.Xml;

namespace EppLib.Entities
{
    public class ContactCheckResult
    {
        public string Id { get; set; }
        public bool Available { get; set; }
        public string Reason { get; set; }

        public ContactCheckResult()
        {
        }

        public ContactCheckResult(XmlNode child, XmlNamespaceManager namespaces)
        {
            var idNode = child.SelectSingleNode("contact:id", namespaces);

            if (idNode != null)
            {
                Id = idNode.InnerText;

                if (idNode.Attributes != null)
                {
                    var xmlAttribute = idNode.Attributes["avail"];

                    if (xmlAttribute != null)
                    {
                        var attributeValue = xmlAttribute.Value.ToLower(CultureInfo.InvariantCulture);
                        Available = attributeValue.Equals("true") || attributeValue.Equals("1");
                    }
                }
            }

            var reasonNode = child.SelectSingleNode("contact:reason", namespaces);

            if (reasonNode != null)
            {
                Reason = reasonNode.InnerText;
            }
        }

        public override bool Equals(object obj)
        {
            ContactCheckResult o = (ContactCheckResult)obj;

            if (o == null) return false;

            return (o.Id == Id) && (o.Available == Available) && (o.Reason == Reason);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Available.GetHashCode() ^ Reason.GetHashCode();
        }
    }
}
