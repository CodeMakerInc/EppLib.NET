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
    public class ContactChange
    {
        public PostalInfo PostalInfo;
        public Telephone Voice;
        public Telephone Fax;
        public string Email;
        public bool? DiscloseFlag;
        public Contact.DiscloseFlags DiscloseMask;

        public ContactChange(Contact contact)
        {
            this.Email = contact.Email;
            this.Voice = contact.Voice;
            this.Fax = contact.Fax;

            this.PostalInfo = contact.PostalInfo;
            this.DiscloseFlag = contact.DiscloseFlag;
            this.DiscloseMask = contact.DiscloseMask;
        }

        public ContactChange()
        {
        }
    }
}