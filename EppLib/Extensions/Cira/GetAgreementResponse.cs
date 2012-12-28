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
            namespaces.AddNamespace("cira", "urn:ietf:params:xml:ns:cira-1.0");

            var children = doc.SelectSingleNode("/ns:epp/ns:response/ns:extension/cira:ciraInfo", namespaces);

            if (children != null)
            {
                var crLanguage = children.SelectSingleNode("cira:language", namespaces);

                if (crLanguage != null)
                {
                    Language = crLanguage.InnerText;
                }

                var crCiraAgreementVersion = children.SelectSingleNode("cira:ciraAgreementVersion", namespaces);

                if (crCiraAgreementVersion != null)
                {
                    AgreementVersion = crCiraAgreementVersion.InnerText;
                }

                var crAgreement = children.SelectSingleNode("cira:ciraAgreement", namespaces);

                if (crAgreement != null)
                {
                    Agreement = crAgreement.InnerText;
                }

                
            }
        }
    }
}