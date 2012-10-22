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
using System;
using System.Collections.Generic;
using System.Xml;
using EppLib.Entities;

namespace EppLib.Extensions.Cira
{
	public class CiraTransferExtension : CiraExtensionBase
    {
        private IList<string> techContactIds = new List<string>();
        
        public IList<string> TechContactIds
        {
            get { return techContactIds; }
            set { techContactIds = value; }
        }

        public string AdminContactId { get; set; }

        public string RegistrantContactId { get; set; }

        public override XmlNode ToXml(XmlDocument doc)
        {
            var root = CreateElement(doc, "cira:ciraTransfer");

            var ciraChg = AddXmlElement(doc, root, "cira:ciraChg", "");


            if (!String.IsNullOrEmpty(RegistrantContactId))
            {
                AddXmlElement(doc, ciraChg, "cira:registrant", RegistrantContactId);
            }

            if(!String.IsNullOrEmpty(AdminContactId))
            {
                var adminContact = AddXmlElement(doc, ciraChg, "cira:contact", AdminContactId);
                adminContact.SetAttribute("type", "admin");
            }

            if (TechContactIds != null)
            {
                foreach (var techContactId in TechContactIds)
                {
                    if (techContactId != null)
                    {
                        var techContact = AddXmlElement(doc, ciraChg, "cira:contact", techContactId);
                        techContact.SetAttribute("type", "tech");
                    }
                }
            }

            return root;
        }
    }
}