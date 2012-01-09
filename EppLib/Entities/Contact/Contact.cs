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

namespace EppLib.Entities
{
    public class Contact
    {
        public Contact(string contactId, string fullName, string companyName, string city, string streetAddress, string province, string postalCode, string countryCode, string email, Telephone voice, Telephone fax)
        {
            var postalAddress = new PostalAddress
            {
                City = city,
                Street1 = streetAddress,
                StateProvince = province,
                PostalCode = postalCode,
                CountryCode = countryCode
            };

            PostalInfo = new PostalInfo
            {
                m_name = fullName,
                m_org = companyName,
                m_type = PostalAddressType.LOC,
                m_address = postalAddress
            };

            Email = email;
            Voice = voice;
            Fax = fax;
            Id = contactId;
            
        }

        public Contact()
        {
        }

        /// <summary>
        /// Contact Id (3 - 16 characters)
        /// </summary>
        public string Id;

        public Telephone Voice;
        public Telephone Fax;
        public string Email;

        public PostalInfo PostalInfo;
        public string Language;
        public string CprCategory;
        public string Individual;
        public string CiraAgreementVersion;
        public string AgreementTimestamp;
        public string WhoisDisplaySetting;

        public string Roid { get; set; }
        public string Status { get; set; }
        public string ClId { get; set; }
        public string CrId { get; set; }
        public string CrDate { get; set; }
    }
}
