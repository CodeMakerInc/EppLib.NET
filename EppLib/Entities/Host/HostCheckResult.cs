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
    public class HostCheckResult
    {
        public HostCheckResult(XmlNode child, XmlNamespaceManager namespaces)
        {
            var nameNode = child.SelectSingleNode("host:name", namespaces);

            if (nameNode != null)
            {

                Name = nameNode.InnerText;

                if (nameNode.Attributes != null)
                {
                    var xmlAttribute = nameNode.Attributes["avail"];

                    if (xmlAttribute != null) Available = xmlAttribute.Value.ToLower(CultureInfo.InvariantCulture).Equals("true");
                }
            }

            var reasonNode = child.SelectSingleNode("host:reason", namespaces);

            if (reasonNode != null)
            {
                Reason = reasonNode.InnerText;
            }

        }

        protected string Reason { get; set; }

        protected bool Available { get; set; }

        protected string Name { get; set; }
    }
}