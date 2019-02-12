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

namespace EppLib.Extensions.Fury
{
    public class GetAgreementResponse : EppResponse
    {
        public string Language;
        public string AgreementVersion;
        public string Agreement;

        public GetAgreementResponse(byte[] bytes) : base(bytes)
        {
            
        }

		protected override void ProcessExtensionNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            namespaces.AddNamespace("fury", "urn:ietf:params:xml:ns:fury-2.0");

            var children = doc.SelectSingleNode("/ns:epp/ns:response/ns:extension/fury:response/fury:infData", namespaces);

            if (children != null)
            {
                var crLanguage = children.SelectSingleNode("fury:language", namespaces);

                if (crLanguage != null)
                {
                    Language = crLanguage.InnerText;
                }

                var children1 = children.SelectSingleNode("fury:properties/fury:property", namespaces);

                if (children1 != null)
                {

                    var furyKey = children1.SelectSingleNode("fury:key", namespaces);
                    var furyLocalizedKey = children1.SelectSingleNode("fury:localizeKey", namespaces);
                    var furyProperyValues = children1.SelectSingleNode("fury:propertyValues/fury:propertyValue", namespaces);

                    if (furyProperyValues != null)
                    {
                        var furyValue = furyProperyValues.SelectSingleNode("fury:value", namespaces);

                        AgreementVersion = furyValue.InnerText;

                        var furyDetail = furyProperyValues.SelectSingleNode("fury:localizedDetail", namespaces);

                        Agreement = furyDetail.InnerText;
                    }
                }
                
            }
        }
    }
}