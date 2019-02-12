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

namespace EppLib.Extensions.Fury
{
    /// <summary>
    /// Use it if your are setting up privacy
    /// </summary>
	public class FuryContactCreateExtension : FuryExtension
    {
        private Dictionary<string, string> properties = new Dictionary<string, string>();

        public FuryContactCreateExtension(string language, string cprCode, string agreement_version)
        {
            if (!string.IsNullOrEmpty(language)) { properties.Add("LANGUAGE", language); }
            if (!string.IsNullOrEmpty(cprCode)) { properties.Add("CPR", cprCode); }
            properties.Add("agreement_version", agreement_version);

        }

        public override XmlNode ToXml(XmlDocument doc)
        {
            var root = CreateElement(doc, "fury:create");

            var propertiesNode = CreateElement(doc, "fury:properties");

            foreach (var a in properties)
            {
                var propertyNode = CreateElement(doc, "fury:property");

                AddXmlElement(doc, propertyNode, "fury:key", a.Key);

                if (a.Value == null)
                {
                    var e = AddXmlElement(doc, propertyNode, "fury:value", "");
                    e.SetAttribute("default", "true");
                }
                else
                {
                    AddXmlElement(doc, propertyNode, "fury:value", a.Value);
                }

                propertiesNode.AppendChild(propertyNode);
            }

            root.AppendChild(propertiesNode);

            return root;
        }

    }
}
