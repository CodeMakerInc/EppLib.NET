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
using EppLib.Entities;

namespace EppLib.Extensions.Fury
{
    /// <summary>
    /// Use it if your are setting up privacy
    /// </summary>
	public class FuryContactUpdateExtension : FuryExtension
    {
        private readonly string oldLanguage;
        private readonly string newLanguage;

        public FuryContactUpdateExtension(string oldLanguage, string newLanguage)
        {
            this.oldLanguage = oldLanguage;
            this.newLanguage = newLanguage;
        }
        
        public override XmlNode ToXml(XmlDocument doc)
        {
            var root = CreateElement(doc,"fury:update");
            
            var add = CreateElement(doc,"fury:add");
            
            var propertiesNode = CreateElement(doc, "fury:properties");
            
            var propertyNode = CreateElement(doc, "fury:property");

            AddXmlElement(doc, propertyNode, "fury:key", "language");
            AddXmlElement(doc, propertyNode, "fury:value", newLanguage);

            propertiesNode.AppendChild(propertyNode);

            add.AppendChild(propertiesNode);

            root.AppendChild(add);
            
            
            var rem = CreateElement(doc,"fury:rem");
            
            var propertiesNode1 = CreateElement(doc, "fury:properties");
            
            var propertyNode1 = CreateElement(doc, "fury:property");

            AddXmlElement(doc, propertyNode1, "fury:key", "language");
            AddXmlElement(doc, propertyNode1, "fury:value", oldLanguage);

            propertiesNode1.AppendChild(propertyNode1);

            rem.AppendChild(propertiesNode1);

            root.AppendChild(rem);
            
            return root;
        }
    }
}
