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

namespace EppLib.Entities
{
    public class ContactCheck : ContactBase<ContactCheckResponse>
    {
        private readonly IList<string> contactIds;

        public ContactCheck(string contactId)
        {
            contactIds = new List<string> {contactId};
        }

        public ContactCheck(IList<string> contactIds)
        {
            this.contactIds = contactIds;
        }

        public override ContactCheckResponse FromBytes(byte[] bytes)
        {
            return new ContactCheckResponse(bytes);
        }

        protected override XmlElement BuildCommandElement(XmlDocument doc, XmlElement commandRootElement)
        {
            var contact_check = BuildCommandElement(doc, "check", commandRootElement);

            foreach (var contactId in contactIds)
            {
                AddXmlElement(doc, contact_check, "contact:id", contactId, namespaceUri);
            }

            return contact_check;
        }
    }
}
