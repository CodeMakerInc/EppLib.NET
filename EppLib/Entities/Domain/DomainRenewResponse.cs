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
namespace EppLib.Entities
{
    using System;
    using System.Xml;

    public class DomainRenewResponse : EppResponse
    {
        protected DateTime? _exDate;
        public virtual DateTime? ExDate
        {
            get { return _exDate; }
        }

        public DomainRenewResponse(byte[] bytes):base(bytes)
        {
            
        }

        protected override void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            namespaces.AddNamespace("domain", "urn:ietf:params:xml:ns:domain-1.0");
            var renData = doc.SelectSingleNode("//domain:renData", namespaces);

            if (renData != null)
            {
                var exDateNode = renData.SelectSingleNode("domain:exDate", namespaces);

                if (exDateNode != null)
                {
                    DateTime exDate;
                    if (DateTime.TryParse(exDateNode.InnerText, out exDate))
                    {
                        this._exDate = exDate;
                    }

                }
            }
        }


    }
}