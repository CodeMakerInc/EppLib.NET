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
    class FuryGetAgreementExtension : FuryExtension
    {
        private string language;
        private string agreement_version;

        public FuryGetAgreementExtension(string language, string agreement_version)
        {
            this.language = language;
            this.agreement_version = agreement_version;
        }

        public override XmlNode ToXml(XmlDocument doc)
        {
            var root = CreateElement(doc, "fury:command");

            var info = CreateElement(doc, "fury:info");

            root.AppendChild(info);

            AddXmlElement(doc, info, "fury:language", language);

            var propertiesNode = CreateElement(doc, "fury:properties");
            info.AppendChild(propertiesNode);

            var propertyNode = CreateElement(doc, "fury:property");
            propertiesNode.AppendChild(propertyNode);

            AddXmlElement(doc, propertyNode, "fury:key", "AGREEMENT_VERSION");
            var e = AddXmlElement(doc, propertyNode, "fury:value", "");
            e.SetAttribute("default","true");

            return root;
        }
    }
}
