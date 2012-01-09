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

namespace EppLib.Entities
{
    public class HostUpdate : HostBase<HostUpdateResponse>
    {
        private readonly string HostName;

        public EppHostUpdateAddRemove ToAdd;
        public EppHostUpdateAddRemove ToRemove;

        public HostChange HostChange;

        public HostUpdate(string hostName)
        {
            HostName = hostName;
        }

        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();

            var domainUpdate = BuildCommandElement(doc, "update");

            AddXmlElement(doc, domainUpdate, "host:name", HostName, namespaceUri);

            var add_element = getAddRemoveElement(doc, ToAdd, "host:add", namespaceUri);

            if (add_element != null)
            {
                domainUpdate.AppendChild(add_element);
            }

            var remove_element = getAddRemoveElement(doc, ToRemove, "host:rem", namespaceUri);

            if (remove_element != null)
            {
                domainUpdate.AppendChild(remove_element);
            }

            if (HostChange != null)
            {
                var change_element = doc.CreateElement("host:chg");

                if (HostChange.HostName != null)
                {
                    AddXmlElement(doc, change_element, "host:name", HostChange.HostName, namespaceUri);
                }

                domainUpdate.AppendChild(change_element);
            }

            return doc;
        }

        private static XmlElement getAddRemoveElement(XmlDocument doc, EppHostUpdateAddRemove add_remove_items, string tag_name, string namespaceUri)
        {
            XmlElement add_remove_element = null;

            if (add_remove_items != null)
            {

                if (add_remove_items.Adresses != null &&
                     add_remove_items.Adresses.Count > 0)
                {
                    add_remove_element = doc.CreateElement(tag_name, namespaceUri);

                    foreach (var address in add_remove_items.Adresses)
                    {
                        var address_element = AddXmlElement(doc, add_remove_element, "host:addr", address.IPAddress, namespaceUri);

                        address_element.SetAttribute("ip", address.IPVersion);
                    }
                    

                }

                if (add_remove_items.Status != null &&
                     add_remove_items.Status.Count > 0)
                {
                    if (add_remove_element == null)
                    {
                        add_remove_element = doc.CreateElement(tag_name, namespaceUri);
                    }

                    foreach (var status in add_remove_items.Status)
                    {

                        var status_element = AddXmlElement(doc, add_remove_element, "host:status", status.Value, namespaceUri);

                        status_element.SetAttribute("s", status.Type);

                        if (status.Lang != null)
                        {
                            status_element.SetAttribute("lang", status.Lang);
                        }
                    }
                }

            }


            return add_remove_element;
        }

        public override HostUpdateResponse FromBytes(byte[] bytes)
        {
            return new HostUpdateResponse(bytes);
        }
    }
}
