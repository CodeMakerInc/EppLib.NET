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

using System.Xml;

namespace EppLib.Extensions.Fury
{
    /// <summary>
    /// Use it if your are setting up privacy
    /// </summary>
	public class FuryDomainCreateExtension : FuryExtension
    {
        private readonly bool isPrivate;

        public FuryDomainCreateExtension(bool isPrivate)
        {
            this.isPrivate = isPrivate;
        }

        public override XmlNode ToXml(XmlDocument doc)
        {
            var root = CreateElement(doc, "fury:create");
            var propertiesNode = CreateElement(doc, "fury:properties");
            var propertyNode = CreateElement(doc, "fury:property");
            AddXmlElement(doc, propertyNode, "fury:key", "PRIVACY");
            AddXmlElement(doc, propertyNode, "fury:value", isPrivate ? "PRIVATE" : "PUBLIC");
            propertiesNode.AppendChild(propertyNode);
            root.AppendChild(propertiesNode);

            return root;
        }

    }
}
