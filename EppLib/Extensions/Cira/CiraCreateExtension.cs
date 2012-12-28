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
	public class CiraCreateExtension : CiraExtensionBase
    {
        public string Language;
        public string OriginatingIpAddress;
        public string CprCategory;
        public string AgreementVersion;
        public string AggreementValue;
        public string CreatedByResellerId;
        
        public override XmlNode ToXml(XmlDocument doc)
        {
            var root = CreateElement(doc,"cira:ciraCreate");

            if (Language != null)
            {
                AddXmlElement(doc, root, "cira:language", Language);
            }
           
            if (CprCategory != null)
            {
                AddXmlElement(doc, root, "cira:cprCategory", CprCategory);
            }

            if (AgreementVersion != null)
            {
                AddXmlElement(doc, root, "cira:ciraAgreementVersion", AgreementVersion);
            }

            if (AggreementValue != null)
            {
                AddXmlElement(doc, root, "cira:agreementValue", AggreementValue);
            }

            //The CIRA Originating IP Address is valid only for Registrants with an assigned CPR category
            if (OriginatingIpAddress != null && CprCategory != null)
            {
                AddXmlElement(doc, root, "cira:originatingIpAddress", OriginatingIpAddress);
            }

            //The CIRA Created by Reseller ID is valid only for Registrants with an assigned CPR category
            if (CreatedByResellerId != null && CprCategory != null)
            {
                AddXmlElement(doc, root, "cira:createdByResellerId", CreatedByResellerId);
            }
            
            return root;
        }

    }
}
