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
using EppLib.Entities;

namespace EppLib.Extensions.Cira
{
	public class CiraContactUpdateExtension : CiraExtensionBase
    {
        public string Language;
        public string CprCategory;
        public string WhoisDisplaySetting;

        public override XmlNode ToXml(XmlDocument doc)
        {
            var root = CreateElement(doc, "cira:ciraUpdate");

            var ciraChg = AddXmlElement(doc, root, "cira:ciraChg", "");
            
            if (Language != null)
            {
                AddXmlElement(doc, ciraChg, "cira:language", Language);
            }

            if (CprCategory != null)
            {
                AddXmlElement(doc, ciraChg, "cira:cprCategory", CprCategory);
            }

            if (WhoisDisplaySetting != null)
            {
                AddXmlElement(doc, ciraChg, "cira:whoisDisplaySetting", WhoisDisplaySetting);
            }

            return root;
        }
    }
}